using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Items.Entities.Base {

    public abstract class EveryNHit : ItemData {

        [SerializeField]
        [Placeholder("n")]
        protected int n = 2;

        private void OnPreMakeAttack(AttackEventData attackEventData) {

            if (attackEventData.attackNumberInFight > 0 && attackEventData.attackNumberInFight % n == 0) {
                OnMakeAttack(attackEventData);
            }

        }

        protected abstract void OnMakeAttack(AttackEventData attackEventData);

        protected override void Equip() {
            owner.actions.onPreMakeAttack.AddSubscription(Observable.OrderVal.Fight, OnPreMakeAttack);
        }

        protected override void OnDeEquip() {
            owner.actions.onPreMakeAttack.RemoveSubscription(OnPreMakeAttack);
        }

    }
}
