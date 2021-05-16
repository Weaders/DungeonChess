using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Events {


    [CreateAssetMenu(menuName = "Events/Fight")]
    public class FightEventStage : EventStage {

        public CharacterDungeonData[] characterDungeonData;

        public EnemyTeamStrtg enemyTeamStrtg;

        public EventStage win;

        public EventStage lose;

        public override void Exec() {

            base.Exec();

            var enemies = new CharacterCtrl[characterDungeonData.Length];

            for (var i = 0; i < enemies.Length; i++) {

                enemies[i] = PrefabFactory.InitCharacterCtrl(characterDungeonData[i].characterCtrl, true);
                GameMng.current.fightMng.fightTeamEnemy.AddCharacterToTeam(enemies[i]);

            }

            EnemyTeamStrtgExtension.GetStrgObj(enemyTeamStrtg).Place(
                enemies,
                GameMng.current.cellsGridMng.enemiesSideCell.Where(c => c.IsAvailableToStay()),
                GameMng.current.cellsGridMng.playerSideCell
            );

        }
    }
}
