using Assets.Scripts.Character;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs {

    public enum DuplicateBuffStrategy { 
        None,
        Replace
    }

    public abstract class Buff : MonoBehaviour {

        [HideInInspector]
        public CharacterCtrl characterCtrl;

        [SerializeField]
        private string id;

        [SerializeField]
        private string _titleKey;

        [SerializeField]
        private string _descriptionKey;

        public string title => TranslateReader.GetTranslate(_titleKey);

        public string description => TranslateReader.GetTranslate(_descriptionKey);

        [SerializeField]
        private DuplicateBuffStrategy duplicateBuffStrategy;

        public DuplicateBuffStrategy GetDuplicateStrg() => duplicateBuffStrategy;

        public string GetId() => id;

        public void ApplyTo(CharacterCtrl newCharacter) {
            characterCtrl = newCharacter;
            Apply();
        }

        public void Remove() {
            DeApply();
            characterCtrl = null;
        }

        protected abstract void Apply();

        protected abstract void DeApply();


    }
}
