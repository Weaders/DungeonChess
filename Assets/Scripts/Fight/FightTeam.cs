using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Fight {

    public class FightTeam {

        public UnityEvent onAllInTeamDie = new UnityEvent();

        /// <summary>
        /// Call when added or removed characte ctrl from team
        /// </summary>
        public UnityEvent onChangeTeamCtrl = new UnityEvent();

        /// <summary>
        /// Call when some character in team is die
        /// </summary>
        public UnityEvent onCharacterDie = new UnityEvent();

        public IReadOnlyList<CharacterCtrl> characters => _characterCtrls;

        public IEnumerable<CharacterCtrl> aliveChars => characters.Where(ctrl => ctrl != null && !ctrl.characterData.stats.isDie);

        private readonly List<CharacterCtrl> _characterCtrls = new List<CharacterCtrl>();

        public readonly TeamSide teamSide;

        public bool isSimulateTeam { get; private set; }


        public ObservableVal<bool> isInFight = new ObservableVal<bool>();

        private List<Buff> _buffs = new List<Buff>();

        public void AddBuffForTeam(Buff buff) {

            _buffs.Add(buff);

            foreach (var character in characters)
                character.characterData.buffsContainer.AddPrefab(buff);

        }

        public FightTeam(TeamSide side, bool isSimulate = false) {

            teamSide = side;
            isSimulateTeam = isSimulate;

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

            foreach(var buff in _buffs)
                ctrl.characterData.buffsContainer.AddPrefab(buff);

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

                if (charCtrl.characterData.cell != null && isSimulateTeam && charCtrl.characterData.cell.characterCtrl == charCtrl)
                    charCtrl.characterData.cell.StayCtrl(null, changeState: false);

                Object.Destroy(charCtrl.gameObject);

            }

            _characterCtrls.Clear();

        }

    }

}
