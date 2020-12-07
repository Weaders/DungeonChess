﻿using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Buffs;
using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Character {

    public class CharacterData : MonoBehaviour, IHaveItemsContainer {

        public Stats stats;

        public SpellContainer spellsContainer;

        public BuffsContainer buffsContainer;

        public ItemsContainer itemsContainer;

        public Actions actions;

        public OrderedEvents onPreMakeAttack = new OrderedEvents();

        public OrderedEvents onPostMakeAttack = new OrderedEvents();

        public CharacterCtrl characterCtrl;

        public ObservableVal<string> characterName;

        public RangeType rangeType {
            get {

                if (spellsContainer.Count == 0)
                    return default;

                return spellsContainer.GetBaseAttackSpell().range <= 6 ? RangeType.Melee : RangeType.Range;

            }
        }

        private void Awake() {
            actions = new Actions(this);
        }

        public void Init() {

            spellsContainer.Init(this);

            buffsContainer.Init(characterCtrl);

            itemsContainer.SetOwner(this);

            stats.hp.onPostChange.AddSubscription(OrderVal.Internal, (data) => {

                if (data.newVal <= 0)
                    stats.isDie.val = true;

            });

            onPostMakeAttack.AddSubscription(OrderVal.Internal, () => {
                actions.GetMana(stats.manaPerAttack.val);
            });

            actions.ResetCountOfAttacks();

        }

        public void OnAddItem(ItemData item) {
            item.Equip(this);
        }

        public void OnRemoveItem(ItemData item) {
            item.DeEquip();
        }

        public void ResetBeforeFight() {
            actions.ResetCountOfAttacks();
        }

        public enum RangeType { 
            Melee, Range
        }

    }

}
