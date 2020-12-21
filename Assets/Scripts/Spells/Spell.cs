﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells {

    public enum SpellType {
        BaseAttack,
        FullManaAttack
    }

    public enum SpellTarget {
        Self, Enemy, RandomEnemy
    }

    public abstract class Spell : MonoBehaviour {

        [SerializeField]
        private string id;

        public string titleKey;

        public string descriptionKey;

        public string Id => id;

        public string GetId() => Id;

        public string description
            => TranslateReader.GetTranslate(descriptionKey);

        public SpellType spellType;

        public SpellTarget spellTarget;

        public string GetDescription(CharacterData owner)
            => TranslateReader.GetTranslate(descriptionKey, GetPlaceholders(owner));

        public string GetTitle(CharacterData owner)
            => TranslateReader.GetTranslate(titleKey, GetPlaceholders(owner));


        public abstract UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseOpts opts);

        public float range = 2f;

        public virtual bool IsInRange(CharacterCtrl from, CharacterCtrl to)
            => true;
            //=> Vector3.Distance(from.transform.position, to.transform.position) <= range;

        private Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var result = descriptionFor.stats.GetFields()
                .Select(f => new Placeholder(f.Name, f.GetValue(descriptionFor.stats).ToString()));

            result = result.Union(GetType().GetMethods().Select(p => {
                var key = p.GetCustomAttribute<PlacecholderAttribute>()?.Key;
                return key != null ? new Placeholder(key, p.Invoke(this, new[] { descriptionFor }).ToString()) : null;
            }).Where(kv => kv != null));

            return result.ToArray();

        }

        public abstract IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data);

        public class UseOpts {
            public float scale = 1f;
            public Animator animator = null;
        }

        public class UseSpellResult {
            
            public Func<bool> IsEndUseSpell;


        }

    }

}
