using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.StartScreen {
    public class StartScreenLogic : MonoBehaviour {

        [SerializeField]
        private Text roomsRecord;

        public void MoveToGame() {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        private void Start() {

            var countRooms = PlayerPrefs.GetInt("countRooms");

            if (countRooms == default) {
                roomsRecord.text = "";
            } else {

                roomsRecord.text = TranslateReader.GetTranslate("record_rooms", 
                    new Placeholder("count_rooms", StaticData.current.GetLevelRecord())
                );

            }
            
        }

    }

}
