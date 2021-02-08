using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.BuyMng {

    [CreateAssetMenu(menuName = "Buy/Data")]
    public class BuyData : ScriptableObject {

        public CharacterCtrl ctrlPrefab;
        public int cost;
        public string descriptionKey;
        public StatGroup[] data;

        public bool IsCanBuy() => GameMng.current.playerData.money >= cost && GameMng.current.playerData.charactersCount < GameMng.current.playerData.maxCharacterCount;

        public StatGroup GetStatGroup(int level, bool fallbackGetMax = false) {

            StatGroup maxGroup = null;

            foreach (var statGroup in data) {

                if (maxGroup == null)
                    maxGroup = statGroup;
                else if (maxGroup.levelOfDifficult < statGroup.levelOfDifficult)
                    maxGroup = statGroup;

                if (statGroup.levelOfDifficult == level)
                    return statGroup;

            }

            if (fallbackGetMax)
                return maxGroup;

            return data.First(d => d.levelOfDifficult == -1);

        }

    }
}
