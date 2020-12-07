using Assets.Scripts.Observable;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.TopSidePanel {

    public class TopSidePanelUI : MonoBehaviour {

        public TextMeshProUGUI moneyCount;

        public TextMeshProUGUI level;

        public void Init() {

            moneyCount.Subscribe(GameMng.current.playerData.money);
            level.Subscribe(GameMng.current.level);

        }

    }

}
