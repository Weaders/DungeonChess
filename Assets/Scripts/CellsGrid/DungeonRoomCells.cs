using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {

    public class DungeonRoomCells : MonoBehaviour {

        public Vector2 dataPosition;

        public Vector3 GetCenterPosition() {

            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (var cell in GetCells()) {

                if (min.x > cell.transform.position.x || min.y > cell.transform.position.y) {
                    min = cell.transform.position;
                }

                if (max.x < cell.transform.position.x || max.y < cell.transform.position.y) {
                    max = cell.transform.position;
                }

            }

            return min + new Vector3(0, 0, 10);

        }

        public IEnumerable<Cell> GetCells() {

            foreach (Transform cellTr in transform) {

                if (cellTr.gameObject.layer == LayerMask.NameToLayer(LayersStore.CELL_LAYER))
                    yield return cellTr.GetComponent<Cell>();

            }

        }

    }

}
