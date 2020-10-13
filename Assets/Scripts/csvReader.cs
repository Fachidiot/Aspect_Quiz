using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class csvReader : MonoBehaviour
{
    public string str_Filename;

    private static string FileName;
    private const string m_Default_Filename = "Resource/WordList.csv";
    private static List<Dictionary<object, object>> m_ShareList = new List<Dictionary<object, object>>();
    public static List<Dictionary<object, object>> ShareList { get { return m_ShareList; } }

    void Awake()
    {
        m_ShareList = ReadData(str_Filename);
        PrintData();
    }

    public List<Dictionary<object, object>> ReadData(string str_name = m_Default_Filename)
    {
        List<Dictionary<object, object>> DataList = new List<Dictionary<object, object>>();
        StreamReader sr = new StreamReader(Application.dataPath + "/" + str_name);

        if (sr.Peek() == 0)
            Debug.LogError("File doesn't exist");
        //=========================================
        bool m_bIsEnd = false;
        bool m_bExample = true;
        //  DATA TempData = this.gameObject.AddComponent<DATA>();
        DATA TempData = new DATA();

        while (!m_bIsEnd)
        {
            string str_Data = sr.ReadLine();
            if (str_Data == null)
            {
                m_bIsEnd = true;
                break;
            }

            if (m_bExample)
            {
                var var_Example = str_Data.Split(',');
                TempData.Nickname = var_Example[0];
                TempData.Level = var_Example[1];
                TempData.Friend = var_Example[2];
                TempData.Score = var_Example[3];
                TempData.Recommend = var_Example[4];
                TempData.Banned = var_Example[5];
                TempData.Me = var_Example[6];
                m_bExample = false;
            }
            else
            {
                var var_Value = str_Data.Split(',');
                var temp = new Dictionary<object, object>();


                temp.Add(TempData.Nickname, var_Value[0]);
                temp.Add(TempData.Level, var_Value[1]);
                temp.Add(TempData.Friend, var_Value[2]);
                temp.Add(TempData.Score, var_Value[3]);
                temp.Add(TempData.Recommend, var_Value[4]);
                temp.Add(TempData.Banned, var_Value[5]);
                temp.Add(TempData.Me, var_Value[6]);
                DataList.Add(temp);
            }
        }

        return DataList;
    }

    void PrintData()
    {
        for (int i = 0; i < m_ShareList.Count; i++)
        {
            Debug.Log("name : " + m_ShareList[i]["Nickname"] + ", level : " + m_ShareList[i]["Level"] +
                ", friend : " + m_ShareList[i]["IsFriend"] + ", score : " + m_ShareList[i]["Score"] +
                ", recommend : " + m_ShareList[i]["Recommended"] + ", banned : " + m_ShareList[i]["Banned"]
                + ", me : " + m_ShareList[i]["IsMe"]);
        }
    }

    public List<Dictionary<object, object>> PushList(object index)
    {
        List<Dictionary<object, object>> TempList = new List<Dictionary<object, object>>();
        string checktrue = "TRUE";
        string checkscore = "0";

        if (index.ToString() == "All")
        {
            for (int i = 0; i < m_ShareList.Count; i++)
            {
                if (m_ShareList[i]["Banned"].ToString() != checktrue)
                {
                    TempList.Add(m_ShareList[i]);
                }
            }

            return TempList;
        }
        if (index.ToString() == "Addable")
        {
            for (int i = 0; i < m_ShareList.Count; i++)
            {
                if (m_ShareList[i]["IsFriend"].ToString() != checktrue && m_ShareList[i]["Banned"].ToString() != checktrue
                    && checktrue != m_ShareList[i]["IsMe"].ToString())
                {
                    TempList.Add(m_ShareList[i]);
                }
            }

            return TempList;
        }

        if (index.ToString() != "Score")
        {
            for (int i = 0; i < m_ShareList.Count; i++)
            {
                if (m_ShareList[i][index].ToString() == checktrue)
                {
                    TempList.Add(m_ShareList[i]);
                }
            }

            return TempList;
        }
        else
        {
            for (int i = 0; i < m_ShareList.Count; i++)
            {
                if (m_ShareList[i][index].ToString() != checkscore)
                {
                    TempList.Add(m_ShareList[i]);
                }
            }

            return TempList;
        }
    }

    public void FixData(string name, string level, string score)
    {
        for (int i = 0; i < m_ShareList.Count; i++)
        {
            if (m_ShareList[i]["Nickname"].ToString() == name &&
                m_ShareList[i]["Level"].ToString() == level &&
                m_ShareList[i]["Score"].ToString() == score)
            {
                if (m_ShareList[i]["IsFriend"].ToString() == "TRUE")
                    continue;
                m_ShareList[i]["IsFriend"] = "TRUE";
                m_ShareList[i]["Recommended"] = "FALSE";
                return;
            }
        }
    }
}
