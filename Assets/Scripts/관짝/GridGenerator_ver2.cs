
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator_ver2 : MonoBehaviour
{
    [Header("Grid Size")]
    public int GridSize;
    [Tooltip("GridObject")]
    public GameObject GridPrefab;
    [Header("WordCount")]
    public Text WordCount;

    private GameObject[,] m_Grid;
    private string[,] temp_Grid;
    private WordList m_WordList;
    private int m_Question = 0;

    private WordList.Word m_TempWord = null;
    private Vector2 m_TempWordLoc;

    void Start()
    {
        m_WordList = csvReader.GetList(GridSize);
        CheckList();
        WordCount.text = m_WordList.Count.ToString();
        TempGenerate(GridSize);
        DoRandom();
        print();
        GridGenerate(GridSize);
    }

    void CheckList()
    {
        Debug.Log("리스트 체크");
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList.GetAnswer(i).Length > this.GridSize)
                m_WordList.DontUse(i);

            else
                m_Question++;
        }
    }

    void print()
    {
        for (int i = 0; i < GridSize; i++)
        {
            switch (GridSize)
            {
                case 8:
                    Debug.Log(temp_Grid[i, 0] + "     " + temp_Grid[i, 1] + "     " + temp_Grid[i, 2] + "     " + temp_Grid[i, 3] + "     " + temp_Grid[i, 4] + "     " +
                        temp_Grid[i, 5] + "     " + temp_Grid[i, 6] + "     " + temp_Grid[i, 7]);
                    break;
                case 11:
                    Debug.Log(temp_Grid[i, 0] + "     " + temp_Grid[i, 1] + "     " + temp_Grid[i, 2] + "     " + temp_Grid[i, 3] + "     " + temp_Grid[i, 4] + "     " +
                        temp_Grid[i, 5] + "     " + temp_Grid[i, 6] + "     " + temp_Grid[i, 7] + "     " + temp_Grid[i, 8] + "     " + temp_Grid[i, 9] + "     " + temp_Grid[i, 10]);
                    break;
                case 13:
                    Debug.Log(temp_Grid[i, 0] + "     " + temp_Grid[i, 1] + "     " + temp_Grid[i, 2] + "     " + temp_Grid[i, 3] + "     " + temp_Grid[i, 4] + "     " +
                        temp_Grid[i, 5] + "     " + temp_Grid[i, 6] + "     " + temp_Grid[i, 7] + "     " + temp_Grid[i, 8] + "     " + temp_Grid[i, 9] + "     " +
                        temp_Grid[i, 10] + "     " + temp_Grid[i, 11] + "     " + temp_Grid[i, 12]);
                    break;
            }
        }
        Debug.Log("==============================================");
    }

    void Update()
    {

    }

    // Generators
    public void TempGenerate(int _wordCount)
    {
        temp_Grid = new string[_wordCount, _wordCount];
        for (int i = 0; i < this.GridSize; i++)
        {
            for (int j = 0; j < this.GridSize; j++)
            {
                temp_Grid[i, j] = "-";
            }
        }
    }
    // GridGenerator
    public void GridGenerate(int _wordCount)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 200, GridSize * 200);
        m_Grid = new GameObject[_wordCount, _wordCount];
        for (int i = 0; i < this.GridSize; i++)
        {
            for (int j = 0; j < this.GridSize; j++)
            {
                if (temp_Grid[i, j] == "-")
                    continue;
                this.m_Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
                this.m_Grid[i, j].transform.localPosition += new Vector3(((i - (GridSize - 1) / 2) * 130), ((j - (GridSize - 2) / 2) * 130), 0);
                //this.m_Grid[i, j].GetComponent<InputWord>().SetUp(temp_Grid[i, j], 0);
            }
        }
    }

    void GridDelete()
    {
        if (m_Grid != null)
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Destroy(m_Grid[i, j]);
                }
            }
        }

        m_TempWord = null;
    }

    // Reset
    public void RESET()
    {
        Debug.Log("==========================RESET========================");
        GridDelete();
        m_WordList = csvReader.GetList(GridSize);
        CheckList();
        WordCount.text = m_WordList.Count.ToString();
        TempGenerate(GridSize);
        try
        {
            DoRandom();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        print();
        GridGenerate(GridSize);
    }






    // Grid Access
    void SetGrid(int x, int y, char _word)
    {
        if (x >= GridSize || y >= GridSize)
        {
            Debug.LogError("잘못된 인덱스");
            return;
        }
        temp_Grid[x, y] = _word.ToString().ToLower();
    }

    Vector2 GetGrid(string name, string _char)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (name.ToLower().Contains(temp_Grid[i, j]))
                {
                    if (_char == temp_Grid[i, j])
                    {
                        return new Vector2(i, j);
                    }
                }
            }
        }

        return new Vector2(-1, -1);
    }








    // Random Setting
    void DoRandom()
    {
        while (true)
        {
            // Vertical
            if (0 == m_WordList.Count)
                break;

            if (m_TempWord != null)
            {
                if (!SetCrossVertical())
                {
                    m_TempWord = null;
                    VerticalRule();
                }
            }
            else
            {
                if (!VerticalCrossRule())
                    VerticalRule();
            }


            // Horizontal
            if (0 == m_WordList.Count)
                break;

            if (m_TempWord != null)
            {
                if (!SetCrossHorizontal())
                {
                    m_TempWord = null;
                    HorizontalRule();
                }
            }
            else
            {
                if (!HorizontalCrossRule())
                    HorizontalRule();
            }
        }
    }



    // VERTICAL
    void VerticalRule()
    {
        int random_index = Random.Range(0, m_WordList.Count);
        WordList.Word Temp_word = m_WordList.GetWord(random_index);
        Debug.Log(Temp_word.Answer + " Vertical 단어를 생성합니다.");

        int x = 0;
        int y = 0;
        bool m_Random = false;
        try
        {
            while (!m_Random)
            {
                Rand(ref x, ref y);

                // Rule
                // x >= 0, Length + x < GridSize
                if (x >= 0 && Temp_word.Answer.Length + x < GridSize)
                {
                    if (CheckVertical(x, y, random_index))
                    {
                        m_Random = true;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        SetVertical(x, y, random_index);
    }


    bool VerticalCrossRule()
    {
        int random_index = Random.Range(0, m_WordList.Count);
        WordList.Word Temp_word = m_WordList.GetWord(random_index);
        Debug.Log(Temp_word.Answer + " Vertical Cross 단어를 생성합니다.");

        // 겹치는 단어가 존재함.
        if (Temp_word.WordInfo.m_CrossWordList.Count > 0)
        {
            int x = 0;
            int y = 0;
            bool m_Random = false;
            int index = 0;

            int cross_Index = 0;
            WordList.Word cross_word = new WordList.Word();
            string cross_char = "";

            try
            {
                while (true)
                {
                    if (Temp_word.WordInfo.m_CrossWordList.Count > index)
                    {
                        cross_word = Temp_word.WordInfo.m_CrossWordList[index];
                        cross_char = Temp_word.WordInfo.m_CrossChar[index];
                        cross_Index = cross_word.FindChar(cross_char);
                        index++;
                    }
                    else
                    {
                        // 10번 검색 실패시
                        Debug.Log("Cross 단어 생성 실패");
                        return false;
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        if (m_Random)
                            break;
                        // 겹치는 단어가 아직 생성되지 X
                        if (m_WordList.SearchWord(cross_word.Answer))
                        {
                            // 겹치는 단어를 저장하고 겹치는 단어의 생성 위치를 고려해준다.

                            Rand(ref x, ref y);

                            // Rule
                            // x - crossword의 앞글자 >= 0, crossword의 뒷글자 + x < GridSize
                            if (y - cross_Index >= 0 && cross_word.Length - cross_Index + y < GridSize
                                && x >= 0 && Temp_word.Answer.Length + x < GridSize)
                            {
                                if (CheckVertical(x, y, random_index))
                                {
                                    Debug.Log("성공");

                                    m_TempWord = cross_word;
                                    int temp = Temp_word.FindChar(cross_char);
                                    m_TempWordLoc = new Vector2(x + temp, y - cross_Index);
                                    SetVertical(x, y, random_index);
                                    print();
                                    return true;
                                }

                                Debug.Log("겹치는 단어 검출!");
                            }
                            else
                            {
                                Debug.Log("notyet cross 실패");
                            }
                        }

                        // 겹치는 단어가 이미 생성되었을때
                        else
                        {
                            // 겹치는 단어의 위치를 검색해서 해당 위치에 생성할 수 있을때만 생성
                            Vector2 temp_index = GetGrid(cross_word.Answer, cross_char);
                            // temp_index = 겹치는 위치
                            if (temp_index != new Vector2(-1, -1))
                            {
                                x = (int)temp_index.x;
                                y = (int)temp_index.y;

                                // Rule
                                // y - crossword의 앞글자 >= 0, crossword의 뒷글자 + y < GridSize
                                if (y - cross_Index >= 0 && cross_word.Length - cross_Index + y < GridSize)
                                {
                                    if (CheckVertical(x, y, random_index))
                                    {
                                        Debug.Log("성공");

                                        SetVertical(x, y, random_index);
                                        m_TempWord = null;
                                        print();
                                        return true;
                                    }
                                }
                                else
                                {
                                    Debug.Log("already cross 실패");
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        return false;
    }

    void SetVertical(int x, int y, int index)
    {
        for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
        {
            SetGrid(x + i, y, m_WordList.GetAnswer(index)[i]);
        }

        m_WordList.UseIndex(index);

        print();
    }

    bool CheckVertical(int x, int y, int index)
    {
        for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
        {
            if (temp_Grid[x + i, y] != "-")
            {
                if (m_WordList.GetAnswer(index)[i].ToString().ToLower() == (temp_Grid[x + i, y]))
                    continue;
                else
                    return false;
            }

        }

        return true;
    }

    bool SetCrossVertical()
    {
        if (CheckVertical((int)m_TempWordLoc.x, (int)m_TempWordLoc.y, m_WordList.FindWord(m_TempWord.Answer)))
        {
            Debug.Log(m_TempWord.Answer + " Vertical Crossed 단어를 생성합니다.");
            for (int i = 0; i < m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)).Length; i++)
            {
                SetGrid((int)m_TempWordLoc.x + i, (int)m_TempWordLoc.y, m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer))[i]);
            }

            m_WordList.UseIndex(m_WordList.FindWord(m_TempWord.Answer));
            m_TempWord = null;
            print();
            return true;
        }

        else
            return false;
    }





    // HORIZONTAL
    void HorizontalRule()
    {
        int random_index = Random.Range(0, m_WordList.Count);
        WordList.Word Temp_word = m_WordList.GetWord(random_index);
        Debug.Log(Temp_word.Answer + " Horizontal 단어를 생성합니다.");

        int x = 0;
        int y = 0;
        bool m_Random = false;
        try
        {
            while (!m_Random)
            {
                Rand(ref x, ref y);

                // Rule
                // y >= 0, Length + y < GridSize
                if (y >= 0 && Temp_word.Answer.Length + y < GridSize)
                {
                    if (CheckHorizontal(x, y, random_index))
                        m_Random = true;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        SetHorizontal(x, y, random_index);
    }


    bool HorizontalCrossRule()
    {
        int random_index = Random.Range(0, m_WordList.Count);
        WordList.Word Temp_word = m_WordList.GetWord(random_index);
        Debug.Log(Temp_word.Answer + " Horizontal Cross 단어를 생성합니다.");

        // 겹치는 단어가 존재함.
        if (Temp_word.WordInfo.m_CrossWordList.Count > 0)
        {
            int x = 0;
            int y = 0;
            bool m_Random = false;
            int index = 0;

            int cross_Index = 0;
            WordList.Word cross_word = new WordList.Word();
            string cross_char = "";

            try
            {
                while (!m_Random)
                {
                    if (Temp_word.WordInfo.m_CrossWordList.Count > index)
                    {
                        cross_word = Temp_word.WordInfo.m_CrossWordList[index];
                        cross_char = Temp_word.WordInfo.m_CrossChar[index];
                        cross_Index = cross_word.FindChar(cross_char);
                        index++;
                    }
                    else
                    {
                        // 10번 검색 실패시
                        Debug.Log("Cross 단어 생성 실패");
                        return false;
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        if (m_Random)
                            break;
                        // 겹치는 단어가 아직 생성되지 X
                        if (m_WordList.SearchWord(cross_word.Answer))
                        {
                            // 겹치는 단어를 저장하고 겹치는 단어의 생성 위치를 고려해준다.

                            Rand(ref x, ref y);

                            // Rule
                            // y - crossword의 앞글자 >= 0, crossword의 뒷글자 + y < GridSize
                            if (x - cross_Index >= 0 && cross_word.Length - cross_Index + x < GridSize
                                && y >= 0 && Temp_word.Answer.Length + y < GridSize)
                            {
                                if (CheckHorizontal(x, y, random_index))
                                {
                                    Debug.Log("성공");

                                    m_TempWord = cross_word;
                                    int temp = Temp_word.FindChar(cross_char);
                                    m_TempWordLoc = new Vector2(x - temp, y);
                                    SetHorizontal(x, y, random_index);
                                    print();
                                    return true;
                                }

                                Debug.Log("겹치는 단어 검출!");
                            }
                            else
                            {
                                Debug.Log("notyet cross 실패");
                            }
                        }

                        // 겹치는 단어가 이미 생성되었을때
                        else
                        {
                            // 겹치는 단어의 위치를 검색해서 해당 위치에 생성할 수 있을때만 생성
                            Vector2 temp_index = GetGrid(cross_word.Answer, cross_char);
                            // temp_index = 겹치는 위치
                            if (temp_index != new Vector2(-1, -1))
                            {
                                x = (int)temp_index.x;
                                y = (int)temp_index.y;

                                // Rule
                                // y - crossword의 앞글자 >= 0, crossword의 뒷글자 + y < GridSize
                                if (x - cross_Index >= 0 && cross_word.Length - cross_Index + x < GridSize)
                                {
                                    if (CheckHorizontal(x, y, random_index))
                                    {
                                        Debug.Log("성공");

                                        SetVertical(x, y, random_index);
                                        m_TempWord = null;
                                        print();
                                        return true;
                                    }
                                }
                                else
                                {
                                    Debug.Log("already cross 실패");
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        return false;
    }

    void SetHorizontal(int x, int y, int index)
    {
        for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
        {
            SetGrid(x, y + i, m_WordList.GetAnswer(index)[i]);
        }

        m_WordList.UseIndex(index);

        print();
    }

    bool CheckHorizontal(int x, int y, int index)
    {
        for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
        {
            if (temp_Grid[x, y + i] != "-")
                if (m_WordList.GetAnswer(index)[i].ToString().ToLower() == (temp_Grid[x, y + i]))
                    continue;
                else
                    return false;
        }

        return true;
    }

    bool SetCrossHorizontal()
    {
        if (CheckHorizontal((int)m_TempWordLoc.x, (int)m_TempWordLoc.y, m_WordList.FindWord(m_TempWord.Answer)))
        {
            Debug.Log(m_TempWord.Answer + " Crossed 단어를 생성합니다.");
            for (int i = 0; i < m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)).Length; i++)
            {
                SetGrid((int)m_TempWordLoc.x, (int)m_TempWordLoc.y + i, m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer))[i]);
            }

            m_WordList.UseIndex(m_WordList.FindWord(m_TempWord.Answer));
            m_TempWord = null;
            print();
            return true;
        }

        else
            return false;
    }










    // Random XY
    void Rand(ref int x, ref int y)
    {
        x = Random.Range(0, GridSize - 1);
        y = Random.Range(0, GridSize - 1);
    }
}
