using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator.Events;
using UnityEngine;
using static Assets.Scripts.Translate.TranslateReader;

namespace Assets.Scripts {
    public class StaticData : MonoBehaviour {

        public const string LAST_COMPLETED_DUNGEON = "last_completed_dungeon";

        private static StaticData _current;

        public static StaticData current {
            get {

                if (_current == null)
                    _current = FindObjectOfType<StaticData>();

                return _current;

            }
        }

        private List<EventStage> eventStages = new List<EventStage>();

        public ColorStore colorStore;

        public int currentDungeon;

        public Lang lang;

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

        public int GetLastCompleteDungeon() {
            return PlayerPrefs.GetInt(LAST_COMPLETED_DUNGEON, -1);
        }

        public void MarkCurrentDungeonAsComplete() {
            PlayerPrefs.SetInt(LAST_COMPLETED_DUNGEON, currentDungeon);
        }

        public void AddSavedStage(EventStage stage) {
            eventStages.Add(stage);
        }

        public bool IsCompletedStage(EventStage stage)
            => eventStages.Contains(stage);

    }
}
