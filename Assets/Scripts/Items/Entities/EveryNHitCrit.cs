using Assets.Scripts.Items.Entities.Base;
using Assets.Scripts.Spells.Modifiers;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Items.Entities {

    public class EveryNHitCrit : EveryNHit {

        protected override void OnMakeAttack(AttackEventData attackEventData) {
            attackEventData.dmg.dmgModifiers.Add(new CritModify(0, owner.stats.critDmg));
        }

    }

}
