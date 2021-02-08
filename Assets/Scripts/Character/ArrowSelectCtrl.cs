using UnityEngine;

namespace Assets.Scripts.Character {

    public class ArrowSelectCtrl : MonoBehaviour {

        [SerializeField]
        private GameObject prefab;

        private GameObject obj;

        private CharacterCtrl selectedCharacterCtrl;

        public float offsetY = 1;

        public void Init() {

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.AddListener((oldCtrl, newCtrl) => {

                if (newCtrl == null) {

                    if (obj != null)
                        obj.SetActive(false);

                } else {

                    if (obj == null)
                        obj = Instantiate(prefab);

                    if (!obj.activeInHierarchy)
                        obj.SetActive(true);


                }

                selectedCharacterCtrl = newCtrl;

            });

            GameMng.current.roomCtrl.onMoveToNextRoom.AddListener(() => {

                if (selectedCharacterCtrl != null && selectedCharacterCtrl.characterData && selectedCharacterCtrl.characterData.stats.isDie) {

                    if (obj != null) {

                        selectedCharacterCtrl = null;
                        obj.SetActive(false);

                    }
                    

                }

            });

        }

        private void Update() {

            if (selectedCharacterCtrl != null)
                obj.transform.position = selectedCharacterCtrl.transform.position + new Vector3(0, offsetY, 0);

        }

    }
}
