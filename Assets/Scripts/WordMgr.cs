using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordMgr : MonoBehaviour
{
    [Header("GridMake StartPosition")]
    public float StartX;
    public float StartY;
    [Header("Prefab")]
    public GameObject GridPrefab;

    private List<Word> m_WordList;
    private Word m_TempWord;
    private bool m_bIsMake;

    private void Awake()
    {
        m_bIsMake = false;
    }

    private void Update()
    {
        if(!m_bIsMake)
            LinkLogic();
    }

    void LinkLogic()
    {
        m_WordList = GameObject.Find("CrossMgr").GetComponent<CrossWord>().GetList();
        RootSet();
        //for (int i = 0; i >= m_WordList.Count; i--)
        //{
        //    RootSet();
        //    RandomMake();
        //}
        m_bIsMake = true;
    }

    void UseWord(int index)
    {
        m_WordList.RemoveAt(index);
    }

    // 루트가 될 단어를 뽑고 생성해주기
    void RootSet()
    {
        // 루트가 될 단어를 랜덤으로 뽑는다.
        var index = Random.Range(0, m_WordList.Count);

        // 랜덤으로 뽑은 인덱스(알파벳)와 인덱스의 인덱스(알파벳과 교차하는 단어 리스트)
        int rand_alphabetindex;
        int rand_wordindex;

        m_TempWord = m_WordList[index];
        int safecount = 0;
        while (true)
        {
            var temp = Random.Range(0, m_TempWord.IndexInfo.Count);
            
            // 해당 리스트에 교차되는 단어가 없을때
            if (m_TempWord.IndexInfo[temp].Count <= 0)
            {
                // 만약 전체 인덱스에 교차되는 단어가 1개도 없을때 무한루프에 빠지지 않도록 해준다.
                if (m_TempWord.IndexInfo.Count <= safecount)
                {
                    Debug.Log("교차단어가 없는 단어 발견!");
                    UseWord(index);
                    return;
                }
                safecount++;
                continue;
            }

            // 교차 단어가 있을때
            rand_alphabetindex = temp;
            break;
        }

        // 랜덤으로 단어를 갖고 온다.
        rand_wordindex = Random.Range(0, m_TempWord.IndexInfo[rand_alphabetindex].Count);

        string var = m_TempWord.IndexInfo[rand_alphabetindex][rand_wordindex];
        // 0 : Word, 1 : index1, 2 : index2, 3 : bool
        var temp_string = var.Split(',');
        float _1 = float.Parse(temp_string[1]);
        float _2 = float.Parse(temp_string[2]);

        // 단어를 생성해준다.
        for (int i = 0; i < m_TempWord.Length; i++)
        {
            var temp = Instantiate(GridPrefab, this.gameObject.transform);
            temp.transform.localPosition += new Vector3(StartX + 110 * i, StartY, 0);
            temp.name = m_TempWord.Answer + i;
        }

        // 생성후에는 다음에 만들 단어를 저장해준다.
        m_TempWord = FindWordIndex(temp_string[0]);
        // 다음 생성될 단어의 위치도 저장해준다.
        StartX = StartX + _1;
        StartY = StartY - _2;
    }

    // 루트 단어에 맞춰 생성해주기
    void RandomMake()
    {
    }

    void Resetting()
    {
    }

    Word FindWordIndex(string _word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.Contains(_word))
                return m_WordList[i];
        }

        return null;
    }
}
