using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTool : EditorWindow
{
    void OnGUI()
    {

        GUILayout.BeginVertical();

        GUILayout.Label(" Excel2Json Ver.1.0 ");
        GUILayout.Label(" 作者很懶不想更新 ");

        

        GUILayout.Label("讀取Excel位置");
        EditorGUILayout.TextField("開啟");
        GUILayout.Label("Excel存放位置");
        GUILayout.TextField("No");
        GUILayout.Label("選擇Excel");
        GUILayout.TextField("No");
        GUILayout.Label("選擇需要轉換Table");
        GUILayout.TextField("No");
        GUILayout.Label("=======================================================================");
        GUILayout.Button("GO", GUILayout.Width(50));

        GUILayout.EndVertical();

    }
}
