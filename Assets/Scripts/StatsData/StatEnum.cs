using Assets.Scripts.ActionsData;
using Assets.Scripts.Observable;

namespace Assets.Scripts.StatsData {

    public enum Stat {
        Hp, MaxHp, Mana, MaxMana, ManaPerAttack,
        Ad, As, MoveSpeed, IsDie, ClassTypes,
        Vampirism, CritChance, CritDmg, ReduceDmg,
        None
    }

    public static class StatTypeExtension {

        public static ObservableVal GetObservableVal(this ChangeStatType stat, StatField statField = null) {

            switch (stat) {
                case ChangeStatType.Hp:
                case ChangeStatType.Ad:
                    return new IntObservable(statField == null ? 0 : statField.intVal);
                case ChangeStatType.As:
                case ChangeStatType.PercentHp:
                case ChangeStatType.CritChange:
                    return new FloatObsrevable(statField == null ? default : statField.floatVal);
                case ChangeStatType.None:
                    return null;
                default:
                    throw new System.Exception("Can not get observable for this type");
            }

        }

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
                        return new FloatObsrevable(default);
                default:
                    throw new System.Exception("Can not get observable for this type");
            }

        }

        public static StatChange GetStatChange(this Stat stat, StatField statField, ISource source) {

            switch (stat) {
                case Stat.Ad:
                case Stat.Hp:
                case Stat.MaxHp:
                case Stat.Mana:
                case Stat.MaxMana:
                case Stat.ManaPerAttack:
                    return new StatChange<int>(new IntObservable(statField.intVal), source);
                case Stat.IsDie:
                    return new StatChange<bool>(new BoolObservable(statField.boolVal), source);
                case Stat.ClassTypes:
                    return new StatChange<CharacterClassType[]>(new CharacterClassTypeObservable(new CharacterClassType[] { }), source);
                case Stat.As:
                case Stat.MoveSpeed:
                case Stat.CritChance:
                case Stat.Vampirism:
                    var obs = new PercentFloatObsrevable(new FloatObsrevable(statField.floatVal));
                    return new StatChange<float>(obs, source);
                default:
                    throw new System.Exception("Can not get observable for this type");
            }

        }

    }
}
