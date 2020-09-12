using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Observable {

    public static class TextExtension {

        public static SubsribeTextResult Subscribe(this Text textObj, ObservableVal val, OrderVal orderVal = OrderVal.UIUpdate) {

            UnityAction changeText = () => textObj.text = val.ToString();

            textObj.text = val.ToString();
            val.onPostChangeBase.AddSubscription(orderVal, changeText);

            changeText();

            return new SubsribeTextResult(changeText, val);

        }

        public static SubsribeTextResult Subscribe(this Text textObj, Func<string> onChange, OrderVal orderVal, params ObservableVal[] stats)
            => Subscribe(textObj, onChange, orderVal, (IEnumerable<ObservableVal>)stats);

        public static SubsribeTextResult Subscribe(this Text textObj, Func<string> onChange, OrderVal orderVal, IEnumerable<ObservableVal> stats) {

            UnityAction changeText = () => {

                var str = onChange.Invoke();
                textObj.text = str;

            };

            foreach (var stat in stats) {
                stat.onPostChangeBase.AddSubscription(orderVal, changeText);
            }

            changeText();

            return new SubsribeTextResult(changeText, stats);

        }

    }

    public interface ISubsribeTextResult { }

    public class SubsribeTextResult {

        private readonly UnityAction _act;
        private readonly IEnumerable<ObservableVal> _vals;

        public SubsribeTextResult(UnityAction act, params ObservableVal[] stats) : this(act, (IEnumerable<ObservableVal>)stats) {

        }

        public SubsribeTextResult(UnityAction act, IEnumerable<ObservableVal> stats) {
            _act = act;
            _vals = stats;
        }

        public void Unsubscribe() {

            foreach (var stat in _vals)
                stat.onPostChangeBase.RemoveSubscription(_act);

        }

    }

}
