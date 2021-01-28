using System.Collections.Generic;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {

    public class CellActionsContainer : MonoBehaviour {

        public delegate void ApplyAction(CharacterCtrl characterCtrl);
        public delegate void DeApplyAction(CharacterCtrl characterCtrl);

        public class ActionData {

            public ApplyAction applyAction;
            public DeApplyAction removeAction;
            public IBuffSource buffSource;

        }

        private List<ActionData> actions = new List<ActionData>();

        [SerializeField]
        private Cell _cell;

        private Dictionary<IBuffSource, Buff> buffs = new Dictionary<IBuffSource, Buff>();

        private void Awake() {

            _cell.onStayCtrl.AddListener((oldCtrl, ctrl) => {

                if (oldCtrl != null) {

                    foreach (var act in actions) {
                        act.removeAction?.Invoke(oldCtrl);
                    }

                }

                if (ctrl != null) {

                    foreach (var act in actions) {
                        act.applyAction?.Invoke(ctrl);
                    }

                }
                    

            });

        }

        public void AddBuffAction(Buff buffPrefab, IBuffSource source) {

            AddAction(new ActionData 
            {
                applyAction = (ctrl) => {
                    buffs.Add(source, ctrl.characterData.buffsContainer.AddPrefab(buffPrefab));
                },
                removeAction = (ctrl) => {
                    ctrl.characterData.buffsContainer.Remove(buffs[source]);
                    buffs.Remove(source);
                },
                buffSource = source
            });

        }

        public void RemoveAction(IBuffSource buffSource) {

            actions.RemoveAll(a => a.buffSource == buffSource);

            if (buffs.TryGetValue(buffSource, out var buff)) {
                
                _cell.characterCtrl.characterData.buffsContainer.Remove(buff);
                buffs.Remove(buffSource);

            }

        }

        public void AddAction(ActionData actionData) {

            actions.Add(actionData);

            if (_cell.characterCtrl != null)
                actionData.applyAction.Invoke(_cell.characterCtrl);

        }

        public void RemoveAction(ActionData actionData) {

            actions.Remove(actionData);

            if (_cell.characterCtrl != null)
                actionData.removeAction.Invoke(_cell.characterCtrl);

        }

    }
}
