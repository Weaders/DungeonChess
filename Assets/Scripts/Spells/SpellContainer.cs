using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells {
    public class SpellContainer : MonoBehaviour, IObservableList<Spell> {

        protected ObservableList<Spell> spells = new ObservableList<Spell>();

        public Spell this[int index] => spells[index];

        public OrderedEvents<ChangeEnumerableItemEvent<Spell>> onAdd => spells.onAdd;

        public OrderedEvents<ChangeEnumerableItemEvent<Spell>> onRemove => spells.onRemove;

        public CharacterData owner { get; set; }

        public int Count => spells.Count;

        public void Add(Spell data) {

            spells.Add(data);
            data.transform.SetParent(transform);

        }

        public Spell GetBaseAttackSpell()
            => spells.FirstOrDefault(sp => sp.spellType == SpellType.BaseAttack);

        public Spell GetFullManaSpellAttack()
            => spells.First(sp => sp.spellType == SpellType.FullManaAttack);

        public IEnumerator<Spell> GetEnumerator() => spells.GetEnumerator();

        public void Remove(Spell data) {

            spells.Remove(data);
            data.transform.SetParent(null);

        }

        IEnumerator IEnumerable.GetEnumerator() => spells.GetEnumerator();

        public void Init(CharacterData owner) {

            this.owner = owner;

            TagLogger<SpellContainer>.Info("Start add default spells");

            foreach (Transform spell in transform) {

                TagLogger<SpellContainer>.Info("Add spell");
                Add(spell.gameObject.GetComponent<Spell>());

            }

        }

    }

}
