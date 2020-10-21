using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [Header("Grid Size")]
    public int GridSize;
    [Tooltip("GridObject")]
    public GameObject GridPrefab;
    [Header("WordCount")]
    public Text WordCount;

    private GameObject[,] m_Grid;
    private string[,] temp_Grid;
    private int[,] temp_GridIndex;
    private WordList m_WordList;
    private int m_Question = 0;

    private WordList.Word m_TempWord = null;
    private Vector2 m_TempWordLoc;

    void Start()
    {
        temp_Grid = new string[GridSize, GridSize];
        temp_GridIndex = new int[GridSize, GridSize];

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
            Debug.Log(ex.Message);
        }
        print();
        GridGenerate(GridSize);
    }

    void CheckList()
    {
        Debug.Log("리스트 체크");
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList.GetAnswer(i).Length > this.GridSize - 1)
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
                    Debug.Log(temp_Grid[i, 0][temp_GridIndex[i,0]] + "     " + temp_Grid[i, 1][temp_GridIndex[i, 1]] + "     " + temp_Grid[i, 2][temp_GridIndex[i, 2]]
                        + "     " + temp_Grid[i, 3][temp_GridIndex[i, 3]] + "     " + temp_Grid[i, 4][temp_GridIndex[i, 4]] + "     " +
                        temp_Grid[i, 5][temp_GridIndex[i, 5]] + "     " + temp_Grid[i, 6][temp_GridIndex[i, 6]] + "     " + temp_Grid[i, 7][temp_GridIndex[i, 7]]);
                    break;
                case 11:
                    Debug.Log(temp_Grid[i, 0][temp_GridIndex[i, 0]] + "     " + temp_Grid[i, 1][temp_GridIndex[i, 1]] + "     " + temp_Grid[i, 2][temp_GridIndex[i, 2]]
                        + "     " + temp_Grid[i, 3][temp_GridIndex[i, 3]] + "     " + temp_Grid[i, 4][temp_GridIndex[i, 4]] + "     " +
                        temp_Grid[i, 5][temp_GridIndex[i, 5]] + "     " + temp_Grid[i, 6][temp_GridIndex[i, 6]] + "     " + temp_Grid[i, 7][temp_GridIndex[i, 7]]
                        + "     " + temp_Grid[i, 8][temp_GridIndex[i, 8]] + "     " + temp_Grid[i, 9][temp_GridIndex[i, 9]] + "     " + temp_Grid[i, 10][temp_GridIndex[i, 10]]);
                    break;
                case 13:
                    Debug.Log(temp_Grid[i, 0][temp_GridIndex[i, 0]] + "     " + temp_Grid[i, 1][temp_GridIndex[i, 1]] + "     " + temp_Grid[i, 2][temp_GridIndex[i, 2]]
                        + "     " + temp_Grid[i, 3][temp_GridIndex[i, 3]] + "     " + temp_Grid[i, 4][temp_GridIndex[i, 4]] + "     " +
                        temp_Grid[i, 5][temp_GridIndex[i, 5]] + "     " + temp_Grid[i, 6][temp_GridIndex[i, 6]] + "     " + temp_Grid[i, 7][temp_GridIndex[i, 7]]
                        + "     " + temp_Grid[i, 8][temp_GridIndex[i, 8]] + "     " + temp_Grid[i, 9][temp_GridIndex[i, 9]] + "     " + temp_Grid[i, 10][temp_GridIndex[i, 10]]
                        + "     " + temp_Grid[i, 11][temp_GridIndex[i, 11]] + "     " + temp_Grid[i, 12][temp_GridIndex[i, 12]]);
                    break;
            }
        }
        Debug.Log("==============================================");
    }

    // Generators
    public void TempGenerate(int _wordCount)
    {
        for (int i = 0; i < this.GridSize; i++)
        {
            for (int j = 0; j < this.GridSize; j++)
            {
                temp_Grid[i, j] = "--";
                temp_GridIndex[i, j] = 0;
            }
        }
    }
    // GridGenerator
    public void GridGenerate(int _wordCount)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 250, GridSize * 250);
        m_Grid = new GameObject[_wordCount, _wordCount];
        for (int i = 0; i < this.GridSize; i++)
        {
            for (int j = 0; j < this.GridSize; j++)
            {
                if (temp_Grid[i, j] == "--")
                    continue;
                this.m_Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
                this.m_Grid[i, j].transform.localPosition += new Vector3(((i - (GridSize - 1) / 2) * 130), ((j - (GridSize - 2) / 2) * 130), 0);
                this.m_Grid[i,j].name = (temp_Grid[i, j] + "" + temp_GridIndex[i, j]);
                this.m_Grid[i, j].GetComponent<InputWord>().SetUp(temp_Grid[i, j], temp_GridIndex[i, j]);
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
    void SetGrid(int x, int y, string _word, int index)
    {
        if (x >= GridSize || y >= GridSize)
        {
            Debug.LogError("잘못된 인덱스");
            return;
        }
        temp_Grid[x, y] = _word.ToUpper();
        temp_GridIndex[x, y] = index;
    }

    Vector2 GetGrid(string name)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (name.ToUpper().Contains(temp_Grid[i, j]))
                {
                    if (name.ToUpper() == temp_Grid[i, j])
                    {
                        return new Vector2(i, j);
                    }
                }
            }
        }

        return new Vector2(-1, -1);
    }

    void CheckGrid()
    {
        string temp = "";
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (temp_Grid[i, j] != "--")
                {
                    if(temp == "")
                        temp = temp_Grid[i, j];
                    if (temp == temp_Grid[i, j])
                    {
                        continue;
                    }
                }
            }
        }
    }







    // Random Setting
    void DoRandom()
    {
        for (int i = 0; i < m_Question / 2 + 1; i++)
        {
            // Vertical
            if (0 == m_WordList.Count)
                break;

            if (m_TempWord != null)
            {
                SetCrossVertical();
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
                SetCrossHorizontal();
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

        Rand(ref x, ref y);

        // Rule
        // x >= 0, Length + x < GridSize
        if (x >= 0 && Temp_word.Answer.Length + x < GridSize)
        {
            // 위, 아래에 공백 있는지 확인
            if (!CheckVertical(x, y, random_index))
                VerticalRule();

            if(CheckVertical(x,y) <= 0)
                VerticalRule();

            SetVertical(x, y, random_index);
        }
        else
            VerticalRule();
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
                for (int q = 0; q < Temp_word.WordInfo.m_CrossWordList.Count; q++)
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
                            //  0   0   0   0   0
                            //  a   p   p   l   e
                            //  c   0   0   0   0
                            //  e   0   0   0   0
                            if (y - cross_Index >= 0 && cross_word.Length - cross_Index + y < GridSize
                                && x >= 0 && Temp_word.Answer.Length + x < GridSize)
                            {
                                // 위, 아래에 공백 있는지 확인
                                if (CheckVertical(x, y, random_index))
                                {
                                    if (CheckVertical(x, y) <= 0)
                                        VerticalRule();
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
                            Vector2 temp_index = GetGrid(cross_word.Answer);
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
                                    break;
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
            SetGrid(x + i, y, m_WordList.GetAnswer(index), i);
        }

        m_WordList.UseIndex(index);

        print();
    }

    // 위, 아래의 공백을 갖도록 설정
    bool CheckVertical(int x, int y, int index)
    {
        int Length = m_WordList.GetAnswer(index).Length;
        for (int i = 0; i < Length; i++)
        {
            if(temp_Grid[x + i, y] != "--")
            {
                if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + i, y][i].ToString())
                    continue;
                return false;
            }
        }
        for (int i = 0; i < Length; i++)
        {
            //  0   0   0   0   0   0
            //  0   0   1   0   0   0
            //  0   0   2   0   0   0
            //  0   1   3   0   0   0
            // x와 단어의 길이를 더한 값이 그리드의 최대값에 도달하지 못했을때
            if (x + Length != GridSize - 1)
            {
                // x의 아래에 공백이 있을때만 true
                if (temp_Grid[x + i, y] != "--")
                {
                    if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + i, y + 1][i].ToString())
                        continue;
                    return false;
                }
                if (temp_Grid[x + i, y + 1] != "--")
                {
                    if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + i, y + 1][i].ToString())
                        continue;
                    return false;
                }
            }
            //  0   0   0   0   0   0   3   0   0
            //  0   0   0   1   2   3   4   0   0
            //  0   0   2   0   0   0   0   0   0
            // x의 시작위치의 위에 공백이 있는지
            if (x > 0)
            {
                if (temp_Grid[x - 1, y] != "--")
                {
                    if (temp_Grid[x + i, y - 1] != "--")
                    {
                        if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + i, y - 1][i].ToString())
                            continue;
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void SetCrossVertical()
    {
        Debug.Log(m_TempWord.Answer + " Vertical Crossed 단어를 생성합니다.");
        for (int i = 0; i < m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)).Length; i++)
        {
            SetGrid((int)m_TempWordLoc.x + i, (int)m_TempWordLoc.y, m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)), i);
        }

        m_WordList.UseIndex(m_WordList.FindWord(m_TempWord.Answer));
        m_TempWord = null;
        print();
    }





    // HORIZONTAL
    void HorizontalRule()
    {
        int random_index = Random.Range(0, m_WordList.Count);
        WordList.Word Temp_word = m_WordList.GetWord(random_index);
        Debug.Log(Temp_word.Answer + " Horizontal 단어를 생성합니다.");

        int x = 0;
        int y = 0;
        Rand(ref x, ref y);

        // Rule
        // y >= 0, Length + y < GridSize
        if (y >= 0 && Temp_word.Answer.Length + y < GridSize)
        {
            // 양 옆에 공백이 있는지 확인
            if (!CheckHorizontal(x, y, random_index))
                HorizontalRule();

            SetHorizontal(x, y, random_index);
        }
        else
            HorizontalRule();
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
                for (int q = 0; q < Temp_word.WordInfo.m_CrossWordList.Count; q++)
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
                            Vector2 temp_index = GetGrid(cross_word.Answer);
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
                                    break;
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
            SetGrid(x, y + i, m_WordList.GetAnswer(index), i);
        }

        m_WordList.UseIndex(index);

        print();
    }

    // 양 옆에 공백이 있는지 확인 해준다.
    bool CheckHorizontal(int x, int y, int index)
    {
        int Length = m_WordList.GetAnswer(index).Length;
        for (int i = 0; i < Length; i++)
        {
            if(temp_Grid[x, y + i] != "--")
            {
                if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x, y + i][i].ToString())
                    continue;
                return false;
            }
        }
        for (int i = 0; i < Length; i++)
        {
            //  0   0   0   0   0   0   3   0   0
            //  0   0   0   1   2   3   4   0   0
            //  0   0   2   0   0   0   0   0   0
            //
            // y와 단어의 길이를 더한 값이 그리드의 최대값에 도달하지 못했을때
            if (y + Length != GridSize - 1)
            {
                // y의 아래에 공백이 있을때만 true
                if (temp_Grid[x, y + i] != "--")
                {
                    if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + 1, y + i][i].ToString())
                        continue;
                    return false;
                }
                if (temp_Grid[x + 1, y + i] != "--")
                {
                    if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x + 1, y + i][i].ToString())
                        continue;
                    return false;
                }
            }
            //  0   0   0   0   0   0   3   0   0
            //  0   0   0   1   2   3   4   0   0
            //  0   0   2   0   0   0   0   0   0
            // y의 시작위치의 위에 공백이 있는지
            if (y > 0)
            {
                if (temp_Grid[x, y - 1] != "--")
                {
                    if(temp_Grid[x - 1, y + i] != "--")
                    {
                        if (m_WordList.GetAnswer(index)[i].ToString().ToUpper() == temp_Grid[x - 1, y + i][i].ToString())
                            continue;
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void SetCrossHorizontal()
    {
        Debug.Log(m_TempWord.Answer + " Crossed 단어를 생성합니다.");
        for (int i = 0; i < m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)).Length; i++)
        {
            SetGrid((int)m_TempWordLoc.x, (int)m_TempWordLoc.y + i, m_WordList.GetAnswer(m_WordList.FindWord(m_TempWord.Answer)), i);
        }

        m_WordList.UseIndex(m_WordList.FindWord(m_TempWord.Answer));
        m_TempWord = null;
        print();
    }

    int CheckHorizontal(int x, int y)
    {
        return CheckRight(x, y) + CheckLeft(x, y) - 1;
    }

    int CheckVertical(int x, int y)
    {
        return CheckUp(x, y) + CheckDown(x, y) - 1;
    }

    int CheckUp(int x, int y)
    {
        int count = 0;
        if (x > -1)
        {
            if (temp_Grid[x, y] == "0")
            {
                count++;
                count += CheckUp(x - 1, y);
            }
        }
        return count;
    }

    int CheckDown(int x, int y)
    {
        int count = 0;
        if (x < 10)
        {
            if (temp_Grid[x, y] == "0")
            {
                count++;
                count += CheckDown(x + 1, y);
            }
        }
        return count;
    }

    int CheckLeft(int x, int y)
    {
        int count = 0;
        if (y > -1)
        {
            if (temp_Grid[x, y] == "0")
            {
                count++;
                count += CheckLeft(x, y - 1);
            }
        }
        return count;
    }

    int CheckRight(int x, int y)
    {
        int count = 0;
        if (y < 10)
        {
            if (temp_Grid[x, y] == "0")
            {
                count++;
                count += CheckRight(x, y + 1);
            }
        }
        return count;
    }









    // Random XY
    void Rand(ref int x, ref int y)
    {
        x = Random.Range(0, GridSize - 1);
        y = Random.Range(0, GridSize - 1);
    }
}
