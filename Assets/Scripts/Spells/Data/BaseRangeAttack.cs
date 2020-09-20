using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Bullet;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class BaseRangeAttack : Spell {

        public BulletCtrl bulletPrefab;

        public int dmgAmount;

        public override void Use(CharacterCtrl from, CharacterCtrl to, UseOpts opts) {

            var bullObj = Instantiate(bulletPrefab, from.transform);

            bullObj.transform.localPosition = Vector3.zero;

            bullObj.StartFly(from, to);

            bullObj.onCome.AddListener(() => {
                to.characterData.actions.GetDmg(from, new Dmg(dmgAmount));
                from.characterData.onPostMakeAttack.Invoke();
            });

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }
    }
}
