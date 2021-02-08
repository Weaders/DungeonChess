using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
    public class CharacterImgCtrl : MonoBehaviour {

        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite fallBackImg;

        public void Init() {

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.AddListener((data, newCtrl) => {

                if (newCtrl == null || newCtrl.imgSprite == null)
                    image.sprite = fallBackImg;
                else 
                    image.sprite = newCtrl.imgSprite;

            });

        }

    }
}
