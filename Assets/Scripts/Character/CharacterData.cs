using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Buffs;
using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using Assets.Scripts.StatsData;
using Assets.Scripts.State;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Character {

    public class CharacterData : MonoBehaviour, IHaveItemsContainer {

        public Stats stats;

        public SpellContainer spellsContainer;

        public BuffsContainer buffsContainer;

        public ItemsContainer itemsContainer;

        public StateContainer stateContainer;

        public Actions actions;

        public OrderedEvents onPreMakeAttack = new OrderedEvents();

        public OrderedEvents onPostMakeAttack = new OrderedEvents();

        public CharacterCtrl characterCtrl;

        public ObservableVal<string> characterName;

        public bool isCanMove => stateContainer.All(s => s.isCharCanMove);

        public bool isCanAttack => stateContainer.All(s => s.isCharCanAttack);

        public RangeType rangeType {
            get {

                if (spellsContainer.Count == 0)
                    return default;

                return spellsContainer.GetBaseAttackSpell().range <= 1 ? RangeType.Melee : RangeType.Range;

            }
        }

        private void Awake() {
            actions = new Actions(this);
        }

        public void Init() {

            spellsContainer.Init(this);

            stateContainer.Init(this);

            buffsContainer.Init(characterCtrl);

            itemsContainer.SetOwner(this);

            itemsContainer.onSet.AddSubscription(OrderVal.Internal, (setData) => {

                if (setData.oldData != null)
                    setData.oldData.DeEquip();

                if (setData.data != null)
                    setData.data.Equip(this);

            });

            stats.hp.onPostChange.AddSubscription(OrderVal.Internal, (data) => {

                if (data.newVal <= 0)
                    stats.isDie.val = true;

            });

            onPostMakeAttack.AddSubscription(OrderVal.Internal, () => {
                actions.GetMana(stats.manaPerAttack.val);
            });

            actions.ResetCountOfAttacks();

        }

        public void ResetBeforeFight() {
            actions.ResetCountOfAttacks();
        }

        public enum RangeType { 
            Melee, Range
        }

    }

}
