using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;

namespace Assets.Scripts.Items.Entities {

    public class Potion : ConcurrentItem {

        protected override void Use(CharacterData data) {
            data.actions.GetHeal(null, new ActionsData.Heal(100));
        }
    }
}
