using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.DungeonGenerator.Data;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {

    public class DungeonRoomCells : MonoBehaviour {

        public RoomData roomData { get; set; }

        public Vector2 dataPosition;

        [HideInInspector]
        [Obsolete]
        public WallGroup[] wallGroups;

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

        public IEnumerable<Cell> GetCells()
            => GetComponentsInChildren<Cell>();

        public IEnumerable<Cell> GetExits() => GetCells().Where(c => c.IsExit());

        public IEnumerable<GameObject> GetWalls() {

            foreach (Transform cellTr in transform) {

                if (cellTr.gameObject.layer == LayerMask.NameToLayer(LayersStore.WALL_LAYER))
                    yield return cellTr.gameObject;

            }

        }

        public void Hide() {

            var renderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in renderers) {
                renderer.enabled = false;
            }

        }

        public void Show() {

            var renderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var render in renderers) {
                render.enabled = true;
            }

        }

    }

}
