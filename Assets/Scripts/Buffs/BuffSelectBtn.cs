using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;

namespace Assets.Scripts.Buffs {
    public class BuffSelectBtn : IForSelectPanel {

        private readonly Buff _buffToShow;

        public Sprite img => null;

        public string title => _buffToShow.title;

        public string description => _buffToShow.description;

        public string selectText => TranslateReader.GetTranslate("select");

        public ObservableVal _onChange = new ObservableVal();

        public ObservableVal onChange => _onChange;

        public bool isEnableToSelect => true;

        public BuffSelectBtn(Buff buff) {
            _buffToShow = buff;
        }

        public void Select() {
            GameMng.current.fightMng.fightTeamPlayer.AddBuffForTeam(_buffToShow);
        }
    }
}
