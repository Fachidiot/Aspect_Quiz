using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMgr : MonoBehaviour
{
    private bool m_IsClicked;
    private bool m_IsAnswer;
    private string m_Answer = "";
    private string m_Input = "";
    private int m_Index = 0;
    private GameObject TempInput;

    public Image Correct;
    public Image Wrong;

    public void KeyInput(string _input)
    {
        if(m_IsClicked)
        {
            m_Input += string.Join("", _input);
            TempInput = GameObject.Find(m_Answer + "" + m_Index);
            TempInput.GetComponent<InputWord>().Input(m_Answer[m_Index].ToString().ToUpper());
            m_Index++;
        }
    }

    private void Update()
    {
        if (m_Answer.Length <= 0)
            return;

        if(m_Answer.Length == m_Input.Length)
        {
            if (m_Input.ToUpper() == m_Answer.ToUpper())
            {
                // 정답
                Correct.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                // 오답
                Correct.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    public void QuestionClick(string _answer)
    {
        RESET();
        m_IsClicked = true;
        m_Answer = _answer;
    }

    void RESET()
    {
        Correct.color = new Color(1f, 1f, 1f, 0f);
        Correct.color = new Color(1f, 1f, 1f, 0f);
    }
}
