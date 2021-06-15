using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    public class DungeonDataPosition {

        public RoomData startRoom => rooms.First();

        public IEnumerable<RoomData> rooms { get; }

        public DungeonDataPosition(IEnumerable<RoomData> rooms) {
            this.rooms = rooms;
        }

    }

    public abstract class RoomData : ICloneable  {

        public RoomData(string titleKey) {
            _title = titleKey;
        }


        protected readonly string _title;

        public string title => TranslateReader.GetTranslate(_title);

        [Obsolete]
        public Vector2Int size { get; }

        public bool isShowBuyPanel { get; protected set; } = false;

        public IReadOnlyList<ExitFromRoom> exitFromRooms => _exitFromRooms;

        private List<ExitFromRoom> _exitFromRooms = new List<ExitFromRoom>();

        public void AddExit(RoomData roomData, Direction direction) {
            _exitFromRooms.Add(new ExitFromRoom(this, roomData, direction));
        }

        public bool IsThereExit(Direction exit)
            => exitFromRooms.Any(e => e.direction == exit);

        public abstract Sprite GetSprite();

        public virtual string GetRoomName() => "RoomData";

        public abstract string GetRoomDescription();

        public abstract void ComeToRoom(DungeonRoomCells dungeonRoomCells);

        public abstract object Clone();

    }

    public enum Direction {
        top, left, right, bottom
    }

    public class ExitFromRoom : IForSelectPanel {

        public ExitFromRoom(RoomData from, RoomData to, Direction dir) {

            fromRoomData = from;
            toRoomData = to;
            direction = dir;

        }

        public readonly RoomData fromRoomData;

        public readonly RoomData toRoomData;

        public readonly Direction direction;

        public Sprite img => toRoomData.GetSprite();

        public string title => toRoomData?.title;

        public string description => toRoomData.GetRoomDescription();

        private ObservableVal _onChange = new ObservableVal();

        public ObservableVal onChange => _onChange;

        public string selectText => TranslateReader.GetTranslate("enter");

        public bool isEnableToSelect => true;

        public void Select() {

            GameMng.current.selectPanel.Hide();
            GameMng.current.roomCtrl.MoveToNextRoom(direction);


        }
    }

}
