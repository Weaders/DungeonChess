using System.Linq;
using System.Reflection;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StatsData {

    public enum CharacterClassType {
        Defender, Warrior, Mage
    }

    public enum Stat {
        Hp, MaxHp, Mana, MaxMana, ManaPerAttack,
        Ad, As, MoveSpeed, IsDie, ClassTypes,
        Vampirism, CritChance, CritDmg
    }

    public static class StatTypeExtension {

        public static ObservableVal GetObservableVal(this Stat stat, StatField statField = null) {

            switch (stat) {
                case Stat.Ad:
                case Stat.Hp:
                case Stat.MaxHp:
                case Stat.Mana:
                case Stat.MaxMana:
                case Stat.ManaPerAttack:
                    return new IntObservable(statField == null ? 0 : statField.intVal);
                case Stat.IsDie:
                    return new BoolObservable(statField == null ? default : statField.boolVal);
                case Stat.ClassTypes:
                    return new CharacterClassTypeObservable(new CharacterClassType[] { });
                case Stat.As:
                case Stat.MoveSpeed:
                case Stat.CritChance:
                case Stat.Vampirism:
                    if (statField != null)
                        return new PercentFloatObsrevable(new FloatObsrevable(statField.floatVal));
                    else
                        return new FloatObsrevable(statField == null ? default : statField.floatVal);
                default:
                    throw new System.Exception("Can not get observable for this type");
            }

        }

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

        public ObservableVal<T> GetStat<T>(Stat stat) {

            var field = GetStatFieldInfo(stat);

            if (field != null) {
                return field.GetValue(this) as ObservableVal<T>;
            }

            return null;

        }

        public IStatField GetStat(Stat stat) {

            var field = GetStatFieldInfo(stat);

            if (field != null) {
                return field.GetValue(this) as IStatField;
            }

            return null;

        }

        private FieldInfo GetStatFieldInfo(Stat stat)
            => GetFields().FirstOrDefault(f => (f.GetValue(this) as IStatField).stat == stat);

        public void Mofify(StatField statField, ModifyType modifyType = ModifyType.Plus) {

            var field = GetStatFieldInfo(statField.stat);

            var val = field.GetValue(this);

            field.FieldType.GetMethod("ModifyBy").Invoke(val, new object[] { statField.observableVal, modifyType });

        }

        public FieldInfo[] GetFields() => GetType().GetFields().ToArray();

    }

}
