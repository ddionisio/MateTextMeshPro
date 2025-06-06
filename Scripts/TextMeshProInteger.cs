using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Integer")]
	[ExecuteInEditMode]
	public class TextMeshProInteger : MonoBehaviour {
		public TMP_Text target;
		public string format = "";

		[SerializeField]
		int _number;

		public int number {
			get { return _number; }
			set {
				if(_number != value) {
					_number = value;
					Refresh();
				}
			}
		}

		public void Refresh() {
			if(target) {
				if(!string.IsNullOrEmpty(format))
					target.text = _number.ToString(format);
				else
					target.text = _number.ToString();
			}
		}

		void OnDidApplyAnimationProperties() {
			Refresh();
		}

		void OnEnable() {
			Refresh();
		}
	}
}