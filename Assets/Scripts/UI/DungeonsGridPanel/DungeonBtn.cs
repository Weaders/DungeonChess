using Assets.Scripts.City;
using Assets.Scripts.EnemyData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.DungeonsGridPanel {

    public class DungeonBtn : MonoBehaviour {

        [HideInInspector]
        public DungeonData dungeonData;

        [SerializeField]
        private Text yearText;

        [SerializeField]
        private Button btn;

        [SerializeField]
        private Sprite defaultSprite;

        [SerializeField]
        private Sprite lockedSprite;

        public void SetDungeonData(DungeonYear dungeonYear, BtnState state) {

            dungeonData = dungeonYear.dungeonData;
            yearText.text = dungeonYear.year.ToString();

            if (state == BtnState.Locked) {
                
                btn.interactable = true;
                btn.image.sprite = lockedSprite;

            } else if (state == BtnState.Visited) {

                btn.interactable = false;
                btn.image.sprite = defaultSprite;

            }

        }

        public void GoToDungeon() {

            var operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            operation.completed += (opr) => {

                if (opr.isDone)
                    SceneManager.UnloadSceneAsync(2);

            };

        }

        public enum BtnState { 
            Available, Locked, Visited

        }

    }

}
