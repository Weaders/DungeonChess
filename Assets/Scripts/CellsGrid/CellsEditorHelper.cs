using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {
    public class CellsEditorHelper : MonoBehaviour {

        [MenuItem("Custom/Cells/SetEnemy")]
        static void SetEnemyCells() {
            foreach (var obj in Selection.gameObjects) {
                obj.GetComponentInChildren<Cell>().SetCellType(Cell.CellType.ForEnemy);
            }
        }

    }
}
