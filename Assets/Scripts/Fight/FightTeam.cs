﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using UnityEngine.Events;

namespace Assets.Scripts.Fight {

    public class FightTeam {

        public UnityEvent onAllInTeamDie = new UnityEvent();

        public IReadOnlyList<CharacterCtrl> chars => _characterCtrls;

        public IEnumerable<CharacterCtrl> aliveChars => chars.Where(ctrl => !ctrl.characterData.stats.isDie);

        private readonly List<CharacterCtrl> _characterCtrls = new List<CharacterCtrl>();

        public void AddCharacterToTeam(CharacterCtrl ctrl) {

            _characterCtrls.Add(ctrl);

            ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Fight, () => {

                if (!aliveChars.Any())
                    onAllInTeamDie.Invoke();

            });

        }

    }

}
