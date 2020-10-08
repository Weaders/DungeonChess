﻿using Assets.Scripts.BuyMng;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CharacterBuyPanel {
    public class CharacterBuyPanelUI : MonoBehaviour {

        [SerializeField]
        private Button btnPrefab = null;

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
        }

        private void RefreshData() {

            foreach (Transform tr in transform)
                Destroy(tr.gameObject);

            foreach (var ctrlBuy in GameMng.current.buyMng.buyDataList) {

                var btn = Instantiate(btnPrefab, transform);
                btn.GetComponentInChildren<Text>().text = ctrlBuy.name;

                var btnObj = btn.GetComponent<Button>();

                btnObj.interactable = ctrlBuy.IsCanBuy();

                btnObj.GetComponent<Button>().onClick.AddListener(() => {
                    selectedBuyData = ctrlBuy;
                });

                GameMng.current.playerData.money.onPostChange.AddSubscription(Observable.OrderVal.UIUpdate, () => {
                    btnObj.interactable = ctrlBuy.IsCanBuy();
                });

            }

        }

    }
}
