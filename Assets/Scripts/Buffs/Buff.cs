using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs {

    public enum DuplicateBuffStrategy { 
        None,
        Replace
    }

    public enum BuffType { 
        Buff,
        Debuff
    }

    public interface IBuffSource { }

    public abstract class Buff : MonoBehaviour {

        [HideInInspector]
        public CharacterCtrl characterCtrl;

        [HideInInspector]
        public CharacterCtrl fromCharacterCtrl;

        [SerializeField]
        private string id;

        [SerializeField]
        private string _titleKey;

        [SerializeField]
        private string _descriptionKey;

        [SerializeField]
        private BuffType buffType;

        public string title => TranslateReader.GetTranslate(_titleKey, GetPlaceholders(fromCharacterCtrl.characterData));

        public string description => TranslateReader.GetTranslate(_descriptionKey, GetPlaceholders(fromCharacterCtrl.characterData));

        [SerializeField]
        private DuplicateBuffStrategy duplicateBuffStrategy;

        public DuplicateBuffStrategy GetDuplicateStrg() => duplicateBuffStrategy;

        public BuffType GetBuffType() => buffType;

        public string GetId() => id;

        public void ApplyTo(CharacterCtrl newCharacter, CharacterCtrl fromCharacter = null) {
            
            characterCtrl = newCharacter;
            fromCharacterCtrl = fromCharacter != null ? fromCharacter : newCharacter;
            Apply();

        }

        /// <summary>
        /// De apply buff, but there you 
        /// </summary>
        public void Remove() {
            DeApply();
            characterCtrl = null;
        }

        protected void RemoveFromCurrent() {
            characterCtrl.characterData.buffsContainer.Remove(this);
        }


        protected abstract void Apply();

        protected abstract void DeApply();

        protected virtual Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var place = this.GetPlaceholdersFromAttrs(descriptionFor);
            return place.ToArray();

        }

    }
}
