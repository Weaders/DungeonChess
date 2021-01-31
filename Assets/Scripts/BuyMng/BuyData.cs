using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.BuyMng {

    [CreateAssetMenu(menuName = "Buy/Data")]
    public class BuyData : ScriptableObject {

        public CharacterCtrl ctrlPrefab;
        public int cost;
        public string descriptionKey;

        public bool IsCanBuy() => GameMng.current.playerData.money >= cost && GameMng.current.playerData.charactersCount < GameMng.current.playerData.maxCharacterCount;

    }
}
