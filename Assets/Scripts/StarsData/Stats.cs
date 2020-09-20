using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StarsData {

    public enum CharacterClassType {
        Defender
    }

    public enum Stat { 
        Hp, MaxHp, Mana, MaxMana, ManaPerAttack,
        Ad, As, MoveSpeed, IsDie, ClassTypes,
        Vampirism
    }

    public static class StatTypeExtension {

        public static ObservableVal GetObservableVal(this Stat stat) {

            switch (stat) {
                case Stat.Ad:
                case Stat.Hp:
                case Stat.MaxHp:
                case Stat.Mana:
                case Stat.MaxMana:
                case Stat.ManaPerAttack:
                case Stat.Vampirism:
                    return new IntObservable(0);
                case Stat.IsDie:
                    return new BoolObservable(false);
                case Stat.ClassTypes:
                    return new CharacterClassTypeObservable(new CharacterClassType[] { });
                case Stat.As:
                case Stat.MoveSpeed:
                    return new FloatObsrevable(0f);
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
        public StatField<float> AS = new StatField<float>(Stat.As, 0.5f);

        public StatField<float> moveSpeed = new StatField<float>(Stat.MoveSpeed, 5);

        public StatField<bool> isDie = new StatField<bool>(Stat.IsDie, false);

        /// <summary>
        /// Vampirism
        /// </summary>
        public StatField<int> vampirism = new StatField<int>(Stat.Vampirism, 0);

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

        public void Mofify(StatField statField, bool alterVal = false) {

            var field = GetStatFieldInfo(statField.stat);

            var val = field.GetValue(this);

            field.FieldType.GetMethod("ModifyBy").Invoke(val, new object[] { statField.observableVal, alterVal });

        }

        public FieldInfo[] GetFields() => GetType().GetFields()
                //.Where(f => f.FieldType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IModifiedObservable<>)))
                .ToArray();
    }

}
