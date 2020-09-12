using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    public abstract class SynergyData : ScriptableObject {

        public abstract RecalcResult[] Recalc(IEnumerable<CharacterCtrl> ctrls);

    }

    public class RecalcResult {

        public SynergyData synergyData;
        public Buff buffResult;
        public IEnumerable<CharacterCtrl> ctrls;

    }

}
