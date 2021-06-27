using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.BuyMng {

    public class CharacterBuyMng : MonoBehaviour {

        public IReadOnlyList<BuyData> buyDataList => _buyDataList;

        public IEnumerable<CharacterCtrl> characterCtrls => buyDataList.Select(b => b.characterCtrl);

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
            
            var ctrl = GameMng.current.fightMng.fightTeamPlayer.AddCharacterToTeamPrefab(buyData.characterCtrl);

            GameMng.current.buyPanelUI.selectedBuyData = null;

            GameMng.current.playerData.charactersCount.val++;

            GameMng.current.playerData.money.val -= buyData.cost;

            postBuy.Invoke();

            ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Internal, () => {

                if (GameMng.current.playerData.charactersCount > 0)
                    GameMng.current.playerData.charactersCount.val--;

            });

            FullTeamBuffMng.Recalc();

            return ctrl;

        }

    }

}
