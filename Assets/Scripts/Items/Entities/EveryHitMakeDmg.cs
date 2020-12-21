﻿using Assets.Scripts.ActionsData;
using Assets.Scripts.Items.Entities.Base;

namespace Assets.Scripts.Items.Entities {

    public class EveryHitMakeDmg : EveryNHit, IDmgSource {

        public int dmg = 10;

        protected override void OnMakeAttack(Actions.AttackEventData attackEventData) {
            attackEventData.to.characterData.actions.GetDmg(attackEventData.from, new Dmg(dmg, this));
        }

    }

}