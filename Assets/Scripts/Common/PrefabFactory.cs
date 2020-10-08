using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Common {

    public static class PrefabFactory {

        public static CharacterCtrl InitCharacterCtrl(CharacterCtrl ctrlPrefab) {

            var ctrl = Object.Instantiate(ctrlPrefab);

            ctrl.Init();

            return ctrl;

        }

    }
}
