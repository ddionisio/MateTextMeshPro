using UnityEngine;
using System.Collections;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Version")]
    public class TextMeshProVersion : MonoBehaviour {
        public TMP_Text target;

        public void Apply() {
            if(!target)
                target = GetComponent<TMP_Text>();

            if(target)
                target.text = Application.version;
        }

        void Awake() {
            Apply();
        }
    }
}