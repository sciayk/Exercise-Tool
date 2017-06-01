using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;

public class PushGameobject : EditorWindow
{
    public List<Object> PushGameObject = new List<Object>();

    void OnGUI()
    {
        GUILayout.Label("Auto Push Public GameObject");
        EditorGUILayout.HelpBox("Call script function name => PushGameObject ", MessageType.Info);
        if (GUILayout.Button("Add new"))
        {
            PushGameObject.Add(new Object());
        }
        if (GUILayout.Button("Clear all"))
        {
            PushGameObject.Clear();
        }
        if (GUILayout.Button("Start Push"))
        {
            PushGbj();
        }
        for (int Count = 0; Count < PushGameObject.Count; Count++)
        {
            GUILayout.BeginHorizontal();
            PushGameObject[Count] = EditorGUILayout.ObjectField(PushGameObject[Count], typeof(Object), true);
            if (GUILayout.Button("X")) {
                PushGameObject.Remove(PushGameObject[Count]);
            }
            GUILayout.EndHorizontal();
        }
    }

    void PushGbj()
    {
        foreach (var CheckClass in PushGameObject)
        {
            GameObject UseGame = (GameObject)CheckClass;
            MonoBehaviour[] CheckScript = UseGame.GetComponents<MonoBehaviour>();
            for (int Count = 0; Count < CheckScript.Length; Count++)
            {
                if (!CheckScript[Count].IsInvoking("PushGameObject"))
                {
                    CheckScript[Count].Invoke("PushGameObject", 0.1f);
                }
            }
        }
    }
}
