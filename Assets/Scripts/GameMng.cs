using System;
using System.Linq;
using Assets.Scripts.BuyMng;
using Assets.Scripts.CameraMng;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.CharacterBuyPanel;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Fight;
using Assets.Scripts.FightText;
using Assets.Scripts.Items;
using Assets.Scripts.Player;
using Assets.Scripts.Synergy;
using Assets.Scripts.TopSidePanel;
using Assets.Scripts.UI;
using Assets.Scripts.UI.BuffsList;
using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.MessagePopup;
using Assets.Scripts.UI.SelectPopup;
using Assets.Scripts.UI.SpellsList;
using UnityEngine;

namespace Assets.Scripts {

    public class GameMng : MonoBehaviour {

        private static GameMng _current;

        public static GameMng current {
            get {

                if (_current == null)
                    _current = FindObjectOfType<GameMng>();

                return _current;
            }
        }

        private MoveItemSystem _moveItemSystem;

        public MoveItemSystem moveItemSystem {
            get {
                if (_moveItemSystem == null)
                    _moveItemSystem = new MoveItemSystem();

                return _moveItemSystem;
            }
        }

        public CellsGridMng cellsGridMng;

        public FightMng fightMng;

        public CharacterBuyMng buyMng;

        public PlayerData playerData;

        public StoreData storeData;

        public CharacterBuyPanelUI buyPanelUI;

        public SelectPanel selectPanel;

        public TopSidePanelUI topSidePanelUI;

        public FightTextMng fightTextMng;

        public DungeonData currentDungeonData;

        public SynergyCtrl synergyCtrl;

        public DungeonObjectGenerator dungeonGenerator;

        public SynergyLines synergyLines;

        public StatsGrid statsGrid;

        public InventoryGrid characterInventoryGrid;

        public InventoryGrid playerInventoryGrid;

        public MoveItemFactory moveItemFactory;

        public CharacterSpellsList characterSpellsList;

        public BuffsListPanel buffsListPanel;

        public ItemInfoPanel itemInfoPanel;

        public DropCtrl dropCtrl;

        public GameData gameData;

        [SerializeField]
        private Animator blackOverlayAnim;

        public MessagePanel messagePanel;

        public RoomCtrl roomCtrl;

        public int level { get; private set; }

        public int roomLvl { get; set; }

        private DungeonDataGenerator dungeonDataGenerator = new DungeonDataGenerator();

        private void Awake() {
            ShowBlackOverlay();
        }

        private void Start() {

            playerData.Init();

            messagePanel.Hide();

            dropCtrl.targetItemsContainer = playerData.itemsContainer;

            roomCtrl.Init();
            roomCtrl.MoveToStartRoom();

            //dungeonGenerator.Generate(dungeonDataGenerator.GetDungeonData());
            cellsGridMng.Init();

            buyMng.Init();
            buyPanelUI.Init();
            topSidePanelUI.Init();

            // On win, add money to player
            fightMng.onPlayerWin.AddListener(() => {

                playerData.money.val += currentDungeonData.moneyVictory;

                var drops = gameData.GetDropChances(level);
                var items = GetItemsForDropChanches(drops, 3);

                selectPanel.SetItems((items[0], items[1], items[2]));
                selectPanel.Show();

            });

            // Set up, synergy data
            buyMng.postBuy.AddListener(() => {
                synergyCtrl.SetUpTeam(fightMng.fightTeamPlayer);
            });

            playerInventoryGrid.SetItemsContainer(playerData.itemsContainer);

            //Camera.main.GetComponent<CameraCtrl>().ToRoom();

            HideBlackOverlay();

        }

        private void Update() {

            if (Input.GetMouseButtonDown(0)) {

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask(LayersStore.CELL_LAYER, LayersStore.CHARACTER_LAYER))) {

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer(LayersStore.CHARACTER_LAYER)) {

                        var ctrl = hit.collider.GetComponent<CharacterCtrl>();

                        if (ctrl == null || ctrl.characterData == null)
                            throw new Exception(hit.collider.gameObject.name);

                        statsGrid.SetCharacter(ctrl.characterData);
                        characterInventoryGrid.SetItemsContainer(ctrl.characterData.itemsContainer);
                        characterSpellsList.SetSpellsContainer(ctrl.characterData.spellsContainer);
                        buffsListPanel.SetBuffsContainer(ctrl.characterData.buffsContainer);


                    } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer(LayersStore.CELL_LAYER)) {

                        if (buyPanelUI.selectedBuyData != null && buyPanelUI.selectedBuyData.IsCanBuy()) {

                            var cell = hit.collider.GetComponent<Cell>();

                            if (cell != null && cell.GetState() == Cell.CellState.Select) {

                                var ctrl = current.buyMng.Buy(current.buyPanelUI.selectedBuyData);
                                cell.StayCtrl(ctrl);

                                if (!current.buyPanelUI.selectedBuyData.IsCanBuy()) {
                                    current.buyPanelUI.selectedBuyData = null;
                                }

                            }

                        }

                    }
                }

            }
        }

        public void ShowBlackOverlay() {
            blackOverlayAnim.SetBool(AnimationValStore.IS_SHOW, true);
        }

        public void HideBlackOverlay() {
            blackOverlayAnim.SetBool(AnimationValStore.IS_SHOW, false);
        }

        private ItemData[] GetItemsForDropChanches(DropChance[] drops, int coutItems) {
            return drops[0].items.Take(coutItems).ToArray();
        }

    }

}
