using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Copy")]
	[ExecuteInEditMode]
    public class TextMeshProCopy : MonoBehaviour {
		public TMP_Text source;
		public TMP_Text destination;

		void Awake() {
			if(!destination)
				destination = GetComponent<TMP_Text>();
		}

		void Update() {
			if(source && destination && destination.text != source.text)
				destination.text = source.text;
		}
	}
}