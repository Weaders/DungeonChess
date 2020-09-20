using System.Collections.Generic;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.BuyMng {

    public class CharacterBuyMng : MonoBehaviour {

        public IReadOnlyList<BuyData> buyDataList => _buyDataList;

        public IReadOnlyList<CharacterCtrl> characterCtrls => _ctrls;

        public UnityEvent postBuy = new UnityEvent();

        private readonly List<CharacterCtrl> _ctrls = new List<CharacterCtrl>();

        private readonly List<BuyData> _buyDataList = new List<BuyData>();

        private int count = 0;

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
            
            var ctrlObj = Instantiate(buyData.ctrlPrefab);
            ctrlObj.gameObject.name = $"{ctrlObj.gameObject.name}_{++count}";

            var ctrl = ctrlObj.GetComponent<CharacterCtrl>();

            ctrl.Init();

            GameMng.current.fightMng.fightTeamPlayer.AddCharacterToTeam(ctrl);

            postBuy.Invoke();

            return ctrl;

        }

        public bool IsCanBuy(BuyData buyData)
            => GameMng.current.playerData.money >= buyData.cost;

        public UnityEvent onChangeCtrls = new UnityEvent();

    }

}
