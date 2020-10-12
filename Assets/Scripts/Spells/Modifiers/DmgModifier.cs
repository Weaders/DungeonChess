using System.Diagnostics;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Spells.Modifiers {

    public abstract class DmgModifier {

        public int order;

        public DmgModifier(int initOrder) {
            order = initOrder;
        }

        public abstract int Modify(Dmg dmg, int val);

    }

    public class CritModify : DmgModifier {

        public CritModify(int initOrder, float scaleVal) : base(initOrder) {
            scale = scaleVal;
        }

        private readonly float scale;

        public override int Modify(Dmg dmg, int val) {
            return val * 2;
        }
    }

    public class DmgScale : DmgModifier {

        public float scale;

        public DmgScale(float scaleVal, int initOrder) : base(initOrder) {
            scale = scaleVal;
        }

        public override int Modify(Dmg dmg, int val)
            => Mathf.RoundToInt(val * scale);

    }

}
