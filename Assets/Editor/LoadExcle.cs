using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;


public class LoadExcle  {

    Dictionary<string, int> TitleName = new Dictionary<string, int>();
    string ExcelNameData;

    public List<string> GetTableName(string ExcelName, string Path) {
        ExcelNameData = ExcelName;
        FileStream stream = File.Open(Path + "" + ExcelName, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        List<string> TableName=new List<string>();
        do
        {
            TableName.Add(excelReader.Name);
        } while (excelReader.NextResult());
        stream.Close();
        return TableName;
    }
    //不須附檔名
    public List<string> GetExcelTableTitleName(string ExcelName, string TableName, string Path)
    {
        List<string> TitleALL = new List<string>();
        // Debug.Log("Get Title");
        ExcelNameData = ExcelName;
        FileStream stream = File.Open(Path + "" + ExcelName, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        int Low;
        do
        {
            if (excelReader.Name == TableName)
            {
                while (excelReader.Read())
                {
                    Low = excelReader.FieldCount;
                    TitleALL.Clear();
                    TitleName.Clear();
                    for (int i = 0; i < Low; i++)
                    {
                        if (excelReader.GetString(i) != null)
                        {
                            if (!TitleName.ContainsKey(excelReader.GetString(i)))
                            {
                                TitleALL.Add(excelReader.GetString(i));
                                TitleName.Add(excelReader.GetString(i), i);
                            }
                        }
                    }
                    break;
                }
                break;
            }

        } while (excelReader.NextResult());

        stream.Close();
        return TitleALL;
    }
    public List<string> LoadExcleData(string TableName, string DataName, string Path)
    {
        List<string> DataAll = new List<string>();
        if (ExcelNameData != null)
        {
            // Debug.Log(DataName);
            if (TitleName.ContainsKey(DataName))
            {
                FileStream stream = File.Open(Path + "" + ExcelNameData, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                int DataNumber = TitleName[DataName];
                do
                {
                    if (excelReader.Name == TableName)
                    {

                        while (excelReader.Read())
                        {
                            if (excelReader.GetString(DataNumber) != null)
                            {
                                DataAll.Add(excelReader.GetString(DataNumber));
                            }
                            else
                            {
                                DataAll.Add("空");
                            }

                            //Debug.Log("2  " + excelReader.GetString(DataNumber));
                        }
                        break;
                    }
                } while (excelReader.NextResult());

                stream.Close();
            }
            else
            {
                Debug.Log("Not Find " + DataName);
            }

        }
        else
        {
            Debug.Log("TitleName Error");
        }
        //Debug.Log("End : "+DataName);
        return DataAll;
    }
}
