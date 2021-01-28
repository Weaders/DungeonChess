using UnityEngine;

namespace Assets.Scripts.AnimationCtrl {

    [CreateAssetMenu(menuName = "AnimsData/Attack")]
    public class AnimAttackData : ScriptableObject {

        public bool isTiggerEndAttack;
        public float scale;

        public static AnimAttackData GetNew() {

            var result = CreateInstance<AnimAttackData>();
            result.scale = 1f;
            result.isTiggerEndAttack = true;

            return result;
        }

        private static AnimAttackData _default;

    }
}
