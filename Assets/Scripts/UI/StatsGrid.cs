using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.UI {

    public class StatsGrid : MonoBehaviour {

        [SerializeField]
        private StatsGridCell gridCellPrefab;

        [SerializeField]
        private CellWhatDisplay[] whatDisplay;

        private StatsGridCell[] cells;

        private void Awake() {

            cells = new StatsGridCell[whatDisplay.Length];

            for (var i = 0; i < whatDisplay.Length; i++) {
                var cell = Instantiate(gridCellPrefab, transform);
                cells[i] = cell.GetComponent<StatsGridCell>();
            }

        }

        public void SetCharacter(CharacterData characterData) {

            for (var i = 0; i < whatDisplay.Length; i++) {
                cells[i].SetCharacter(characterData, whatDisplay[i]);
            }

        }

    }
}
