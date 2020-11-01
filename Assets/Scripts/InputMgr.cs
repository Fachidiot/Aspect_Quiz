using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMgr : MonoBehaviour
{
    [Header("Veiwer")]
    public GameObject RectScroll;
    [Header("Effect Image")]
    public Image Correct;
    public Image Wrong;
    [Header("Text UI")]
    public Text CurrentHint;
    public Text QuestionCount;
    public Text Ready;
    public Text Start;
    [Header("Timer")]
    public Text TimerText;
    public float MaxTime;
    [HideInInspector]
    public bool Assistant;

    private float m_CurrentTime;

    private string m_Difficult;
    private int m_Score = -1;
    public int GetScore(string difficult)
    {
        if(PlayerPrefs.HasKey(difficult))
        {
            return PlayerPrefs.GetInt(difficult);
        }
        else
        {
            return -1;
        }
    }

    private bool m_bEasy;
    private bool m_bNormal;
    private bool m_bHard;

    [HideInInspector]
    public bool m_Power;
    public void PowerOn()
    {
        m_Power = true;
    }

    private bool m_IsStart;
    private bool m_IsEnd;
    private bool m_IsClicked;
    private bool m_IsInput;
    private bool m_IsCheck;
    // 정답
    private Word m_Answer;
    // 사용자 입력
    private string m_Input = "";
    private static int m_InputIndex = 0;
    private int m_MaxCount = 10;
    private GameObject TempInput;
    private GameObject WordManager;
    private List<Word> m_WordList;
    private List<Word> m_DoneList;
    private GameObject[] m_ObjectList;

    private void Awake()
    {
        Assistant = true;
        m_IsEnd = false;
        m_IsStart = false;
        m_IsCheck = false;
        m_IsInput = false;
        m_IsClicked = false;
        m_WordList = new List<Word>();
        m_DoneList = new List<Word>();
    }

    GameObject CheckCross()
    {
        bool vertical;
        if (TempInput.tag == "Vertical")
        {
            vertical = true;
        }
        else
        {
            vertical = false;
        }
        var temp = CheckDouble(TempInput.transform.localPosition, vertical);

        return temp;
    }

    public void KeyInput(string _input)
    {
        if (m_IsClicked)
        {

            if (_input == "BK_SPACE")
            {
                if(m_InputIndex == 0)
                {
                    m_IsInput = false;
                    return;
                }

                TempInput = GameObject.Find(m_Answer.Answer + " " + (m_InputIndex - 1));
                var temp1 = CheckCross();

                if (temp1 != null)
                { // 겹치는 부분이 존재함
                    m_Input = m_Input.Remove(m_Input.Length - 1);
                    Debug.Log(_input);
                    TempInput.GetComponent<InputWord>().Wrong();
                    temp1.GetComponent<InputWord>().Wrong();
                    m_InputIndex--;
                    return;
                }

                m_Input = m_Input.Remove(m_Input.Length - 1);
                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Wrong();
                m_InputIndex--;
                return;
            }

            if(m_InputIndex >= m_Answer.Answer.Length)
            {
                return;
            }
            m_Input += string.Join("", _input);
            TempInput = GameObject.Find(m_Answer.Answer + " " + m_InputIndex);

            var temp = CheckCross();

            if (temp != null)
            { // 겹치는 부분이 존재함
                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Input(_input);
                temp.GetComponent<InputWord>().Input(_input);
                m_InputIndex++;
                m_IsInput = true;
                return;
            }

            Debug.Log(_input);
            TempInput.GetComponent<InputWord>().Input(_input);
            m_InputIndex++;
            m_IsInput = true;
            return;
        }
    }

    GameObject CheckDouble(Vector3 Pos, bool Vertical)
    {
        if (Vertical)
        {
            var temp = GameObject.FindGameObjectsWithTag("Horizontal");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }
        else
        {
            var temp = GameObject.FindGameObjectsWithTag("Vertical");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }

        return null;
    }

    IEnumerator DiffuseEffect()
    {
        if(Correct.IsActive())
        { // 정답 이펙트
            while (Correct.color.a > 0f)
            {
                Correct.color -= new Color(0, 0, 0, 0.02f * Time.deltaTime);
            }
            m_IsCheck = false;

            RESET();

            // 문제를 맞혔을때만 다음으로 넘어가거나 뒤로 돌아가게 해줘야함
            if(Assistant)
            {
                Next();
            }
        }
        else
        { // 오답 이펙트
            while (Wrong.color.a > 0f)
            {
                Wrong.color -= new Color(0, 0, 0, 0.02f * Time.deltaTime);
            }
            m_IsCheck = false;
            RESET();

            // 문제를 틀렸을때는 다시 해당 문제로 가게 해줘야함
            m_IsClicked = true;
            HighLight();
        }
        yield return null;
    }

    private void FixedUpdate()
    {
        if (m_Power)
        {
            // 시작했을때 한번만
            if (!m_IsStart)
            {
                WordManager = GameObject.Find("WordMgr");
            }
            if (WordManager.GetComponent<WordMgr>().IsMake)
            {
                // 시작했을때 한번만
                if (!m_IsStart)
                {
                    Ready.gameObject.SetActive(false);
                    Start.gameObject.SetActive(true);
                    m_IsStart = true;
                }
                else if (!m_IsEnd)
                {
                    Timer();
                }
                Start.color -= new Color(0, 0, 0, 1f) * Time.deltaTime;
            }
            else
            { // 게임 시작 전
                Ready.GetComponent<Outline>().effectDistance += new Vector2(150.0f, 0.0f) * Time.deltaTime;
            }

            if (m_IsCheck)
            {
                StartCoroutine(DiffuseEffect());
            }
        }
    }

    void Timer()
    {
        if(m_DoneList.Count >= m_MaxCount)
        { // 모든 정답을 맞춘 상황
            Debug.Log("게임 종료");
            m_Score = (int)(m_CurrentTime);
            SaveRecord();
            m_IsEnd = true;
            return;
        }
        if(m_CurrentTime >= MaxTime)
        { // 시간 종료
            Debug.Log("시간 종료");
            m_IsEnd = true;
            // 게임 시작 화면으로
            return;
        }
        m_CurrentTime += Time.deltaTime;
        TimerText.text = ((int)(MaxTime - m_CurrentTime)).ToString();
    }

    private void Update()
    {
        if (m_Power)
        {
            // 시작했을때 한번만
            if (!m_IsStart)
            {
                WordManager = GameObject.Find("WordMgr");
            }
            // 처음 한번만 단어 목록 생성
            if (m_WordList.Count <= 0 && WordManager.GetComponent<WordMgr>().IsMake)
            {
                var temp = WordManager.GetComponent<WordMgr>().GetList();
                for (int i = 0; i < temp.Length; i++)
                {
                    m_WordList.Add(temp[i]);
                }
                m_MaxCount = m_WordList.Count;
            }

            QuestionCount.text = (m_DoneList.Count + " / " + m_MaxCount);

            // 표를 눌렀을 때
            if (m_IsClicked)
            {
                if (m_IsInput && m_Input.Length == m_Answer.Answer.Length)
                {
                    if (m_Input == m_Answer.Answer)
                    {
                        // 정답
                        Correct.gameObject.SetActive(true);
                        m_DoneList.Add(m_Answer);
                        m_WordList.Remove(m_Answer);
                        m_IsCheck = true;
                    }
                    else
                    {
                        // 오답
                        Wrong.gameObject.SetActive(true);
                        m_IsCheck = true;
                        for (int i = 0; i < m_Answer.Answer.Length; i++)
                        {
                            TempInput = GameObject.Find(m_Answer.Answer + " " + i);
                            TempInput.GetComponent<InputWord>().Wrong();
                            var temp = CheckCross();
                            if (temp != null)
                            {
                                temp.GetComponent<InputWord>().Wrong();
                            }
                        }
                    }
                }
            }
        }
    }

    public void Next()
    {
        RESET();
        int index = FindWord(m_Answer);
        if(index < m_WordList.Count - 1)
        {
            m_Answer = m_WordList[index + 1];
            CurrentHint.text = m_Answer.Meaning;
        }
        HighLight();
        var temp = GameObject.Find(m_Answer.Answer + " 0");
        Vector3 Pos = new Vector3(-temp.transform.localPosition.y - 200, temp.transform.localPosition.x + 200, 0);
        RectScroll.transform.localPosition = Pos;
        m_IsClicked = true;
    }

    public void Prev()
    {
        RESET();
        int index = FindWord(m_Answer);
        if (index > 0)
        {
            m_Answer = m_WordList[index - 1];
            CurrentHint.text = m_Answer.Meaning;
        }
        HighLight();
        var temp = GameObject.Find(m_Answer.Answer + " 0");
        Vector3 Pos = new Vector3(-temp.transform.localPosition.y - 200, temp.transform.localPosition.x + 200, 0);
        RectScroll.transform.localPosition = Pos;
        m_IsClicked = true;
    }

    public void QuestionClick(string _answer)
    {
        RESET();
        for (int i = 0; i < m_DoneList.Count; i++)
        {
            if(m_DoneList[i].Answer == _answer)
            {
                return;
            }
        }
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer == _answer)
            { // 정답 정보 입력
                m_Answer = m_WordList[i];
            }
        }
        Debug.Log(m_Answer.Answer);
        HighLight();
        CurrentHint.text = m_Answer.Meaning;
        m_IsClicked = true;
    }

    void HighLight()
    { // 클릭시 하이라이팅 처리
        m_ObjectList = new GameObject[m_Answer.Length];
        for (int i = 0; i < m_Answer.Length; i++)
        {
            m_ObjectList[i] = GameObject.Find(m_Answer.Answer + " " + i);
            m_ObjectList[i].GetComponent<Outline>().enabled = true;
        }
    }

    void RESET()
    {
        if(m_ObjectList != null)
        {
            for (int i = 0; i < m_ObjectList.Length; i++)
            {
                m_ObjectList[i].GetComponent<Outline>().enabled = false;
            }
        }
        m_IsClicked = false;
        m_Input = "";
        m_InputIndex = 0;
        m_IsInput = false;
        Correct.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        m_IsCheck = false;
        Correct.color += new Color(0, 0, 0, 255);
        Wrong.color += new Color(0, 0, 0, 255);
    }

    int FindWord(Word word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if(m_WordList[i] == word)
            {
                return i;
            }
        }

        return -1;
    }

    public void SetEasy()
    {
        m_Difficult = "Easy";
        m_bEasy = true;
        m_bNormal= false;
        m_bHard = false;
        MaxTime = 360;
    }

    public void SetNormal()
    {
        m_Difficult = "Normal";
        m_bEasy = false;
        m_bNormal = true;
        m_bHard = false;
        MaxTime = 100;
    }

    public void SetHard()
    {
        m_Difficult = "Hard";
        m_bEasy = false;
        m_bNormal = false;
        m_bHard = true;
        MaxTime = 60;
    }

    void SaveRecord()
    {
        if(PlayerPrefs.HasKey(m_Difficult))
        {
            if(PlayerPrefs.GetInt(m_Difficult) > m_Score)
            {
                PlayerPrefs.SetInt(m_Difficult, m_Score);
            }
        }
        else
        {
            PlayerPrefs.SetInt(m_Difficult, m_Score);
        }
    }
}
