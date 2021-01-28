using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

    [RequireComponent(typeof(Text))]
    public class NameOfCharacter : MonoBehaviour {

        [SerializeField]
        private Text textObj;

        private SubsribeTextResult subscribeTextResult;

        private void Reset() {
            textObj = GetComponent<Text>();
        }

        public void SetCharacterData(CharacterData characterData) {

            if (subscribeTextResult != null) {
                subscribeTextResult.Unsubscribe();
            }

            subscribeTextResult = textObj.Subscribe(
                () => TranslateReader.GetTranslate(characterData.characterName), 
                OrderVal.UIUpdate, 
                characterData.characterName
            );

        }

    }
}
