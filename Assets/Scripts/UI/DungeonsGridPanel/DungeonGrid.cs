using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.UI.DungeonsGridPanel.DungeonBtn;

namespace Assets.Scripts.UI.DungeonsGridPanel {

    [RequireComponent(typeof(GridLayoutGroup))]
    public class DungeonGrid : MonoBehaviour {

        [SerializeField]
        private GridLayoutGroup gridLayout;

        [SerializeField]
        private DungeonBtn btnPrefab;

        private void Reset() {
            gridLayout = GetComponent<GridLayoutGroup>();
        }

        private void Start() {
            DisplayDungeons();
        }

        public void DisplayDungeons() {

            foreach (Transform tr in transform) {
                Destroy(tr.gameObject);
            }

            for (var i = 0; i < CityMng.current.cityData.dungeonYears.Length; i++) {

                var btn = Instantiate(btnPrefab, transform);

                if (i < CityMng.current.cityData.GetCurrentDungeonIndex()) {
                    btn.SetDungeonData(CityMng.current.cityData.dungeonYears[i], BtnState.Visited);
                } else if (i == CityMng.current.cityData.GetCurrentDungeonIndex()) {
                    btn.SetDungeonData(CityMng.current.cityData.dungeonYears[i], BtnState.Available);
                } else {
                    btn.SetDungeonData(CityMng.current.cityData.dungeonYears[i], BtnState.Locked);
                }

            }

        }
    }
}
