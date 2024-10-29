using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Scramble")]
    public class TextMeshProScramble : MonoBehaviour {
        [Header("Config")]
        [SerializeField]
        int _scrambleDefaultCount;
        [SerializeField]
        float _scrambleDelay = 0.1f;
        [SerializeField]
        float _fixedStringPerCharDelay = 0.1f;
        [SerializeField]
        bool _isRealtime;

        [Header("Display")]
        [SerializeField]
        TMP_Text _target;
                
        public string fixedString { get; private set; }
        public bool isBusy { get { return mRout != null; } }

        private static char[] mScrambleChars;

        private int mScrambleCurInd;
        private int mScrambleCurCount;
        private float mScrambleLastTime;

        private System.Text.StringBuilder mFixedStringBuff;
        private System.Text.StringBuilder mStringBuff;
        private Coroutine mRout;

        public void ApplyFixedImmediate(string str) {
            fixedString = str;
            mScrambleCurCount = str.Length;
            mFixedStringBuff.Clear();
            mFixedStringBuff.Append(str);

            if(_target)
                _target.text = str.ToString();
        }

        public void ApplyFixedString(string str) {
            if(mRout != null)
                StopCoroutine(mRout);

            mRout = StartCoroutine(ApplyFixedStringWait(str));
        }

        public IEnumerator ApplyFixedStringWait(string str) {
            mFixedStringBuff.Clear();

            mScrambleCurCount = str.Length > _scrambleDefaultCount ? str.Length : _scrambleDefaultCount;

            fixedString = str;

            var curFixedStringInd = 0;
            var lastTime = _isRealtime ? Time.realtimeSinceStartup : Time.time;

            var fixedStringLen = fixedString.Length;

            while(curFixedStringInd < fixedStringLen || mScrambleCurCount > fixedStringLen) {
                yield return null;

                var time = _isRealtime ? Time.realtimeSinceStartup : Time.time;
                if(time - lastTime >= _fixedStringPerCharDelay) {
                    //apply a fixed char
                    if(curFixedStringInd < fixedStringLen) {
                        mFixedStringBuff.Append(fixedString[curFixedStringInd]);
                        curFixedStringInd++;
                    }

                    //reduce length if applicable
                    if(mScrambleCurCount > fixedStringLen)
                        mScrambleCurCount--;

                    lastTime = time;
                }
            }

            if(_target)
                _target.text = mFixedStringBuff.ToString();

            mRout = null;
        }

        public void ClearFixedString() {
            fixedString = "";
            mFixedStringBuff.Clear();

            mScrambleCurCount = _scrambleDefaultCount;

            ApplyScrambleText();
        }

        void OnDisable() {
            if(mRout != null) {
                StopCoroutine(mRout);
                mRout = null;
            }
        }

        void OnEnable() {
            ApplyScrambleText();

            mScrambleLastTime = _isRealtime ? Time.realtimeSinceStartup : Time.time;
        }

        void Awake() {
            if(mScrambleChars == null) {
                int asciiStart = 0x21;
                int asciiEnd = 0x7e;

                mScrambleChars = new char[asciiEnd - asciiStart];
                for(int i = 0; i < mScrambleChars.Length; i++)
                    mScrambleChars[i] = (char)(asciiStart + i);

                ArrayUtil.Shuffle(mScrambleChars);
            }

            mScrambleCurCount = _scrambleDefaultCount;

            mFixedStringBuff = new System.Text.StringBuilder(mScrambleCurCount);
            mStringBuff = new System.Text.StringBuilder(mScrambleCurCount);

            if(!_target)
                _target = GetComponent<TMP_Text>();
        }

        void Update() {
            if(mFixedStringBuff.Length < mScrambleCurCount) {
                var time = _isRealtime ? Time.realtimeSinceStartup : Time.time;
                if(time - mScrambleLastTime >= _scrambleDelay) {
                    ApplyScrambleText();
                    mScrambleLastTime = time;
                }
            }
        }

        private void ApplyScrambleText() {
            mStringBuff.Clear();
            mStringBuff.Append(mFixedStringBuff.ToString());

            for(int i = mStringBuff.Length; i < mScrambleCurCount; i++) {
                if(mScrambleCurInd >= mScrambleChars.Length)
                    mScrambleCurInd = 0;

                mStringBuff.Append(mScrambleChars[mScrambleCurInd]);
                mScrambleCurInd++;
            }

            if(_target)
                _target.text = mStringBuff.ToString();
        }
    }
}