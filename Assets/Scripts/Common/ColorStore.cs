using System;
using System.Linq;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Common {
    
    [CreateAssetMenu(menuName = "Common/ColorStore")]
    public class ColorStore : ScriptableObject {

        public Color getDmgText = new Color(1, 0, 0, 1);
        public Color getHealText = new Color(0, 1, 0, 1);

        #region Cell
        public Color cellPlayerAllow = new Color(0.16f, 0.4f, 0.08f, 0.08f);
        public Color cellPlayerAllowOutline = new Color(0, 1f, 0, 1f);

        public Color cellPlayerNotAllow = new Color(0.5f, 0, 0, 0.6f);
        public Color cellPlayerNotAllowOutline = new Color(0, 1f, 0, 1f);

        public Color cellEnemy = new Color(0.5f, 0, 0, 0.6f);
        public Color cellEnemyOutlineCell = new Color(1, 0, 0, 1f);
        #endregion

        public Color playerTeamDetectColor = new Color(0, 1, 0, 1f);
        public Color enemyTeamDetectColor = new Color(1, 0, 0, 1f);

        #region Classes
        public ClassColorLine[] classesTypes;

        public Color GetLineColor(CharacterClassType characterClassType) 
            => classesTypes.FirstOrDefault(c => c.characterClassType == characterClassType)?.color ?? default;
        #endregion

        [Serializable]
        public class ClassColorLine {
            public CharacterClassType characterClassType;

            public Color color;
        }

    }

    
}
