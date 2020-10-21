using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputWord : MonoBehaviour
{
    public Text Answer;

    private GameObject Mgr;
    private string m_Answer;
    private int m_Index;

    void Start()
    {
        Answer.text = "";
        Mgr = GameObject.Find("InputMgr");
    }

    public void ClickButton()
    {
        Mgr.GetComponent<InputMgr>().QuestionClick(m_Answer);
    }

    public void Show()
    {
        Answer.text = m_Answer[m_Index].ToString();
    }

    public void Input(string _input)
    {
        Answer.text = _input;
    }

    public void SetUp(string _word, int index)
    {
        m_Answer = _word;
        m_Index = index;
    }
}
