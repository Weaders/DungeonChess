using Assets.Scripts.Buffs;
using UnityEngine;

namespace Assets.Scripts.UI.BuffsList {

    public class BuffsListPanel : MonoBehaviour {

        [SerializeField]
        private BuffsListRow rowPrefab;

        public void SetBuffsContainer(BuffsContainer buffsContainer) {

            foreach (Transform tr in transform) {
                Destroy(tr.gameObject);
            }

            foreach (var buff in buffsContainer) {

                var row = Instantiate(rowPrefab.gameObject, transform);
                row.GetComponent<BuffsListRow>().SetBuff(buff);

            }


        }
    }
}
