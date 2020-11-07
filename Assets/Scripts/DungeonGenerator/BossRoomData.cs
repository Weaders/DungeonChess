using System.Linq;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    public class BossRoomData : EnemyRoomData {

        public BossRoomData(string titleKey) : base(titleKey) { }

        public override string GetRoomName() => "BossRoom";

        protected override EnemyTeam[] GetEnemyTeams()
            => GameMng.current.currentDungeonData.enemiesPoll.GetBossTeams().ToArray();

    }

}
