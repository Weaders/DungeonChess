using System;

namespace Assets.Scripts.Translate {

    [Serializable]
    public class TranslateModel {

        public string lang;

        public TranslateVal[] translations;

    }

    [Serializable]
    public class TranslateVal {
        public string key;
        public string value;
    }

}
