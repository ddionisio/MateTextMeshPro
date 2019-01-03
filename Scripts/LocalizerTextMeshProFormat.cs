using UnityEngine;
using System.Collections;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Localizer Format")]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizerTextMeshProFormat : LocalizerTextMeshPro {
        public string format = "{0}";

        protected override void ApplyToLabel(string text) {
            uiText.text = string.Format(format, text);
        }
    }
}