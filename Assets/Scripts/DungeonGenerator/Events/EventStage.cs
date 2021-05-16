using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Events {
    public abstract class EventStage : ScriptableObject {

        public int violianceOffset;

        public int demonScienceOffset;

        public int necroApproveOffset;

        public virtual void Exec() {

            GameMng.current.cityDataChange.demonScienceOffset += demonScienceOffset;
            GameMng.current.cityDataChange.violenceOffset += violianceOffset;
            GameMng.current.cityDataChange.necroApproveOffset += necroApproveOffset;

            StaticData.current.AddSavedStage(this);
        }

    }
}
