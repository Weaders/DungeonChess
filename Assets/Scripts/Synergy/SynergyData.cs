using System.Collections.Generic;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    public abstract class SynergyData : ScriptableObject {

        public abstract RecalcResult[] Recalc(IEnumerable<CharacterCtrl> ctrls);

        public abstract Color GetLineColor();

    }

    public class RecalcResult {

        public SynergyData synergyData;
        public Buff buffResult;
        public IEnumerable<CharacterCtrl> ctrls;

    }

}
