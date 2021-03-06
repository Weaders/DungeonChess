﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Buffs {

    public class BuffsContainer : MonoBehaviour, IObservableList<Buff> {

        public bool isInProcessOfInit { get; private set; } = false;

        public ObservableList<Buff> buffs = new ObservableList<Buff>();

        private CharacterCtrl characterCtrl;

        public Buff this[int index] => buffs[index];

        public OrderedEvents<ChangeEnumerableItemEvent<Buff>> onAdd => buffs.onAdd;

        public OrderedEvents<ChangeEnumerableItemEvent<Buff>> onRemove => buffs.onRemove;

        public int Count => buffs.Count;

        public Buff AddPrefab(Buff dataPrefab, CharacterCtrl from = null) {

            var buffObj = Instantiate(dataPrefab.gameObject, transform);
            var buff = buffObj.GetComponent<Buff>();
            
            Add(buff, from);

            return buff;

        }

        public void Add(Buff data, CharacterCtrl from = null, bool forceIgnoreDuplicate = false) {

            var duplicate = buffs.FirstOrDefault(b => b.GetId() == data.GetId());

            if (duplicate != null) {

                if (forceIgnoreDuplicate)
                    return;

                if (data.GetDuplicateStrg() == DuplicateBuffStrategy.Replace) {
                    Remove(duplicate);
                }

            }

            data.ApplyTo(characterCtrl, from);

            buffs.Add(data);
            data.transform.SetParent(transform);

        }

        public IEnumerator<Buff> GetEnumerator() =>
            buffs.GetEnumerator();

        public void Remove(Buff data) {

            var exists = buffs.FirstOrDefault(d => d == data);

            if (exists != null) {

                exists.Remove();
                buffs.Remove(exists);
                Destroy(data.gameObject);

            }

        }

        public void Remove(string buffId) {

            var exists = buffs.FirstOrDefault(d => d.GetId() == buffId);
            Remove(exists);

        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Init(CharacterCtrl charCtrl) {

            isInProcessOfInit = true;

            characterCtrl = charCtrl;

            TagLogger<BuffsContainer>.Info("Start add default spells");

            foreach (Transform spell in transform) {

                TagLogger<BuffsContainer>.Info("Add spell");
                Add(spell.gameObject.GetComponent<Buff>());

            }

            isInProcessOfInit = false;

        }

        public void Add(Buff data) => Add(data, null);
    }
}
