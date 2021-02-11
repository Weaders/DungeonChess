using Assets.Scripts.Common;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.StartScreen {
    public class StartScreenLogic : MonoBehaviour {

        [SerializeField]
        private Text roomsRecord;

        [SerializeField]
        private Animator blackOverlayAnim;

        public void MoveToGame() {

            blackOverlayAnim.SetBool(AnimationValStore.IS_SHOW, true);
            var operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            operation.completed += (opr) => {

                if (opr.isDone)
                    SceneManager.UnloadSceneAsync(0);

            };

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
