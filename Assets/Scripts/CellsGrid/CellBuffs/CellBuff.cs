using System.Collections.Generic;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.CellsGrid.CellBuffs {

    [RequireComponent(typeof(Cell))]
    public class CellBuff : MonoBehaviour {

        [SerializeField]
        private Buff buffPrefab;

        [SerializeField]
        private Material cellBuffMaterial;

        private void Awake() {

            var cell = GetComponent<Cell>();

            cell.onStayCtrl.AddListener(OnCome);
            cell.SetMaterial(cellBuffMaterial);

        }

        private Dictionary<CharacterCtrl, Buff> buffs = new Dictionary<CharacterCtrl, Buff>();

        protected virtual void OnCome(CharacterCtrl from, CharacterCtrl to) {

            if (from != null)
                RemoveBuff(from);

            if (to != null)
                AddBuff(buffPrefab, to);

        }

        protected void AddBuff(Buff buffPrefab, CharacterCtrl to) {
            var buff = to.characterData.buffsContainer.AddPrefab(buffPrefab);
            buffs.Add(to, buff);
        }

        protected void RemoveBuff(CharacterCtrl from) {
            
            from.characterData.buffsContainer.Remove(buffs[from]);
            buffs.Remove(from);

        }

    }
}
