using UnityEngine;

namespace Assets.Scripts.BuyMng {

    [CreateAssetMenu(menuName = "Buy/StoreData")]
    public class StoreData : ScriptableObject {

        public BuyData[] buyDatas;

    }
}
