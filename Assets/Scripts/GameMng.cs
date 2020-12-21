﻿using System;
using System.Linq;
using Assets.Scripts.BuyMng;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.CharacterBuyPanel;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Fight;
using Assets.Scripts.FightText;
using Assets.Scripts.Items;
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
using UnityEngine.UI;

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

        private Vector3 mousePosition;

        private CharacterCtrl selectedCharacterCtrl;

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

        public GameTitlePanel locationTitle;

        public GameData gameData;

        [SerializeField]
        private Text selectedCharacterName;

        [SerializeField]
        private Animator blackOverlayAnim;

        public MessagePanel messagePanel;

        public RoomCtrl roomCtrl;

        public RerollCtrl rerollCtrl = new RerollCtrl();

        public SelectCharacterEvent onSelectCharacter = new SelectCharacterEvent();

        [SerializeField]
        private GameInputCtrl gameInputCtrl;

        public PathToCell pathToCell { get; private set; }

        public ObservableVal<int> countLevels { get; private set; }

        public ObservableVal<int> level { get; private set; }

        private void Awake() {
            ShowBlackOverlay();
        }

        private void Start() {

            level = new ObservableVal<int>(0);
            countLevels = new ObservableVal<int>(currentDungeonData.countRooms.GetRandomLvl());

            TagLogger<GameMng>.Info($"Set count rooms - {countLevels}");

            playerData.Init();

            locationTitle.HidePopup();
            messagePanel.Hide();

            dropCtrl.targetItemsContainer = playerData.itemsContainer;

            roomCtrl.Init();
            roomCtrl.MoveToStartRoom();

            cellsGridMng.Init();

            buyMng.Init();
            buyPanelUI.Init();
            topSidePanelUI.Init();

            // Set up, synergy data
            buyMng.postBuy.AddListener(() => {

                synergyCtrl.SetUpTeam(fightMng.fightTeamPlayer);
            });

            playerInventoryGrid.SetItemsContainer(playerData.itemsContainer);

            // Update ui on select character ctrl
            gameInputCtrl.onSelectCharacter.AddListener((ctrl) => {

                if (ctrl != null) {

                    selectedCharacterName.text = ctrl.characterData.characterName.val;
                    statsGrid.SetCharacter(ctrl.characterData);
                    characterInventoryGrid.SetItemsContainer(ctrl.characterData.itemsContainer);
                    characterSpellsList.SetSpellsContainer(ctrl.characterData.spellsContainer);
                    buffsListPanel.SetBuffsContainer(ctrl.characterData.buffsContainer);

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

        private ItemData[] GetItemsForDropChanches(DropChance[] drops, int coutItems) {
            return drops[0].items.Take(coutItems).ToArray();
        }

        public class SelectCharacterEvent : UnityEvent<CharacterCtrl>{}

    }

}
