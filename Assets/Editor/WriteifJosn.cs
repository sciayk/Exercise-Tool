using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;


public class WriteifJosn :EditorWindow
{

    string LoadDataPath;
    string SaveDataPath;

    LoadExcle Loading = new LoadExcle();
    List<string> ExcelName=new List<string>();

    //Excel Name Dictionary
    Dictionary<string, List<bool>> Excel = new Dictionary<string, List<bool>>();
    Dictionary<int, List<string>> SaveDate = new Dictionary<int, List<string>>();
	Dictionary<string,List<string>> ExcelTableName = new Dictionary<string, List<string>> ();

    List<string> TitleALL = new List<string>();
    List<List<string>> ExcelData=new List<List<string>>();

    List<string> TableName = new List<string>();
	int toolbarInt = 0;
	//List<string> toolbarStrings = new List<string>();

    int Priv = 1000;

    private void OnEnable()
    {
        LoadDataPath = Application.dataPath + "/Excle/";
        SaveDataPath = Application.dataPath + "/Resources/InputData/";
       
        TableName.Clear();
        ExcelName.Clear();
    }
   

    void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Excel2Json Ver.1.0 ");
        GUILayout.Label("選擇讀取資料夾 / Chose you Excel File");
        GUILayout.Label(LoadDataPath);
        if (GUILayout.Button("選擇讀取資料夾 / Chose you Excel File")) {
            LoadDataPath = EditorUtility.OpenFolderPanel("選擇資料夾 / Chose you Excel File","","") + "/";
        }

        GUILayout.Label("Json存放位置 / Json save path");
        GUILayout.Label(SaveDataPath);
        if (GUILayout.Button("選擇存放資料夾  / Json save path"))
        {
            SaveDataPath = EditorUtility.OpenFolderPanel("選擇資料夾 / Json save path", "", "") + "/";

        }

        GUILayout.Label("=====================================================================================================================");
        EditorGUILayout.HelpBox("Close Excel please.", MessageType.Warning);
        if (GUILayout.Button("讀取 / Loading")) {
            if (Directory.Exists(LoadDataPath))
            {
                Priv = 1000;
                string[] Allfile = Directory.GetFiles(LoadDataPath, "*.xlsx");
                ExcelName.Clear();
                for(int i=0;i< Allfile.Length; i++)
                {               
                    if (Allfile[i][0] == '~')
                    {
                        Debug.Log("Not Excel");
                    }
                    else {
                        FileInfo thisInfo = new FileInfo(Allfile[i]);

                        ExcelName.Add(thisInfo.Name);
                      
                        if (ExcelTableName.ContainsKey(thisInfo.Name))
                        {
                            Debug.Log("Have this Excel data");
                            TableName = ExcelTableName[thisInfo.Name];
                        }
                        else
                        {
                          ExcelTableName.Add(thisInfo.Name, TableName);
                        }
                    }
                }       
            }
        }
        GUILayout.Label("Excel Lable");
        if (ExcelName.Count > 0)
        {
            for (int i = 0; i < ExcelName.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ExcelName[i], GUILayout.Width(150));
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.Label("=====================================================================================================================");
        GUILayout.Label("Chose you need TableName");
        if (ExcelName.Count > 0)
        {
            toolbarInt = GUILayout.Toolbar(toolbarInt, ExcelName.ToArray());
            if (Priv != toolbarInt)
            {
                Priv = toolbarInt;
                TableName.Clear();
                TableName = Loading.GetTableName(ExcelName[toolbarInt], LoadDataPath);
            }
            if (TableName.Count > 0)
            {
                for (int i = 0; i < TableName.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(TableName[i]);
                    if (GUILayout.Button("X", GUILayout.Width(30)))
                    {
                        TableName.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }

            }

        }
        GUILayout.Label("=====================================================================================================================");

        GUILayout.EndVertical();
        if (GUILayout.Button("開始轉檔(Json) / Start born Json"))
        {
            try
            {
                ModelCreater();
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                throw;
            }
          
        }
    }
    void ModelCreater()
    {
        lock (this){
            for (int ExcelCount = 0; ExcelCount < ExcelName.Count; ExcelCount++)
            {
                for (int i = 0; i < TableName.Count; i++)
                {
                    if (File.Exists(SaveDataPath + TableName[i] + ".json"))
                    {
                        File.Delete(SaveDataPath + TableName[i] + ".json");
                        //Debug.Log("Delete json : " + TableName[i]);
                    }

                    TitleALL.Clear();
                    TitleALL = Loading.GetExcelTableTitleName(ExcelName[ExcelCount], TableName[i], LoadDataPath);
                    for (int w = 0; w < TitleALL.Count; w++)
                    {
                        SaveDate[w] = Loading.LoadExcleData(TableName[i], TitleALL[w], LoadDataPath);
                    }
                    for (int w = 0; w < SaveDate[0].Count; w++)
                    {
                        List<string> DateExcel = new List<string>();
                        DateExcel.Clear();
                        for (int Sa = 0; Sa < TitleALL.Count; Sa++)
                        {
                            DateExcel.Add(SaveDate[Sa][w]);
                        }
                        ExcelData.Add(DateExcel);
                    }
                    for (int a = 1; a < ExcelData.Count; a++)
                    {
                        outputJsonFile(writeJson(TitleALL.ToArray(), ExcelData[a].ToArray(), a, ExcelData.Count), SaveDataPath + TableName[i] + ".json");
                    }
                   Debug.Log("Born Josn : " + TableName[i]);
 	           System.Threading.Thread.Sleep(200);
	           // Time wait down for 200ms.System maby not Instance
                    ExcelData.Clear();
                }
            }
           
        }
        
    }

    string writeJson(string[] key, string[] value,int a,int Count)
    {

        if (key.Length != value.Length)
        {
            throw new System.Exception("key.Length != value.Length");
        }
        StringBuilder sb = new StringBuilder();
        if (a == 1)
        {
            sb.AppendLine("[");
        }
        sb.AppendLine("{");
        for (int i = 0; i < key.Length; i++)
        {
           // Debug.Log(key[i]+" || "+ value[i]);
            if (i == key.Length - 1)
            {
                sb.AppendLine("\"" + key[i] + "\":\"" + value[i] + "\"");
            }
            else
            {
                sb.AppendLine("\"" + key[i] + "\":\"" + value[i] + "\",");
            }
        }
        if (a == Count - 1)
        {
            sb.AppendLine("}");
            sb.AppendLine("]");
        }
        else {
            sb.AppendLine("},");
        }

        return sb.ToString();
    }
    void outputJsonFile(string sb,string SavePath)
    {
        using (StreamWriter sw = new StreamWriter(SavePath, true))
        {
            sw.WriteLine(sb);
        }
    }
   
}
