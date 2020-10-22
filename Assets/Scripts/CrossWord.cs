using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossWord : MonoBehaviour
{
    private static List<Word> m_WordList;

    private int[] m_UsedIndex;
    
    void Start()
    {
        SetList();
        for (int i = 0; i < m_WordList.Count; i++)
        {
            CheckCross(i);
        }
        //print();
    }

    // 체크용
    void print()
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            Debug.Log(m_WordList[i].Answer);
            for (int j = 0; j < m_WordList[i].IndexInfo.Count; j++)
            {
                for (int k = 0; k < m_WordList[i].IndexInfo[j].Count; k++)
                {
                    Debug.Log(m_WordList[i].IndexInfo[j][k]);
                }
            }
        }
    }

    // 단어장 가져오기
    void SetList()
    {
        m_WordList = csvReader_ver2.GetList();
        if (m_WordList.Count <= 0)
        {
            Debug.Log("No Dictionary");
            return;
        }
        m_UsedIndex = new int[m_WordList.Count];
    }

    // 교차단어를 체크
    void CheckCross(int index)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            // 자기 자신을 제외
            if (i == index)
                continue;

            // 해당 단어와 교차하는 단어를 저장
            for (int j = 0; j < m_WordList[i].Length; j++)
            {
                if (m_WordList[index].Answer.Contains(m_WordList[i].Answer[j].ToString()))
                { // 해당 단어와 교차되는 단어일때
                    var temp = m_WordList[index].FindAlphabetIndex(m_WordList[i].Answer[j].ToString());
                    // 해당 리스트인덱스에 string리스트를 추가해준다.
                    // Word, 1, 2, false
                    var temp_string = m_WordList[i].Answer + ", " + temp + ", " + j;
                    m_WordList[index].IndexInfo[temp].Add(temp_string);
                }
            }
        }
    }

    public List<Word> GetList()
    {
        var temp = m_WordList.ToList();
        return temp;
    }
}
