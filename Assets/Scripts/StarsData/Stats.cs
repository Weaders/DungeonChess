using System.Linq;
using System.Reflection;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StarsData {

    public enum CharacterClassType {
        Defender
    }

    public class Stats : MonoBehaviour {

        public IntObservable hp = new IntObservable(100);
        public IntObservable maxHp = new IntObservable(100);

        public IntObservable mana = new IntObservable(0);
        public IntObservable maxMana = new IntObservable(100);

        public IntObservable manaPerAttack = new IntObservable(5);

        /// <summary>
        /// Attack dmg
        /// </summary>
        public IntObservable AD = new IntObservable(10);

        /// <summary>
        /// Attack speed
        /// </summary>
        public FloatObsrevable AS = new FloatObsrevable(0.5f);

        public FloatObsrevable moveSpeed = new FloatObsrevable(5);

        public BoolObservable isDie = new BoolObservable(false);

        public ArrayObservable<CharacterClassType> classTypes = new ArrayObservable<CharacterClassType>(new CharacterClassType[] { });

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

        public FieldInfo[] GetFields()  => GetType().GetFields()
                .Where(f => f.FieldType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IModifiedObservable<>)))
                .ToArray();
    }

}
