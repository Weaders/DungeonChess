using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.DungeonGenerator {

    /// <summary>
    /// This class genereate rooms in 3D based on RoomData
    /// </summary>
    public abstract class DungeonObjectGenerator : MonoBehaviour {

        public abstract void Generate(DungeonDataPosition data);

        public virtual void BakePath() {

            var surfaces = GetComponents<NavMeshSurface>();

            foreach (var surface in surfaces)
                surface.BuildNavMesh();

        }

        public virtual void GenerateAndBake(DungeonDataPosition data) {
            Generate(data);
            BakePath();
        }

    }

}
