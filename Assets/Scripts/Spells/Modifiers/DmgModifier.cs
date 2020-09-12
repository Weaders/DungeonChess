using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.ActionsData;
using UnityEngine;

namespace Assets.Scripts.Spells.Modifiers {
    public abstract class DmgModifier {

        public int order;

        public DmgModifier(int initOrder) {
            order = initOrder;
        }

        public abstract int Modify(Dmg dmg, int val);

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
