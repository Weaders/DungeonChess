using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Buffs;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    [CreateAssetMenu(menuName = "Dungeon/Buffs")]
    public class BuffsPoll : ScriptableObject {

        public Buff[] buffs;

    }
}
