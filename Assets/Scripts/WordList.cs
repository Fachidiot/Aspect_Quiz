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
}
