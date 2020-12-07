using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private MessageData _messageData;

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void SetData(MessageData messageData) {
            _text.text = messageData.msg;
            GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = messageData.btnOk;
            _messageData = messageData;
        }

        private void Reset() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponentInChildren<Text>();
        }

        public void ClickOK() {
            Hide();
            _messageData.onClick?.Invoke();
        }

        public class MessageData { 
            public string msg { get; set; }

            public string btnOk { get; set; }

            public UnityAction onClick { get; set; }
        }

    }
}
