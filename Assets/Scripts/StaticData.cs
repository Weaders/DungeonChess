using Assets.Scripts.Common;
using UnityEngine;
using static Assets.Scripts.Translate.TranslateReader;

namespace Assets.Scripts {
    public class StaticData : MonoBehaviour {

        private static StaticData _current;

        public static StaticData current { 
            get {

                if (_current == null)
                    _current = FindObjectOfType<StaticData>();

                return _current;

            } 
        }

        public ColorStore colorStore;

        public Lang lang;

    }
}
