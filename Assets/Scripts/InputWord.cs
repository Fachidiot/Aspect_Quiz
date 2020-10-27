using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputWord : MonoBehaviour
{
    public Text Answer;

    [HideInInspector]
    public bool m_IsDouble;

    private GameObject Mgr;
    private string m_Answer;
    private int m_Index;

    void Start()
    {
        m_IsDouble = false;
        Answer.text = "";
        Mgr = GameObject.Find("InputMgr");


        var temp = this.gameObject.name.Split(' ');
        //Answer.text = this.gameObject.name[int.Parse(temp[1])].ToString();
        m_Answer = temp[0];
        m_Index = int.Parse(temp[1]);
    }

    public void ClickButton()
    {
        if (this.gameObject.transform.localPosition.z != 0)
        {
            this.gameObject.transform.localPosition += Vector3.forward;
        }

        if (m_IsDouble)
        {
            if (gameObject.tag == "Vertical")
            {
                var temp = GameObject.FindGameObjectsWithTag("Horizontal");
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].transform.localPosition == this.gameObject.transform.localPosition)
                    {
                        var obj = temp[i];
                        m_IsDouble = false;
                        obj.transform.localPosition += Vector3.back;
                        obj.GetComponent<InputWord>().m_IsDouble = false;
                        obj.GetComponent<InputWord>().ClickButton();
                        return;
                    }
                }
            }
            else
            {
                var temp = GameObject.FindGameObjectsWithTag("Vertical");
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].transform.localPosition == this.gameObject.transform.localPosition)
                    {
                        var obj = temp[i];
                        m_IsDouble = false;
                        obj.transform.localPosition += Vector3.back;
                        obj.GetComponent<InputWord>().m_IsDouble = false;
                        obj.GetComponent<InputWord>().ClickButton();
                        return;
                    }
                }
            }
        }
        Mgr.GetComponent<InputMgr>().QuestionClick(m_Answer);
        m_IsDouble = true;
    }

    public void Show()
    {
        Answer.text = m_Answer[m_Index].ToString();
    }

    public void Input(string _input)
    {
        Answer.text = _input;
    }

    public void Wrong()
    {
        Answer.text = "";
    }

    public void SetUp(string _word, int index)
    {
        m_Answer = _word;
        m_Index = index;
    }
}
