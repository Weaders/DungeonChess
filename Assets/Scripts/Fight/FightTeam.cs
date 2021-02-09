using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Fight {

    public class FightTeam {

        public UnityEvent onAllInTeamDie = new UnityEvent();

        public UnityEvent onChangeTeamCtrl = new UnityEvent();

        public IReadOnlyList<CharacterCtrl> characters => _characterCtrls;

        public IEnumerable<CharacterCtrl> aliveChars => characters.Where(ctrl => !ctrl.characterData.stats.isDie);

        private readonly List<CharacterCtrl> _characterCtrls = new List<CharacterCtrl>();

        public readonly TeamSide teamSide;

        public FightTeam(TeamSide side) {
            teamSide = side;
        }

        public void AddCharacterToTeam(CharacterCtrl ctrl) {

            _characterCtrls.Add(ctrl);

            GameMng.current.levelUpService.LevelUpToCurrent(ctrl, teamSide != TeamSide.Player);                

            ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Fight, () => {

                if (!aliveChars.Any())
                    onAllInTeamDie.Invoke();

            });

            ctrl.onDestoy.AddSubscription(Observable.OrderVal.Fight, () => {

                _characterCtrls.Remove(ctrl);
                onChangeTeamCtrl.Invoke();

            });

            ctrl.teamSide = teamSide;
            onChangeTeamCtrl.Invoke();

        }

        public CharacterCtrl AddCharacterToTeamPrefab(CharacterCtrl ctrlPrefab) {

            var ctrl = PrefabFactory.InitCharacterCtrl(ctrlPrefab);

            AddCharacterToTeam(ctrl);

            return ctrl;

        }

        public void RemoveCharacter(CharacterCtrl ctrl) {

            foreach (var charCtrl in _characterCtrls) {
                if (ctrl == charCtrl) {
                    Object.Destroy(ctrl.gameObject);
                }
            }

            _characterCtrls.Remove(ctrl);

        }

        public void ClearChars() {

            foreach (var charCtrl in _characterCtrls) {
                Object.Destroy(charCtrl.gameObject);
            }

        }

    }

}
