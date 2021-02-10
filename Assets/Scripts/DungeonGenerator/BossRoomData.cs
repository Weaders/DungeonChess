using System.Linq;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    public class BossRoomData : EnemyRoomData {

        public BossRoomData(string titleKey) : base(titleKey) { }

        public override string GetRoomName() => "BossRoom";

        protected override void OnPlayerWin() {

            GameMng.current.RefeshLevelsToNextDifficult();

            GameMng.current.locationTitle.ShowPopup(TranslateReader.GetTranslate("difficult_up"));

            base.OnPlayerWin();

        }

        protected override EnemyTeam[] GetEnemyTeams()
            => GameMng.current.currentDungeonData.enemiesPoll.GetBossTeams()
                .Where(t => t.difficult.IsInRange(GameMng.current.levelDifficult))
                .ToArray();

    }

}
