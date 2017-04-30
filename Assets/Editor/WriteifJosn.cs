using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using UnityEditor;
using Excel;

public class WriteifJosn :EditorWindow
{

    string LoadDataPath;
    string SaveDataPath;

    LoadExcle Loading = new LoadExcle();

	List<string> Tabl = new List<string>() {"DataInput", "DataInput_JP",  "SoundLib", "FaceLib"};

    List<string> ExcelName=new List<string>();
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
        GUILayout.Label("選擇讀取資料夾");
        GUILayout.Label(LoadDataPath);
        if (GUILayout.Button("選擇讀取資料夾")) {
            LoadDataPath = EditorUtility.OpenFolderPanel("選擇資料夾","","") + "/";
        }

        GUILayout.Label("Json存放位置");
        GUILayout.Label(SaveDataPath);
        if (GUILayout.Button("選擇存放資料夾"))
        {
            SaveDataPath = EditorUtility.OpenFolderPanel("選擇資料夾", "", "") + "/";

        }

        GUILayout.Label("=====================================================================================================================");
        EditorGUILayout.HelpBox("請先關閉Excel，才能讀取Excel", MessageType.Warning);
        if (GUILayout.Button("讀取")) {
            if (Directory.Exists(LoadDataPath))
            {
                Priv = 1000;
                string[] Allfile = Directory.GetFiles(LoadDataPath, "*.xlsx");
                ExcelName.Clear();
                for(int i=0;i< Allfile.Length; i++)
                {               
                    if (Allfile[i][0] == '~')
                    {
                        Debug.Log("是暫存檔");
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
        GUILayout.Label("Excel列表");
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
        GUILayout.Label("Chose u Setting Excel");
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
                    TableName[i] = EditorGUILayout.TextField(TableName[i]);
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
        if (GUILayout.Button("開始轉檔(Json)"))
        {
            ModelCreater();
        }
    }
    public void ModelCreater()
    {
        for (int ExcelCount = 0; ExcelCount < ExcelName.Count; ExcelCount++)
        {
            for (int i = 0; i < TableName.Count; i++)
            {
                File.Delete(SaveDataPath + TableName[i] + ".json");
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
                Debug.Log("End Born Josn : " + TableName[i]);
                ExcelData.Clear();
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
