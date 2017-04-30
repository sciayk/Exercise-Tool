using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckUIAct : EditorWindow
{

    List<string> UI = new List<string>();
    void OnGUI()
    {

        if(GUILayout.Button("GetUI")){
           // GameObject.FindObjectsOfType<>();
        }
    }
}
