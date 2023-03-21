using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace M8.TextMeshPro {
    /// <summary>
    /// Useful for TextMeshPro components outside of the UI system.
    /// </summary>
    [AddComponentMenu("M8/TextMeshPro/ColorAlphaGroup")]
    public class TextMeshProColorAlphaGroup : MonoBehaviour {
        public bool initOnAwake = true;

        public TMP_Text[] targets;

        public float alpha {
            get { return mAlpha; }
            set {
                if(mAlpha != value || !mIsApplied) {
                    Apply(value);
                }
            }
        }

        private float[] mGraphicDefaultAlphas;

        private bool mIsApplied = false;
        private float mAlpha = 1f;

        public void Apply(float a) {
            if(targets == null || targets.Length == 0 || (mGraphicDefaultAlphas != null && targets.Length != mGraphicDefaultAlphas.Length))
                Init();
            else if(mGraphicDefaultAlphas == null)
                InitDefaultData();

            for(int i = 0; i < targets.Length; i++) {
                if(targets[i]) {
                    var clr = targets[i].color;
                    clr.a = mGraphicDefaultAlphas[i] * a;
                    targets[i].color = clr;
                }
            }

            mIsApplied = true;
            mAlpha = a;
        }

        public void Revert() {
            if(mIsApplied) {
                mIsApplied = false;

                if(targets == null || mGraphicDefaultAlphas == null)
                    return;

                for(int i = 0; i < targets.Length; i++) {
                    if(targets[i]) {
                        var clr = targets[i].color;
                        clr.a = mGraphicDefaultAlphas[i];
                        targets[i].color = clr;
                    }
                }

                mAlpha = 1f;
            }
        }

        public void Init() {
            Revert();

            targets = GetComponentsInChildren<TMP_Text>(true);

            InitDefaultData();
        }

        void OnDestroy() {
            Revert();
        }

        void Awake() {
            if(initOnAwake && (targets == null || targets.Length == 0))
                Init();
        }

        private void InitDefaultData() {
            mGraphicDefaultAlphas = new float[targets.Length];

            for(int i = 0; i < targets.Length; i++)
                mGraphicDefaultAlphas[i] = targets[i].color.a;
        }
    }
}