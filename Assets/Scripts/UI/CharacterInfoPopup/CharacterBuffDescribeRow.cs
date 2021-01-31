using Assets.Scripts.Buffs;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterInfoPopup {
    public class CharacterBuffDescribeRow : MonoBehaviour {


        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

        public void SetData(Buff buff) {

            title.text = buff.title;
            description.text = buff.description;

        }

    }
}
