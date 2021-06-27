using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.BuyMng {

    [CreateAssetMenu(menuName = "Buy/Data")]
    public class BuyData : CharacterDungeonData {

        public int cost;
        public string descriptionKey;

        public bool IsCanBuy() => GameMng.current.playerData.money >= cost 
            && GameMng.current.playerData.charactersCount < GameMng.current.playerData.maxCharacterCount;

    }
}
