using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessagePopup {

    public class MessagePanel : MonoBehaviour {

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private Button buttonPrefab;

        [SerializeField]
        private Transform containerBtns;

        private MessageData _messageData;

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void SetData(MessageData messageData) {

            _text.text = messageData.msg;

            foreach (Transform tr in containerBtns)
                Destroy(tr.gameObject);

            foreach (var btn in messageData.btns) {

                var obj = Instantiate(buttonPrefab.gameObject, containerBtns);
                var btnObj = obj.GetComponent<Button>();

                btnObj.onClick.AddListener(btn.onClick);
                btnObj.GetComponentInChildren<Text>().text = btn.title;

            }

            _messageData = messageData;
        }

        private void Reset() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponentInChildren<Text>();
        }

        public class MessageData {

            public string msg { get; set; }

            public BtnData[] btns { get; set; }

            public class BtnData {
                public string title { get; set; }
                public UnityAction onClick { get; set; }
            }

        }

    }
}
