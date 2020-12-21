using Assets.Scripts.ActionsData;
using Assets.Scripts.Items.Entities.Base;
using Assets.Scripts.Spells.Modifiers;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Items.Entities {

    public class EveryNHitReturnDmg : EveryNHit, IDmgSource {

        protected override void OnMakeAttack(AttackEventData attackEventData) {

            attackEventData.dmg.dmgModifiers.Add(new DmgScale(0f, int.MaxValue));
            var val = attackEventData.dmg.GetCalculateVal();
            attackEventData.from.characterData.actions.GetDmg(attackEventData.to, new Dmg(val, this));

        }

    }

}
