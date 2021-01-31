using Assets.Scripts.Observable;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.TopSidePanel {

    public class TopSidePanelUI : MonoBehaviour {

        public TextMeshProUGUI moneyCount;

        public TextMeshProUGUI level;

        public TextMeshProUGUI charactersCount;

        public void Init() {

            moneyCount.Subscribe(GameMng.current.playerData.money);
            level.Subscribe(GameMng.current.level);

            charactersCount.Subscribe(
                () => $"{GameMng.current.playerData.charactersCount}/{GameMng.current.playerData.maxCharacterCount}", 
                OrderVal.UIUpdate, 
                GameMng.current.playerData.charactersCount, 
                GameMng.current.playerData.maxCharacterCount
            );

        }

    }

}
