using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CommentMonoBehaviour))]
public class CommentMonoBehaviourEditor : Editor
{
    private CommentMonoBehaviour script { get { return target as CommentMonoBehaviour; } }

    public override void OnInspectorGUI()
    {
        Color oldColor = GUI.color;

        GUI.color = Color.green;
        script.Comment = EditorGUILayout.TextField("Comment", script.Comment);

        GUI.color = oldColor;
    }
}
