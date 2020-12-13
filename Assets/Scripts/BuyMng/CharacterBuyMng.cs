using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.BuyMng {

    public class CharacterBuyMng : MonoBehaviour {

        public IReadOnlyList<BuyData> buyDataList => _buyDataList;

        public IEnumerable<CharacterCtrl> characterCtrls => buyDataList.Select(b => b.ctrlPrefab);

        public UnityEvent postBuy = new UnityEvent();

        public UnityEvent onChangeCtrls = new UnityEvent();

        private readonly List<BuyData> _buyDataList = new List<BuyData>();

        [SerializeField]
        private StoreData storeData;

        public void AddToCtrl(BuyData buyData) {
            _buyDataList.Add(buyData);
            onChangeCtrls.Invoke();
        }

        public void Init() {

            foreach (var ctrlBuy in storeData.buyDatas) {
                AddToCtrl(ctrlBuy);
            }

        }

        public CharacterCtrl Buy(BuyData buyData) {

            GameMng.current.playerData.money.val -= buyData.cost;
            
            var ctrl = GameMng.current.fightMng.fightTeamPlayer.AddCharacterToTeamPrefab(buyData.ctrlPrefab);

            postBuy.Invoke();

            return ctrl;

        }

    }

}
