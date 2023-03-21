﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace M8.TextMeshPro {
    [AddComponentMenu("M8/TextMeshPro/Typewriter")]
    public class TextMeshProTypewriter : MonoBehaviour {
        public TMP_Text label;
        public float delay;

        public bool autoText = false;
        public bool autoPlay = true;
        public bool useRealTime;

        public string text {
            get {
                return mString;
            }

            set {
                mString = value;

                if(autoPlay)
                    Play();
            }
        }

        public event System.Action proceedCallback;
        public event System.Action doneCallback;

        public bool isBusy { get { return mRout != null; } }

        private string mString;
        private System.Text.StringBuilder mStringBuff;
        private Coroutine mRout;

        private bool mIsStarted;

        public void Play() {
            if(mRout != null)
                StopCoroutine(mRout);

            if(gameObject.activeInHierarchy)
                mRout = StartCoroutine(DoType());
        }

        public void Skip() {
            if(mRout != null) {
                StopCoroutine(mRout);
                mRout = null;

                label.text = mString;

                if(doneCallback != null)
                    doneCallback();
            }
        }

        void OnEnable() {
            if(autoPlay && mIsStarted) {
                if(autoText)
                    mString = label.text;

                Play();
            }
        }

        void OnDisable() {
            label.text = "";

            if(mRout != null) {
                StopCoroutine(mRout);
                mRout = null;
            }
        }

        void Start() {
            if(autoPlay) {
                mIsStarted = true;

                if(autoText)
                    mString = label.text;

                Play();
            }
        }

        IEnumerator DoType() {
            var wait = useRealTime ? null : new WaitForSeconds(delay);

            label.text = "";

            if(mString != null) {
                if(mStringBuff == null || mStringBuff.Capacity < mString.Length)
                    mStringBuff = new System.Text.StringBuilder(mString.Length);
                else
                    mStringBuff.Remove(0, mStringBuff.Length);

                int count = mString.Length;
                for(int i = 0; i < count; i++) {
                    if(useRealTime) {
                        var lastTime = Time.realtimeSinceStartup;
                        while(Time.realtimeSinceStartup - lastTime < delay)
                            yield return null;
                    }
                    else
                        yield return wait;

                    if(mString[i] == '<') {
                        int endInd = -1;
                        bool foundEnd = false;
                        for(int j = i + 1; j < mString.Length; j++) {
                            if(mString[j] == '>') {
                                endInd = j;
                                if(foundEnd)
                                    break;
                            }
                            else if(mString[j] == '/')
                                foundEnd = true;
                        }

                        if(endInd != -1 && foundEnd) {
                            mStringBuff.Append(mString, i, (endInd - i) + 1);
                            i = endInd;
                        }
                        else
                            mStringBuff.Append(mString[i]);
                    }
                    else
                        mStringBuff.Append(mString[i]);

                    label.text = mStringBuff.ToString();

                    if(proceedCallback != null)
                        proceedCallback();
                }
            }

            mRout = null;

            if(doneCallback != null)
                doneCallback();
        }
    }
}