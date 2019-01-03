using UnityEngine;
using UnityEditor;
using System.Collections;

using TMPro;

namespace M8.TextMeshPro {
    [CustomEditor(typeof(LocalizerTextMeshPro), true)]
    public class LocalizerTextMeshProInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorExt.Utility.DrawSeparator();

            //preview (for now, just base)
            GUI.enabled = LocalizeEdit.isLocalizeFileExists;

            if(GUILayout.Button("Preview")) {
                var dat = target as LocalizerTextMeshPro;

                var textUI = dat.GetComponent<TextMeshProUGUI>();
                if(textUI) {
                    dat.Apply();
                    EditorUtility.SetDirty(textUI);
                }
            }

            GUI.enabled = true;
        }
    }
}