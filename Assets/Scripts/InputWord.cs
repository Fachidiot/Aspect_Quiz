using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputWord : MonoBehaviour
{
    public Text Answer;
    
    private string m_Answer;

    void Start()
    {
        Answer.text = "";
    }

    public void Show()
    {
        Answer.text = m_Answer;
    }

    public void SetUp(string _answer)
    {
        m_Answer = _answer;
    }
}
