using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class csvReader : MonoBehaviour
{
    public string str_Filename;

    private static string FileName;
    private const string m_Default_Filename = "Resource/WordList.csv";
    private static WordList m_WordList = new WordList();

    void Awake()
    {
        m_WordList = ReadData(str_Filename);
        PrintData();
    }

    public WordList ReadData(string str_name = m_Default_Filename)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + str_name);

        if (sr.Peek() == 0)
            Debug.LogError("File doesn't exist");
        //=========================================
        bool m_bIsEnd = false;
        bool m_bExample = true;

        WordList templist = new WordList();

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
                var var_Value = str_Data.Split();
                if (var_Value.Length < 2)
                    m_bExample = false;
                continue;
            }
            else
            {
                var var_Value = str_Data.Split(',');

                templist.Add(var_Value[0], var_Value[1]);
            }
        }

        if (templist.Count <= 0)
            Debug.LogError("Empty reading list");

        Debug.Log("Successfully reading data");
        return templist;
    }

    void PrintData()
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            Debug.Log(m_WordList.GetMeaning(i) + m_WordList.GetAnswer(i));
        }
    }

    public static WordList GetList()
    {
        if (m_WordList.Count <= 0)
        {
            Debug.LogError("[AccessException] : WordList is Empty");
            return null;
        }
        return m_WordList;
    }
}
