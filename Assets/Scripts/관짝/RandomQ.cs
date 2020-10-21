using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomQ : MonoBehaviour
{
    public int QuestionCount;
    public GameObject Prefab;

    private GameObject[,] m_Grid;
    private char[,] temp_Grid;
    private WordList m_WordList;

    void Start()
    {
        m_WordList = csvReader.GetList(QuestionCount);
        m_WordList.Disintegrate();
        temp_Grid = new char[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                temp_Grid[i, j] = '0';
            }
        }

        QRandom();

        m_Grid = new GameObject[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                m_Grid[i, j] = Instantiate(Prefab, gameObject.transform);
                m_Grid[i, j].transform.localPosition += new Vector3(((i - 5) * 130), ((j - 5) * 130), 0);
                //m_Grid[i, j].GetComponent<InputWord>().SetUp(temp_Grid[i, j], 0);
            }
        }
    }


    void QRandom()
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            // 겹치는 단어가 1개라도 있을때
            if(m_WordList.GetInfo(i).m_CrossWordList.Count > 0)
            {
                int index = Random.Range(0, m_WordList.GetInfo(i).m_CrossWordList.Count);

                MakeGrid(m_WordList.GetWord(index));
            }
        }
    }

    void MakeGrid(WordList.Word word)
    {

        for (int j = 0; j < 3; j++)
        {
            int x = Random.Range(0, 10);
            int y = Random.Range(0, 10);

            if (CheckVertical(x, y) > 0)
            {
                for (int i = 0; i < word.WordInfo.m_CrossWordList.Count; i++)
                {
                    if (CheckHorizontal(x, y) <= word.WordInfo.m_CrossWordList[i].Length)
                    {
                        SetVertical(x, y, word.ToString().ToUpper());
                        SetHorizontal(x, y, word.WordInfo.m_CrossWordList[i].ToString().ToUpper());
                        return;
                    }
                }
            }
        }

        MakeGrid(word);
    }

    void SetVertical(int x, int y, string _word)
    {
        for (int i = 0; i < _word.Length; i++)
        {
            temp_Grid[x + i, y] = _word[i];
        }

        m_WordList.UseIndex(m_WordList.FindWord(_word));
    }

    void SetHorizontal(int x, int y, string _word)
    {
        for (int i = 0; i < _word.Length; i++)
        {
            temp_Grid[x, y + i] = _word[i];
        }

        m_WordList.UseIndex(m_WordList.FindWord(_word));
    }

    int CheckHorizontal(int x, int y)
    {
        return CheckRight(x, y) + CheckLeft(x, y) - 1;
    }

    int CheckVertical(int x, int y)
    {
        return CheckUp(x, y) + CheckDown(x, y) - 1;
    }

    int CheckUp(int x, int y)
    {
        int count = 0;
        if (x > -1)
        {
            if (temp_Grid[x, y] == '0')
            {
                count++;
                count += CheckUp(x - 1, y);
            }
        }
        return count;
    }

    int CheckDown(int x, int y)
    {
        int count = 0;
        if (x < 10)
        {
            if (temp_Grid[x, y] == '0')
            {
                count++;
                count += CheckDown(x + 1, y);
            }
        }
        return count;
    }

    int CheckLeft(int x, int y)
    {
        int count = 0;
        if (y > -1)
        {
            if (temp_Grid[x, y] == '0')
            {
                count++;
                count += CheckLeft(x, y - 1);
            }
        }
        return count;
    }

    int CheckRight(int x, int y)
    {
        int count = 0;
        if (y < 10)
        {
            if (temp_Grid[x, y] == '0')
            {
                count++;
                count += CheckRight(x, y + 1);
            }
        }
        return count;
    }
}
