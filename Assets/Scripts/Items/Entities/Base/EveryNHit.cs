using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Items.Entities.Base {

    public abstract class EveryNHit : ItemData {

        [SerializeField]
        [Placecholder("n")]
        protected int n = 2;

        protected abstract bool onGetAttack { get; }

        private void OnPreMakeAttack(AttackEventData attackEventData) {

            if (attackEventData.attackNumberInFight > 0 && attackEventData.attackNumberInFight % n == 0) {
                OnMakeAttack(attackEventData);
            }

        }

        protected abstract void OnMakeAttack(AttackEventData attackEventData);

        protected override void Equip() {
            if (onGetAttack)
                owner.actions.onPreMakeAttack.AddSubscription(Observable.OrderVal.Fight, OnPreMakeAttack);
            else
                owner.actions.onPreMakeAttack.AddSubscription(Observable.OrderVal.Fight, OnPreMakeAttack);
        }

        protected override void OnDeEquip() {
            owner.actions.onPreMakeAttack.RemoveSubscription(OnPreMakeAttack);
        }

    }
}
