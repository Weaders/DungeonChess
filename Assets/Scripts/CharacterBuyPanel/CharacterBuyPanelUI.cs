using System.Collections.Generic;
using Assets.Scripts.BuyMng;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CharacterBuyPanel {

    [RequireComponent(typeof(CanvasGroup))]
    public class CharacterBuyPanelUI : MonoBehaviour {

        [SerializeField]
        private Button btnPrefab = null;

        [SerializeField]
        private CanvasGroup canvasGroup = null;

        private List<(BuyData, Button)> btns = new List<(BuyData, Button)>();

        public CharacterCtrl selectedCharaCtrl {
            get;
            private set;
        }

        public BuyData selectedBuyData {
            get;
            set;
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
                btn.GetComponentInChildren<Text>().text = ctrlBuy.name;

                var btnObj = btn.GetComponent<Button>();

                btnObj.interactable = ctrlBuy.IsCanBuy();

                btnObj.GetComponent<Button>().onClick.AddListener(() => {
                    selectedBuyData = ctrlBuy;
                });

                btns.Add((ctrlBuy, btnObj));

            }

        }

    }
}
