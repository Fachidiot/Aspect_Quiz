using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordList
{
    private List<Word> m_WordList = new List<Word>();

    private class Word
    {
        private string m_Answer;
        public string Answer { get { return m_Answer; } set { m_Answer = value; } }
        private string m_Meaning;
        public string Meaning { get { return m_Meaning; } set { m_Meaning = value; } }
        private int m_Length;

        public Word(string _answ, string _mean)
        {
            this.Answer = _answ;
            this.Meaning = _mean;
        }

        public void Print()
        {
            Debug.Log(Answer);
            Debug.Log(Meaning);
        }
    }

    List<int> list = new List<int>();

    public void Add(string _str1, string _str2)
    {
        Word temp = new Word(_str1, _str2);
        m_WordList.Add(temp);
    }

    public string GetAnswer(int index)
    {
        Debug.Log("Answer lookup");
        return m_WordList[index].Answer;
    }

    public string GetMeaning(int index)
    {
        Debug.Log("Meaning lookup");
        return m_WordList[index].Meaning;
    }

    public int Count()
    {
        return m_WordList.Count;
    }

    public void DontUse(int index)
    {
        m_WordList.RemoveAt(index);
        Debug.Log(m_WordList[index].Answer + " 해당 단어의 길이가 너무 길어 제외됩니다.");
    }
}
