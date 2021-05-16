using Assets.Scripts.City;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI.MessagePopup;
using Assets.Scripts.UI.NewsPopup;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Scripts.UI.MessagePopup.MessagePanel;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.BaseMessageData;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts {

    public class CityMng : MonoBehaviour {

        public CityData cityData;

        [SerializeField]
        private NewsPanel newsPanel;

        public static CityMng current;

        [SerializeField]
        private Text necroApprove;

        [SerializeField]
        private Text violence;

        [SerializeField]
        private Text demonScience;

        public MessagePanel messagePanel;

        public SelectPanel selectPanel;

        private void Awake() {
            current = this;
        }

        private void Start() {

            newsPanel.RefreshNewsList();

            demonScience.Subscribe(cityData.demonScience);
            violence.Subscribe(cityData.violence);
            necroApprove.Subscribe(cityData.necroApprove);

            messagePanel.Hide();

            cityData.violence.onPostChange.AddSubscription(OrderVal.Internal, ViolenceTestEnd);

            if (cityData.GetCurrentDungeonIndex() == 0) {

                messagePanel.SetData(new MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("start_1"),
                    btns = new[] {
                        new BtnData(TranslateReader.GetTranslate("ok"), () => {
                            messagePanel.Hide();
                        })
                    }
                });

                messagePanel.Show();

            }

            ViolenceTestEnd();

        }

        public void ViolenceTestEnd() {

            if (cityData.violence.val >= 100) {

                messagePanel.SetData(new MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("city_died_buy_violence"),
                    btns = new[] {
                        new BtnData(TranslateReader.GetTranslate("ok"), () => {
                            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
                        })
                    }
                });

                messagePanel.Show();

            }

        }

    }
}
