using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TopSidePanel {

    public class TopSidePanelUI : MonoBehaviour {

        public Text moneyCount;

        public Button startBtn;

        public Text level;

        public void Init() {

            moneyCount.Subscribe(GameMng.current.playerData.money);
            
            level.Subscribe(
                () => $"{GameMng.current.level}/{GameMng.current.countLevels}",
                OrderVal.UIUpdate, 
                GameMng.current.level, 
                GameMng.current.countLevels
            );

        }

    }

}
