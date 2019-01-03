using UnityEngine;
using System.Collections;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Localizer Multi Format")]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizerTextMeshProMultiFormat : MonoBehaviour {
        /// <summary>
        /// Localization key.
        /// </summary>
        [Localize]
        public string[] keys;

        public string format = "{0}";

        public TextMeshProUGUI uiText { get; private set; }

        private bool mStarted = false;
        private string[] mTexts;

        /// <summary>
        /// Localize the widget on enable, but only if it has been started already.
        /// </summary>

        void OnEnable() { Localize.instance.localizeCallback += Apply; if(mStarted) Apply(); }
        void OnDisable() { if(Localize.instance) Localize.instance.localizeCallback -= Apply; }

        /// <summary>
        /// Localize the widget on start.
        /// </summary>

        void Start() {
            mStarted = true;
            Apply();
        }

        /// <summary>
        /// Force-localize the widget.
        /// </summary>
        public void Apply() {
            if(uiText == null)
                uiText = GetComponent<TextMeshProUGUI>();

            if(mTexts == null || mTexts.Length != keys.Length)
                mTexts = new string[keys.Length];

            for(int i = 0; i < keys.Length; i++) {
                var key = keys[i];
                if(Localize.instance.Exists(key))
                    mTexts[i] = Localize.Get(key);
                else
                    mTexts[i] = "";
            }

            uiText.text = string.Format(format, mTexts);
        }
    }
}