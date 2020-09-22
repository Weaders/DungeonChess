using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells.Modifiers;

namespace Assets.Scripts.ActionsData {

    public class Actions {

        private readonly CharacterData _characterData;

        public OrderedEvents<DmgEventData> onPreMakeDmg = new OrderedEvents<DmgEventData>();

        public OrderedEvents<DmgEventData> onPostMakeDmg = new OrderedEvents<DmgEventData>();

        public OrderedEvents<DmgEventData> onPreGetDmg = new OrderedEvents<DmgEventData>();

        public OrderedEvents<DmgEventData> onPostGetDmg = new OrderedEvents<DmgEventData>();

        public OrderedEvents<HealEventData> onPostGetHeal = new OrderedEvents<HealEventData>();

        public Actions(CharacterData charData) {
            _characterData = charData;
        }

        public void GetDmg(CharacterCtrl from, Dmg dmg) {

            var dmgEventData = new DmgEventData() {
                dmg = dmg,
                from = from,
                to = _characterData.characterCtrl
            };

            from.characterData.actions.onPreMakeDmg.Invoke(dmgEventData);
            onPreGetDmg.Invoke(dmgEventData);

            TagLogger<Actions>.Info(dmg.ToString());

            _characterData.stats.hp.val -= dmg.GetCalculateVal();

            onPostMakeDmg.Invoke(dmgEventData);
            onPostGetDmg.Invoke(dmgEventData);

        }

        public void GetMana(int mana) {

            _characterData.stats.mana.val += mana;

        }

        public void GetHeal(CharacterCtrl from, Heal heal) {

            var healEventData = new HealEventData() {
                heal = heal,
                from = from
            };

            _characterData.stats.hp.val += heal.GetCalculateVal();

            onPostGetHeal.Invoke(healEventData);
        }

        public class DmgEventData {

            public DmgEventData() { }

            public Dmg dmg { get; set; }
            public CharacterCtrl from { get; set; }
            public CharacterCtrl to { get; set; }

        }

        public class HealEventData { 
            
            public Heal heal { get; set; }

            public CharacterCtrl from { get; set; }

        }

    }

    public interface IDmgSource {
        string GetId();
    }

    public class Dmg {

        public List<DmgModifier> dmgModifiers { get; private set; } = new List<DmgModifier>();

        public int initVal { get; private set; }

        public IDmgSource source { get; private set; }

        public Dmg(int val, IDmgSource dmgSource) {

            initVal = val;
            source = dmgSource;

        }

        public int GetCalculateVal() {

            var currentVal = initVal;

            foreach (var modify in dmgModifiers.OrderBy(a => a.order)) {
                currentVal = modify.Modify(this, currentVal);
            }

            return currentVal;

        }

        public override string ToString() {
            return $"Make {GetCalculateVal()} dmg";
        }

    }

    public class Heal {

        public readonly int initVal;

        public Heal(int heal) {
            initVal = heal;
        }

        public int GetCalculateVal() {
            return initVal;
        }

    }

}
