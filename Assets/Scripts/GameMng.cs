using Assets.Scripts.BuyMng;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.CharacterBuyPanel;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Fight;
using Assets.Scripts.FightText;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Player;
using Assets.Scripts.Synergy;
using Assets.Scripts.TopSidePanel;
using Assets.Scripts.UI;
using Assets.Scripts.UI.BuffsList;
using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.GameTitlePopup;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.MessagePopup;
using Assets.Scripts.UI.SelectPopup;
using Assets.Scripts.UI.SpellsList;
using UnityEngine;
using UnityEngine.Events;

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

        public SynergyLines synergyLines;

        public StatsGrid statsGrid;

        public InventoryGrid characterInventoryGrid;

        public InventoryGrid playerInventoryGrid;

        public MoveItemFactory moveItemFactory;

        public CharacterSpellsList characterSpellsList;

        public BuffsListPanel buffsListPanel;

        public ItemInfoPanel itemInfoPanel;

        public DropCtrl dropCtrl;

        public GameTitlePanel locationTitle;

        public GameData gameData;

        [SerializeField]
        private Animator blackOverlayAnim;

        public MessagePanel messagePanel;

        public RoomCtrl roomCtrl;

        public RerollCtrl rerollCtrl = new RerollCtrl();

        public GameInputCtrl gameInputCtrl;

        [SerializeField]
        private NameOfCharacter nameOfCharacter;

        public PathToCell pathToCell { get; private set; }

        public int levelsBeforeChange { get; set; }

        public ObservableVal<int> level { get; private set; }

        public int levelDifficult { get; private set; } = -1;

        public ObservableVal<bool> isBuildPhase = new ObservableVal<bool>();

        private void Awake() {
            ShowBlackOverlay();
        }

        public bool IsNextBossRoom()
            => levelsBeforeChange == 1;

        public void RefeshLevelsToNextDifficult() {
            
            levelsBeforeChange = new ObservableVal<int>(currentDungeonData.changeDifficultEvery.GetRandomLvl());
            levelDifficult++;

        }

        private void Start() {

            level = new ObservableVal<int>(1);

            RefeshLevelsToNextDifficult();

            TagLogger<GameMng>.Info($"Set levels before change difficult - {levelsBeforeChange}");

            playerData.Init();

            locationTitle.HidePopup();
            messagePanel.Hide();

            dropCtrl.targetItemsContainer = playerData.itemsContainer;

            roomCtrl.Init();
            roomCtrl.MoveToStartRoom();

            buyMng.Init();
            buyPanelUI.Init();
            topSidePanelUI.Init();

            // Set up, synergy data
            buyMng.postBuy.AddListener(() => {
                synergyCtrl.SetUpTeam(fightMng.fightTeamPlayer);
            });

            playerInventoryGrid.SetItemsContainer(playerData.itemsContainer);

            // Update ui on select character ctrl
            gameInputCtrl.onChangeSelectCharacter.AddListener((old, ctrl) => {

                if (old != ctrl) {

                    if (old != null) {
                        //old.HideWhenDeselected();
                    }

                    if (ctrl != null) {

                        nameOfCharacter.SetCharacterData(ctrl.characterData);
                        statsGrid.SetCharacter(ctrl.characterData);
                        characterInventoryGrid.SetItemsContainer(ctrl.characterData.itemsContainer);
                        characterSpellsList.SetSpellsContainer(ctrl.characterData.spellsContainer);
                        buffsListPanel.SetBuffsContainer(ctrl.characterData.buffsContainer);

                        //ctrl.ShowWhenSelected();

                    }

                }

            });

            HideBlackOverlay();

            pathToCell = new PathToCell(cellsGridMng);

        }

        public void ShowBlackOverlay() {
            blackOverlayAnim.SetBool(AnimationValStore.IS_SHOW, true);
        }

        public void HideBlackOverlay() {
            blackOverlayAnim.SetBool(AnimationValStore.IS_SHOW, false);
        }

        public class SelectCharacterEvent : UnityEvent<CharacterCtrl> { }

    }

}
