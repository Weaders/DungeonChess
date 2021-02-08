using Assets.Scripts.BuyMng;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
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
using Assets.Scripts.Translate;
using Assets.Scripts.UI;
using Assets.Scripts.UI.CharacterBuyPanel;
using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.GameTitlePopup;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.MessagePopup;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

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

        [SerializeField]
        private CharacterImgCtrl characterImgCtrl;

        public TopSidePanelUI topSidePanelUI;

        [SerializeField]
        private CharacterOpenInfoBtn characterOpenInfoBtn;

        public FightTextMng fightTextMng;

        public DungeonData currentDungeonData;

        public SynergyCtrl synergyCtrl;

        public SynergyLines synergyLines;

        public StatsGrid statsGrid;

        public InventoryGrid characterInventoryGrid;

        public InventoryGrid playerInventoryGrid;

        public MoveItemFactory moveItemFactory;

        public ItemInfoPanel itemInfoPanel;

        public LevelUpService levelUpService;

        public DropCtrl dropCtrl;

        public GameTitlePanel locationTitle;

        public CharacterHealService characterHealService;

        public GameData gameData;

        [SerializeField]
        private Animator blackOverlayAnim;

        public MessagePanel messagePanel;

        public RoomCtrl roomCtrl;

        public RerollCtrl rerollCtrl = new RerollCtrl();

        public GameInputCtrl gameInputCtrl;

        public ArrowSelectCtrl arrowCtrl;

        [SerializeField]
        private NameOfCharacter nameOfCharacter;

        public PathToCell pathToCell { get; private set; }

        public int initLevelsBeforeChange { get; set; }

        public ObservableVal<int> levelsBeforeChange { get; set; }

        public ObservableVal<int> level { get; private set; }

        public int levelDifficult { get; private set; } = -1;

        public ObservableVal<bool> isBuildPhase = new ObservableVal<bool>();

        private void Awake() {
            ShowBlackOverlay();
        }

        public bool IsNextBossRoom()
            => levelsBeforeChange == 1;

        public bool IsNextBuildRoom()
            => levelsBeforeChange == 0;

        public void RefeshLevelsToNextDifficult() {

            levelsBeforeChange = new ObservableVal<int>(currentDungeonData.changeDifficultEvery.GetRandomLvl());
            initLevelsBeforeChange = levelsBeforeChange;
            levelDifficult++;

        }

        private void Start() {

            level = new ObservableVal<int>(1);

            RefeshLevelsToNextDifficult();

            TagLogger<GameMng>.Info($"Set levels before change difficult - {levelsBeforeChange}");

            characterOpenInfoBtn.Init();

            playerData.Init();
            characterHealService.Init();

            locationTitle.HidePopup();
            messagePanel.Hide();

            dropCtrl.targetItemsContainer = playerData.itemsContainer;

            roomCtrl.Init();
            roomCtrl.MoveToStartRoom();

            buyMng.Init();
            buyPanelUI.Init();
            topSidePanelUI.Init();

            characterImgCtrl.Init();
            arrowCtrl.Init();

            gameInputCtrl.Init();

            levelUpService.Init();

            playerInventoryGrid.SetItemsContainer(playerData.itemsContainer, true);

            // Update ui on select character ctrl
            gameInputCtrl.onChangeSelectCharacter.AddListener((old, ctrl) => {

                if (old != ctrl) {

                    if (ctrl != null) {

                        var sideOfCtrl = fightMng.GetTeamSide(ctrl);

                        nameOfCharacter.SetCharacterData(ctrl.characterData);
                        statsGrid.SetCharacter(ctrl.characterData);
                        characterInventoryGrid.SetItemsContainer(ctrl.characterData.itemsContainer, sideOfCtrl == TeamSide.Player);

                    }

                }

            });

            fightMng.onEnemyTeamWin.AddListener(() => {

                StaticData.current.TrySetLevevRecord(level);

                messagePanel.SetData(new MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("you_lose_you_ned_room", new Placeholder("count_rooms", level)),
                    btns = new[] {
                        new BtnData(TranslateReader.GetTranslate("restart"), () => {
                            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                        })
                    }
                });

                messagePanel.Show();

            });

            level.onPostChange.AddSubscription(OrderVal.UIUpdate, () => {

                if (level == gameData.victoryLevel) {

                    messagePanel.SetData(new MessagePanel.MessageData {
                            msg = TranslateReader.GetTranslate("victory"),
                            btns = new[] {
                            new BtnData(TranslateReader.GetTranslate("i_am_cool"), messagePanel.Hide)
                        }
                    });

                    messagePanel.Show();

                }

            });

            HideBlackOverlay();

            pathToCell = new PathToCell(cellsGridMng);

            if (StaticData.current.IsNeedShowTutorial()) {

                messagePanel.SetData(new MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("tutorial"),
                    btns = new[] {
                    new BtnData(TranslateReader.GetTranslate("ok"), messagePanel.Hide)
                }
                });

                messagePanel.Show();

                StaticData.current.TutorialShowed();

            }

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
