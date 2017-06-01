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

    [MenuItem("MyTool/FlyCat/AutoPushGameObject", false, 0)]

    static void AutoPushGameObject()

    {

        EditorWindow.GetWindow(typeof(PushGameobject));

    }
}
