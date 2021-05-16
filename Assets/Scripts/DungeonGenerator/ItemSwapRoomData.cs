using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class ItemSwapRoomData : RoomData {
        public ItemSwapRoomData(string titleKey) : base(titleKey) {
        }

        public override object Clone()
            => new ItemSwapRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {
            


        }

        public override string GetRoomDescription() {
            throw new NotImplementedException();
        }

        public override Sprite GetSprite() {
            throw new NotImplementedException();
        }

    }
}
