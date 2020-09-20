using Assets.Scripts.Buffs;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.BuffsList {
    public class BuffsListRow : MonoBehaviour {

        public Text title;

        public Text description;

        public void SetBuff(Buff buff) {

            title.text = buff.title;
            description.text = buff.description;

        }

    }
}
