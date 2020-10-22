using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    private string m_Answer;
    public string Answer { get { return m_Answer; } set { m_Answer = value; } }
    private string m_Meaning;
    public string Meaning { get { return m_Meaning; } set { m_Meaning = value; } }
    private int m_Length;
    public int Length { get { return m_Length; } }

    public int m_IndexCount = new int();
    
    // 겹치는 단어(Word), 인덱스(int, int) 목록
    private List<List<string>> m_IndexInfo = new List<List<string>>();
    public List<List<string>> IndexInfo { get { return m_IndexInfo; } set { IndexInfo = value; } }
    //ex) 0              1                2
    //    Apple, (1, 4)  Banana, (0, 3)   Orange, (2, 4)
    //    Book, (4, 2)   Note, (1, 3)     Orange, (1, 3)

    // 인덱스의 열린값
    public bool[] m_bIsOpen;

    private string[,] n_IndexInfo;
    public string[,] nIndexInfo { get { return n_IndexInfo; } set { n_IndexInfo = value; } }
    
    // 생성자
    public Word(string _answ = "", string _mean = "")
    {
        this.Answer = _answ;
        this.Meaning = _mean;
        this.m_Length = _answ.Length;
        m_bIsOpen = new bool[m_Length];
        for (int i = 0; i < Length; i++)
        {
            var temp = new List<string>();
            this.m_IndexInfo.Add(temp);
            this.m_bIsOpen[i] = true;
        }
    }
    
    // 해당 단어의 알파벳의 위치 반환
    public int FindAlphabetIndex(string _char)
    {
        if (!Answer.Contains(_char))
            Debug.LogError("FindAlphabetIndexError : 잘못된 인덱스 값으로 알파벳을 찾으려고 했습니다.");

        for (int i = 0; i < this.Answer.Length; i++)
        {
            if (this.Answer[i].ToString().ToLower() == _char.ToLower())
                return i;
        }

        return -1;
    }

    // 해당 위치의 알파벳 반환
    public char ParsingAlphabet(int index)
    {
        if (index >= Answer.Length)
            Debug.LogError("ParsingError : 잘못된 인덱스 값으로 알파벳을 찾으려고 했습니다.");
        return Answer[index];
    }
}
