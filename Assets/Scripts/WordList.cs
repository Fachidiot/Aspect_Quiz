using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordList
{
    private List<Word> m_WordList = new List<Word>();
    public int Count { get { return m_WordList.Count; } }


    // Never use
    public void FindDelete(string name)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            for (int j = 0; j < m_WordList[i].WordInfo.m_CrossWordList.Count; j++)
            {
                if (m_WordList[i].WordInfo.m_CrossWordList[j].Answer == name)
                    m_WordList[i].UseCross(j);
            }
        }
    }

    public void UseIndex(int index)
    {
        m_WordList.RemoveAt(index);
    }

    public Word.Inspector GetInfo(int index)
    {
        return m_WordList[index].WordInfo;
    }

    // 해당 단어의 인덱스 검색
    public int FindWord(string name)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.ToLower() == name.ToLower())
                return i;
        }

        return -1;
    }

    public bool SearchWord(string name)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.ToLower() == name.ToLower())
                return true;
        }

        return false;
    }

    public class Word
    {
        private string m_Answer;
        public string Answer { get { return m_Answer; } set { m_Answer = value; } }
        private string m_Meaning;
        public string Meaning { get { return m_Meaning; } set { m_Meaning = value; } }
        private int m_Length;
        public int Length { get { return m_Length; } }

        public Inspector WordInfo = new Inspector();

        public void UseCross(int index)
        {
            WordInfo.m_CrossChar.RemoveAt(index);
            WordInfo.m_CrossIndex.RemoveAt(index);
            WordInfo.m_CrossWordList.RemoveAt(index);
        }

        public Inspector GetInfo()
        {
            return WordInfo;
        }

        // 해당 단어의 알파벳의 위치 반환
        public int FindChar(string _char)
        {
            for (int i = 0; i < this.Answer.Length; i++)
            {
                if (this.Answer[i].ToString().ToLower() == _char.ToLower())
                    return i;
            }

            return -1;
        }

        public class Inspector
        {
            // Inspector
            public List<int> m_CrossIndex = new List<int>();
            public List<Word> m_CrossWordList = new List<Word>();
            public List<string> m_CrossChar = new List<string>();

            public void Cross(Word name, int index, string word)
            {
                m_CrossIndex.Add(index);
                m_CrossWordList.Add(name);
                m_CrossChar.Add(word);
            }
        }

        public Word(string _answ = "", string _mean = "")
        {
            this.Answer = _answ;
            this.Meaning = _mean;
            this.m_Length = _answ.Length;
        }

        public char Parsing(int index)
        {
            return Answer[index];
        }
    }

    public void Add(string _str1, string _str2)
    {
        Word temp = new Word(_str1, _str2);
        m_WordList.Add(temp);
    }

    public void Add(Word.Inspector _info, string _str1, string _str2)
    {
        Word temp = new Word(_str1, _str2);
        temp.WordInfo = _info;
        m_WordList.Add(temp);
    }

    public Word GetWord(int index)
    {
        return m_WordList[index];
    }

    public string GetAnswer(int index)
    {
        return m_WordList[index].Answer;
    }

    public string GetMeaning(int index)
    {
        return m_WordList[index].Meaning;
    }

    public void DontUse(int index)
    {
        m_WordList.RemoveAt(index);
        Debug.Log(" 해당 단어의 길이가 너무 길어 제외됩니다.");
    }

    public void Disintegrate()
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            for (int j = 0; j < m_WordList.Count; j++)
            {
                IsCross(i, j);
            }
        }
    }

    bool IsCross(int _index1, int _index2)
    {
        if (_index1 == _index2)
            return false;
        for (int i = 0; i < m_WordList[_index1].Length; i++)
        {
            for (int j = 0; j < m_WordList[_index2].Length; j++)
            {
                if (m_WordList[_index1].Parsing(i).ToString().ToLower() == m_WordList[_index2].Parsing(j).ToString().ToLower())
                {
                    m_WordList[_index1].WordInfo.Cross(m_WordList[_index2], i, m_WordList[_index1].Parsing(i).ToString().ToLower());
                    return true;
                }
            }
        }

        return false;
    }
}
