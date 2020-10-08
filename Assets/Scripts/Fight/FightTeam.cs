using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.Fight.PlaceStrategy;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Fight {

    public class FightTeam {

        public UnityEvent onAllInTeamDie = new UnityEvent();

        public IReadOnlyList<CharacterCtrl> chars => _characterCtrls;

        public IEnumerable<CharacterCtrl> aliveChars => chars.Where(ctrl => !ctrl.characterData.stats.isDie);

        private readonly List<CharacterCtrl> _characterCtrls = new List<CharacterCtrl>();

        public readonly TeamSide teamSide;

        public FightTeam(TeamSide side) {
            teamSide = side;
        }

        public void AddCharacterToTeam(CharacterCtrl ctrl) {

            _characterCtrls.Add(ctrl);

            ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Fight, () => {

                if (!aliveChars.Any())
                    onAllInTeamDie.Invoke();

            });

            ctrl.teamSide = teamSide;

        }

        public void ClearChars() {

            foreach (var charCtrl in _characterCtrls) {
                Object.Destroy(charCtrl.gameObject);
            }

            _characterCtrls.Clear();

        }

    }

}
