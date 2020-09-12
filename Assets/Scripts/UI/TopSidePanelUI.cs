using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TopSidePanel {

    public class TopSidePanelUI : MonoBehaviour {

        public Text moneyCount;

        public Button startBtn;

        public void Init() {

            moneyCount.Subscribe(GameMng.current.playerData.money);

        }

    }

}
