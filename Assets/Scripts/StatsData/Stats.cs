using System;
using System.Linq;
using System.Reflection;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StatsData {

    public enum CharacterClassType {
        Defender, Warrior, Mage
    }

    public class Stats : MonoBehaviour {

        public StatField<int> hp = new StatField<int>(Stat.Hp, 100);
        public StatField<int> maxHp = new StatField<int>(Stat.MaxHp, 100);

        public StatField<int> mana = new StatField<int>(Stat.Mana, 0);
        public StatField<int> maxMana = new StatField<int>(Stat.MaxMana, 100);

        public StatField<int> manaPerAttack = new StatField<int>(Stat.ManaPerAttack, 5);

        /// <summary>
        /// Attack dmg
        /// </summary>
        public StatField<int> AD = new StatField<int>(Stat.Ad, 10);

        /// <summary>
        /// Attack speed
        /// </summary>
        public PercentStatField AS = new PercentStatField(new StatField<float>(Stat.As, 0.5f));

        /// <summary>
        /// Crit chance
        /// </summary>
        public PercentStatField critChance = new PercentStatField(new StatField<float>(Stat.CritChance, 0f));

        /// <summary>
        /// Dmg on crit
        /// </summary>
        public PercentStatField critDmg = new PercentStatField(new StatField<float>(Stat.CritDmg, 2f));

        public StatField<float> moveSpeed = new StatField<float>(Stat.MoveSpeed, 5);

        public StatField<bool> isDie = new StatField<bool>(Stat.IsDie, false);

        public StatField<float> reduceDmg = new StatField<float>(Stat.ReduceDmg, 0);

        /// <summary>
        /// Vampirism
        /// </summary>
        public PercentStatField vampirism = new PercentStatField(new StatField<float>(Stat.Vampirism, 0f));

        public StatField<CharacterClassType[]> classTypes = new StatField<CharacterClassType[]>(Stat.ClassTypes, new CharacterClassType[] { });

        public static Stats operator +(Stats first, Stats second) {

            var firstFields = first.GetFields();

            for (var i = 0; i < firstFields.Count(); i++) {

                var fieldType = firstFields[i].FieldType;
                fieldType.GetMethod("Modify").Invoke(firstFields[i].GetValue(first), new[] { firstFields[i].GetValue(second), false });
            }

            return first;

        }

        public static Stats operator -(Stats first, Stats second) {

            var firstFields = first.GetFields();

            for (var i = 0; i < firstFields.Count(); i++) {

                var fieldType = firstFields[i].FieldType;
                fieldType.GetMethod("Modify").Invoke(firstFields[i].GetValue(first), new[] { firstFields[i].GetValue(second), true });
            }

            return first;

        }

        /// <summary>
        /// Modify stat
        /// </summary>
        /// <param name="statField"></param>
        /// <param name="source"></param>
        /// <param name="modifyType"></param>
        public void Modify(StatField statField, ISource source, ModifyType modifyType = ModifyType.Plus) {

            var func = GetActionForStat(statField, StatAction.Modify, null);

            if (func != null) {
                func();
            }

        }

        public enum StatAction {
            Add, Remove, Modify
        }

        /// <summary>
        /// Get action for stat
        /// </summary>
        /// <param name="statField"></param>
        /// <param name="statAction"></param>
        /// <param name="source"></param>
        /// <param name="statChange"></param>
        /// <returns></returns>
        public Func<StatChange[]> GetActionForStat(StatField statField, StatAction statAction, ISource source, StatChange statChange = null) {

            if (statField.changeStatType == ChangeStatType.PercentHp || statField.changeStatType == ChangeStatType.Hp) {

                StatChange<int> change;
                int val;

                if (statField.changeStatType == ChangeStatType.PercentHp)
                    val = Mathf.RoundToInt(statField.floatVal * maxHp);
                else
                    val = statField.intVal;

                if (statAction == StatAction.Add) {

                    change = new StatChange<int>(new IntObservable(val), source);

                    return () => new[] {
                            hp.AddChange(change),
                            maxHp.AddChange(change)
                        };

                } else if (statAction == StatAction.Modify) {

                    return () => {
                        hp.ModifyBy(new IntObservable(val));
                        maxHp.ModifyBy(new IntObservable(val));
                        return null;
                    };

                } else {

                    return () => {

                        hp.RemoveChange(statChange as StatChange<int>);
                        maxHp.RemoveChange(statChange as StatChange<int>);

                        return null;
                    };

                }

            } else if (statField.changeStatType == ChangeStatType.Ad) {

                StatChange<int> change;
                var val = statField.intVal;

                if (statAction == StatAction.Add) {

                    change = new StatChange<int>(new IntObservable(val), source);

                    return () => new[] {
                            AD.AddChange(change),
                        };

                } else if (statAction == StatAction.Modify) {

                    return () => {
                        AD.ModifyBy(new IntObservable(val));
                        return null;
                    };

                } else {

                    return () => {
                        AD.RemoveChange(statChange as StatChange<int>);
                        return null;
                    };

                }

            } else if (statField.changeStatType == ChangeStatType.As) {

                StatChange<float> change;
                var val = statField.floatVal;

                if (statAction == StatAction.Add) {

                    change = new StatChange<float>(new FloatObsrevable(val), source);

                    return () => new[] {
                        AS.AddChange(change),
                    };

                } else if (statAction == StatAction.Modify) {

                    return () => {
                        AS.ModifyBy(new FloatObsrevable(val));
                        return null;
                    };

                } else {

                    return () => {
                        AS.RemoveChange(statChange as StatChange<float>);
                        return null;
                    };

                }

            } else if (statField.changeStatType == ChangeStatType.CritChange) {

                StatChange<float> change;
                var val = statField.floatVal;

                if (statAction == StatAction.Add) {

                    change = new StatChange<float>(new FloatObsrevable(val), source);

                    return () => new[] {
                        critChance.AddChange(change),
                    };

                } else if (statAction == StatAction.Modify) {

                    return () => {
                        critChance.ModifyBy(new FloatObsrevable(val));
                        return null;
                    };

                } else {

                    return () => {
                        critChance.RemoveChange(statChange as StatChange<float>);
                        return null;
                    };

                }

            }

            return null;

        }

        /// <summary>
        /// Add change by stat field
        /// </summary>
        /// <param name="statField"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public StatChange[] AddChange(StatField statField, ISource source) {

            var func = GetActionForStat(statField, StatAction.Add, source);

            if (func != null) {
                return func();
            }

            return null;

        }

        /// <summary>
        /// Remove exists change
        /// </summary>
        /// <param name="statField"></param>
        /// <param name="statChange"></param>
        public void RemoveChange(StatField statField, StatChange change) {

            var func = GetActionForStat(statField, StatAction.Remove, null, change);

            if (func != null) {
                func();
            }

        }

        public FieldInfo[] GetFields() => GetType().GetFields().ToArray();

    }

}
