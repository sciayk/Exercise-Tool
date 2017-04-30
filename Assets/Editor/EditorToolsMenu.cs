using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorToolsMenu : EditorWindow {

    [MenuItem("MyTool/FlyCat/Excel2Json", false, 0)]
    static void ModelCreater()
    {
        EditorWindow.GetWindow(typeof(WriteifJosn));
    }

	[MenuItem("MyTool/FlyCat/CheckUI", false, 0)]
    static void CheckUI()
    {
        EditorWindow.GetWindow(typeof(CheckUIAct));
    }

}
