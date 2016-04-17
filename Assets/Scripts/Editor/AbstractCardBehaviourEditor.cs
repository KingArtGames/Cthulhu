using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Scripts.CardBehaviours
{
    [CustomEditor(typeof(AbstractCardBehaviour), true)]
    public class AbstractCardBehaviourEditor : Editor
    {
        private AbstractCardBehaviour script { get { return target as AbstractCardBehaviour; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            string description = script.GetDescription();
            GUI.enabled = false;
            EditorGUILayout.LabelField("Description");
            EditorGUILayout.TextArea((description == null) ? "null" : description);
            GUI.enabled = true;
        }
    }
}