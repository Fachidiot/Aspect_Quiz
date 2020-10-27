using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text Score;
    public Text AssistantOn;
    public Text AssistantOff;

    private string m_Difficult;
    private bool m_bEasy;
    private bool m_bNormal;
    private bool m_bHard;
    private bool m_bAssistant = true;

    private GameObject InputManager;

    private void Start()
    {
        Easy();
        InputManager = GameObject.Find("InputMgr");
    }

    void Update()
    {
        if(InputManager.GetComponent<InputMgr>().GetScore(m_Difficult) != -1)
        {
            Score.text = "최고점수 : " + InputManager.GetComponent<InputMgr>().GetScore(m_Difficult).ToString();
        }
        else
        {
            Score.text = "최고점수 : null";
        }
    }

    public void OnAssist()
    {
        if(!m_bAssistant)
        {
            InputManager.GetComponent<InputMgr>().Assistant = true;
            AssistantOn.gameObject.SetActive(true);
            Debug.Log("Assistant On");
            m_bAssistant = true;
        }
        else
        {
            InputManager.GetComponent<InputMgr>().Assistant = false;
            AssistantOff.gameObject.SetActive(true);
            Debug.Log("Assistant Off");
            m_bAssistant = false;
        }
    }

    public void Easy()
    {
        m_Difficult = "Easy";
        m_bEasy = true;
        m_bNormal = false;
        m_bHard = false;
    }

    public void Normal()
    {
        m_Difficult = "Normal";
        m_bEasy = false;
        m_bNormal = true;
        m_bHard = false;
    }

    public void Hard()
    {
        m_Difficult = "Hard";
        m_bEasy = false;
        m_bNormal = false;
        m_bHard = true;
    }
}
