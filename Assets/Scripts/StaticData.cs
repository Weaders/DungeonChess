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

        private void Awake() {

            if (FindObjectsOfType<StaticData>().Length > 1)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }

        public int GetLevelRecord()
            => PlayerPrefs.GetInt("countRooms");

        public bool TrySetLevevRecord(int countRooms) {

            var count = PlayerPrefs.GetInt("countRooms");

            if (countRooms > count) {

                PlayerPrefs.SetInt("countRooms", countRooms);
                return true;

            }
                
            return false;

        }

        public bool IsNeedShowTutorial()
            => PlayerPrefs.GetInt("tutorial", default) == 0;

        public void TutorialShowed()
            => PlayerPrefs.SetInt("tutorial", 1);

        public ColorStore colorStore;

        public Lang lang;

    }
}
