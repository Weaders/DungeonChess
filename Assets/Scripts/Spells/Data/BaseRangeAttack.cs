using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Bullet;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class BaseRangeAttack : Spell, IDmgSource {

        public GameObject bulletPrefab;

        public int dmgAmount;

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseOpts opts) {

            var bullObj = Instantiate(bulletPrefab, from.transform);

            var bullet = bullObj.GetComponent<IBullet>();

            //if (bullet is Targ)

            //bullObj.transform.localPosition = Vector3.zero;

            //bullObj.StartFly(from, to);

            //bullObj.onCome.AddListener(() => {

            //    to.characterData.actions.GetDmg(from, new Dmg(dmgAmount, this));
            //    from.characterData.onPostMakeAttack.Invoke();

            //});

            return null;

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

        public string GetId() => Id;
    }
}
