using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessagePopup {

    public class MessagePanel : MonoBehaviour {

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Button buttonPrefab;

        [SerializeField]
        private Transform containerBtns;

        [SerializeField]
        private RectTransform containerText;

        [SerializeField]
        private TextMessageRowData textMessageRowData;

        [SerializeField]
        private RectTransform scrollView;

        [SerializeField]
        private ImgBlockMessageRowData imgBlockMessageRowData;

        private BaseMessageData[] baseMessageDatas;

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        private float maxHeight = 0;

        public void SetData(params BaseMessageData[] messages) {

            baseMessageDatas = messages;
            Display();

        }

        private void Awake() {
            maxHeight = scrollView.sizeDelta.y;
        }

        private void Display() {

            foreach (Transform tr in containerText)
                Destroy(tr.gameObject);

            foreach (Transform tr in containerBtns)
                Destroy(tr.gameObject);

            foreach (var msgData in baseMessageDatas) {
                msgData.AddToTransform(containerText, this);
            }

            var btns = baseMessageDatas.Where(m => m.btns != null).SelectMany(m => m.btns);

            foreach (var btn in btns) {

                var obj = Instantiate(buttonPrefab.gameObject, containerBtns);
                var btnObj = obj.GetComponent<Button>();

                btnObj.onClick.AddListener(btn.onClick);
                btnObj.GetComponentInChildren<Text>().text = btn.title;

            }

            var currentHeight = containerText.sizeDelta.y;

            scrollView.sizeDelta = new Vector2(
                scrollView.sizeDelta.x, 
                currentHeight + 5f
            );

        }

        public bool IsShowed
            => _canvasGroup.IsShowed();

        private void Reset() {

            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponentInChildren<TextMeshProUGUI>();

        }

        public abstract class BaseMessageData {

            public BtnData[] btns { get; set; }

            public abstract void AddToTransform(Transform transform, MessagePanel messagePanel);

            public class BtnData {

                public BtnData() { }

                public BtnData(string _title, UnityAction _onClick) {

                    title = _title;
                    onClick = _onClick;

                }

                public string title { get; set; }
                public UnityAction onClick { get; set; }
            }

        }

        public class MessageData : BaseMessageData {

            public string msg { get; set; }

            public override void AddToTransform(Transform transform, MessagePanel messagePanel) {

                var obj  = Instantiate(messagePanel.textMessageRowData, transform);
                obj.textObj.text = msg;

                Canvas.ForceUpdateCanvases();

                var delta = obj.textObj.rectTransform.sizeDelta.y - obj.textObj.preferredHeight;

                obj.textObj.rectTransform.sizeDelta = new Vector2(
                    obj.textObj.rectTransform.sizeDelta.x,
                    obj.textObj.preferredHeight
                );

                var rectTransform = transform as RectTransform;

                rectTransform.sizeDelta = new Vector2(
                    rectTransform.sizeDelta.x,
                    rectTransform.sizeDelta.y - delta
                );

            }
        }

        public class ImgBlockMessageData : BaseMessageData {

            public Sprite img;
            public string title;
            public string description;

            public override void AddToTransform(Transform transform, MessagePanel messagePanel) {

                var obj = Instantiate(messagePanel.imgBlockMessageRowData, transform);
                obj.title.text = title;
                obj.description.text = description;
                obj.img.sprite = img;

            }


        }

    }
}
