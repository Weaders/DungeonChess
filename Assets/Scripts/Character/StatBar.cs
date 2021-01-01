using Assets.Scripts.Observable;
using Assets.Scripts.StatsData;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character {

    public class StatBar : MonoBehaviour {

        public Stats stats { get; set; }

        [SerializeField]
        private Text hpText;

        public void SetData() { 
            
        }

        public void Init(CharacterData charData) {

            stats = charData.stats;

            hpText.Subscribe(
                Display,
                OrderVal.UIUpdate,
                stats.hp, stats.maxHp
            );

            hpText.text = Display();
        }

        private string Display() => $"{stats.hp.val}/{stats.maxHp.val}";

    }

}
