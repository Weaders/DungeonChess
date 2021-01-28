using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Observable {

    public static class TextExtension {

        public static SubsribeTextResult Subscribe(this TextMeshProUGUI textObj, ObservableVal val, OrderVal orderVal = OrderVal.UIUpdate) {

            void changeText() { 
                textObj.text = val.ToString(); 

            };

            val.onPostChangeBase.AddSubscription(orderVal, changeText);

            changeText();

            return new SubsribeTextResult(changeText, val);

        }

        public static SubsribeTextResult Subscribe(this Text textObj, ObservableVal val, OrderVal orderVal = OrderVal.UIUpdate) {

            void changeText() {
                textObj.text = val.ToString();
            } 

            textObj.text = val.ToString();
            val.onPostChangeBase.AddSubscription(orderVal, changeText);

            changeText();

            return new SubsribeTextResult(changeText, val);

        }

        public static SubsribeTextResult Subscribe(this Text textObj, Func<string> onChange, OrderVal orderVal, params ObservableVal[] stats)
            => Subscribe(textObj, onChange, orderVal, (IEnumerable<ObservableVal>)stats);

        public static SubsribeTextResult Subscribe(this Text textObj, Func<string> onChange, OrderVal orderVal, IEnumerable<ObservableVal> stats)
            => Subscribe(textObj, () => new TextData 
            {
                text = onChange()
            }, orderVal, stats);

        public static SubsribeTextResult Subscribe(this Text textObj, Func<TextData> onChange, OrderVal orderVal, IEnumerable<ObservableVal> stats) {

            void changeText() {

                var str = onChange.Invoke();

                textObj.text = str.text;

                if (str.color.HasValue)
                    textObj.color = str.color.Value;

            }

            foreach (var stat in stats) {
                stat.onPostChangeBase.AddSubscription(orderVal, changeText);
            }

            changeText();

            return new SubsribeTextResult(changeText, stats);

        }

        public static SubsribeTextResult SubscribeStatWithMaxVal(this Text textObj, OrderVal orderVal, ObservableVal<int> stat, ObservableVal<int> maxStat) {

            TextData subscribe() {

                var result = new TextData();

                if (stat.val < maxStat.val) {
                    result.color = Color.red;
                } else {
                    result.color = Color.green;
                }

                result.text = stat.ToString();
                return result;

            }

            return Subscribe(textObj, subscribe, orderVal, new[] { stat, maxStat });
            

        }
        public class TextData {
            public string text;
            public Color? color = null;
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
