using System.Collections.Generic;
using Assets.Scripts.BuyMng;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterBuyPanel {

    [RequireComponent(typeof(CanvasGroup))]
    public class CharacterBuyPanelUI : MonoBehaviour {

        [SerializeField]
        private CharacterBuyPanelBtn btnPrefab = null;

        [SerializeField]
        private CanvasGroup canvasGroup = null;

        private List<(BuyData, CharacterBuyPanelBtn)> btns = new List<(BuyData, CharacterBuyPanelBtn)>();

        private BuyData _selectedBuyData;

        public CharacterCtrl selectedCharaCtrl {
            get;
            private set;
        }

        public BuyData selectedBuyData {
            get => _selectedBuyData;
            set {
                _selectedBuyData = value;

                foreach (var (buyData, btn) in btns) {

                    if (buyData == _selectedBuyData) {
                        btn.Select();
                    } else if (btn.IsSelected()) {
                        EventSystem.current.SetSelectedGameObject(null);
                    }

                }

            }
        }

        public void Init() {

            RefreshData();

            GameMng.current.roomCtrl.onMoveToNextRoom.AddListener(RefreshData);

            GameMng.current.fightMng.isInFight.onPostChange.AddSubscription(
                Observable.OrderVal.UIUpdate,
                RefreshData
            );

            GameMng.current.isBuildPhase.onPostChange.AddSubscription(
                Observable.OrderVal.UIUpdate,
                RefreshData
            );

            GameMng.current.playerData.money.onPostChange.AddSubscription(Observable.OrderVal.UIUpdate, () => {

                foreach (var (buyData, btn) in btns) {
                    btn.interactable = buyData.IsCanBuy();
                }

            });

        }

        private void RefreshData() {

            var roomCtrl = GameMng.current.roomCtrl;
            var isBuildPhase = GameMng.current.isBuildPhase;

            if (isBuildPhase && roomCtrl.currentRoom != null
                && (roomCtrl.currentRoom.roomData is RerollRoomData
                    || roomCtrl.currentRoom.roomData is StartRoomData)) {

                canvasGroup.Show();

            } else {

                canvasGroup.Hide();
                return;

            }

            foreach (Transform tr in transform)
                Destroy(tr.gameObject);

            btns.Clear();

            foreach (var ctrlBuy in GameMng.current.buyMng.buyDataList) {

                var btn = Instantiate(btnPrefab, transform);

                btn.SetBuyData(ctrlBuy);

                btn.interactable = ctrlBuy.IsCanBuy();

                btn.onClick.AddListener(() => {

                    if (selectedBuyData == ctrlBuy)
                        selectedBuyData = null;
                    else
                        selectedBuyData = ctrlBuy;

                });

                btns.Add((ctrlBuy, btn));

            }

        }

    }
}
