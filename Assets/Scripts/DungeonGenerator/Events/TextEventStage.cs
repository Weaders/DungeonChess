using System;
using System.Linq;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.UI.MessagePopup.MessagePanel;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.BaseMessageData;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts.DungeonGenerator.Events {

    [CreateAssetMenu(menuName = "Events/Text")]
    public class TextEventStage : EventStage {

        public string textKey;

        public Btn[] btns;

        public override void Exec() {

            base.Exec();

            var btnsForPanel = btns.Select(btn => new BtnData(TranslateReader.GetTranslate(btn.textKey), () => {

                if (btn.manaModifier != 0)
                    GameMng.current.playerData.money.val += btn.manaModifier;

                if (btn.eventStage != null)
                    btn.eventStage.Exec();
                else
                    GameMng.current.messagePanel.Hide();

            })).ToArray();

            if (btnsForPanel.Length == 0)
                btnsForPanel = new[] { 
                    new BtnData(TranslateReader.GetTranslate("end"), GameMng.current.messagePanel.Hide)
                };

            GameMng.current.messagePanel.SetData(
                new MessageData {
                    msg = TranslateReader.GetTranslate(textKey),
                    btns = btnsForPanel
                }
            );

            GameMng.current.messagePanel.Show();

        }

        [Serializable]
        public class Btn {

            public string textKey;
            public EventStage eventStage;
            public int manaModifier;

        }
    }
}
