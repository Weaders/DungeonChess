using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessagePopup {

    public class MessagePanel : MonoBehaviour {

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Text _text;

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void SetData(MessageData messageData) {
            _text.text = messageData.msg;
            GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = messageData.btnOk;
        }

        private void Reset() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponentInChildren<Text>();
        }

        public class MessageData { 
            public string msg { get; set; }

            public string btnOk { get; set; }
        }

    }
}
