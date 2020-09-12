﻿using UnityEngine;

namespace Assets.Scripts.Common {
    
    [CreateAssetMenu(menuName = "Common/ColorStore")]
    public class ColorStore : ScriptableObject {

        public Color getDmgText = new Color(1, 0, 0, 1);
        public Color getHealText = new Color(0, 1, 0, 1);

        #region Cell
        public Color cellPlayerAllow = new Color(0, 0.5f, 0, 0.6f);
        public Color cellPlayerAllowOutline = new Color(0, 1f, 0, 1f);

        public Color cellPlayerNotAllow = new Color(0, 0.5f, 0, 0.6f);
        public Color cellPlayerNotAllowOutline = new Color(0, 1f, 0, 1f);

        public Color cellEnemy = new Color(0.5f, 0, 0, 0.6f);
        public Color cellEnemyOutlineCell = new Color(1, 0, 0, 1f);
        #endregion

    }
}