using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public bool Enable;
    [Header("Grid Size")]
    public int GridSize;
    [Tooltip("GridObject")]
    public GameObject GridPrefab;
    public GameObject VerticalPrefab;
    public GameObject HorizontalPrefab;

    private GameObject[,] m_Grid;
    private string[,] temp_Grid;
    // temp_Grid
    // 0 = dontuse   1 = use
    private WordList m_WordList;
    private int m_Question = 0;
    private int m_QuestionLength = 0;
    private int[] SaveIndex = new int[2] { -1, -1 };
    private WordList.Word SaveWord = new WordList.Word();

    void Start()
    {
        if (!Enable)
            return;
        TempGenerate(this.GridSize);
        m_WordList = csvReader.GetList(GridSize);
        CheckList();
        RandomSetGrid();
        for (int i = 0; i < GridSize; i++)
        {
            Debug.Log(temp_Grid[i, 0] + " " + temp_Grid[i, 1] + " " + temp_Grid[i, 2] + " " + temp_Grid[i, 3] + " " + temp_Grid[i, 4] + " " + temp_Grid[i, 5] + " "
            + temp_Grid[i, 6] + " " + temp_Grid[i, 7] + " " + temp_Grid[i, 8] + " " + temp_Grid[i, 9] + " " + temp_Grid[i, 10] + " " + temp_Grid[i, 11] + " " + temp_Grid[i, 12]);
        }
        GridGenerate(this.GridSize);
    }

    public WordList.Word.Inspector Inspecting(int index)
    {
        WordList.Word.Inspector TempInfo = new WordList.Word.Inspector();
        TempInfo = m_WordList.GetInfo(index);
        if (TempInfo.m_CrossWordList.Count > 0)
                return TempInfo;

        return null;
    }

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

    void SetGrid(int x, int y, char _word)
    {
        if (x > GridSize - 1 || y > GridSize - 1)
        {
            Debug.LogError("잘못된 인덱스");
            return;
        }
        temp_Grid[x, y] = _word.ToString().ToLower();
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
                this.m_Grid[i, j].transform.position += new Vector3((i - (GridSize - 1) / 2 + 0.3f) * 0.15f, (j - (GridSize - 2) / 2 - 0.3f) * 0.15f, 0);
            }
        }
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
        for (int i = 0; i < m_Question; i++)
        {
            if (m_WordList.GetAnswer(i).Length > m_QuestionLength)
                m_QuestionLength = m_WordList.GetAnswer(i).Length;
        }
    }

    void RandomSetGrid()
    {
        while (true)
        {
            if (0 == m_Question)
                break;
            RandomVerticalOpen(0);

            if (0 == m_Question)
                break;
            RandomHorizontalOpen(0);
        }
    }

    void RandomVerticalOpen(int index)
    {
        int x = 0;
        int y = 0;
        if (SaveIndex[0] != -1 && m_WordList.GetWord(index) != SaveWord)
            CrossVertical(x, y, index);
        else
            Vertical(x, y, index);
    }

    void CrossVertical(int x, int y, int index)
    {
        x = SaveIndex[0];
        y = SaveIndex[1];
        string tempword = m_WordList.GetAnswer(index);
        for (int i = 0; i < tempword.Length; i++)
        {
            if (tempword[i].ToString().ToLower() == temp_Grid[x, y])
            {
                if (SetHorizontalChain(x, y - i, m_WordList.FindWord(SaveWord.Answer)))
                    break;
            }
        }

        SaveIndex[0] = -1;
        SaveIndex[1] = -1;
        SaveWord = null;
        m_Question = m_WordList.Count;
    }

    // 가로
    void Vertical(int x, int y, int index)
    {
        x = Random.Range(0, this.GridSize);
        y = Random.Range(0, this.GridSize);

        // 현재 리스트의 단어와 크로스하는 리스트중 무작위로 선점
        int temp = index;
        if (m_WordList.GetInfo(index).m_CrossWordList.Count >= 1)
        {
            temp = Random.Range(0, m_WordList.GetInfo(index).m_CrossWordList.Count);
            int _CrossIndex = m_WordList.GetInfo(index).m_CrossWordList[temp].FindChar(m_WordList.GetInfo(index).m_CrossChar[temp]);

            if (y - _CrossIndex > 0 && y + m_WordList.GetInfo(index).m_CrossWordList[temp].Length - _CrossIndex < this.GridSize - 1)
                RandomVerticalOpen(index);

            if (m_WordList.GetInfo(index) != null)
            {
                SaveIndex[0] = x;
                SaveIndex[1] = y + m_WordList.GetInfo(index).m_CrossIndex[temp];
                SaveWord = m_WordList.GetInfo(index).m_CrossWordList[temp];
            }
            
            SetVerticalChain(x, y, index);
            m_Question = m_WordList.Count;
            return;
        }

        if (temp_Grid[x, y] == "-")
        {
            SetVerticalChain(x, y, index);
            m_Question = m_WordList.Count;
        }
        else
            RandomVerticalOpen(index);
    }

    void RandomHorizontalOpen(int index)
    {
        int x = 0;
        int y = 0;
        if (SaveIndex[0] != -1 && m_WordList.GetWord(index) != SaveWord)
            CrossHorizontal(x, y, index);
        else
            Horizontal(x, y, index);
    }

    void CrossHorizontal(int x, int y, int index)
    {
        x = SaveIndex[0];
        y = SaveIndex[1];
        for (int i = 0; i < SaveWord.Length; i++)
        {
            if (SaveWord.Answer[i].ToString().ToLower() == temp_Grid[x, y])
            {
                if (SetHorizontalChain(x - i, y, m_WordList.FindWord(SaveWord.Answer)))
                    break;
            }
        }

        SaveIndex[0] = -1;
        SaveIndex[1] = -1;
        SaveWord = null;
        m_Question = m_WordList.Count;
    }
    
    // 세로
    void Horizontal(int x, int y, int index)
    {
        x = Random.Range(0, this.GridSize);
        y = Random.Range(0, this.GridSize);

        int temp = 0;
        if (m_WordList.GetInfo(index).m_CrossWordList.Count > 1)
        {
            temp = Random.Range(0, m_WordList.GetInfo(index).m_CrossWordList.Count);
            int _CrossIndex = m_WordList.GetInfo(index).m_CrossWordList[temp].FindChar(m_WordList.GetInfo(index).m_CrossChar[temp]);
            if (x - _CrossIndex < 0 && x + m_WordList.GetInfo(index).m_CrossWordList[temp].Length - _CrossIndex < this.GridSize - 1)
                RandomVerticalOpen(index);
            if (m_WordList.GetInfo(index) != null)
            {
                SaveIndex[0] = x + m_WordList.GetInfo(index).m_CrossIndex[temp];
                SaveIndex[1] = y;
                SaveWord = m_WordList.GetInfo(index).m_CrossWordList[temp];
            }
            SetHorizontalChain(x, y, index);
            m_Question = m_WordList.Count;
        }

        if (temp_Grid[x, y] == "-")
        {
            SetHorizontalChain(x, y, index);
            m_Question = m_WordList.Count;
        }
        else
            RandomHorizontalOpen(index);
    }

    int GetVerticalChain(int x, int y)
    {
        int count = 0;
        if (y <= this.GridSize - 1)
        {
            if (temp_Grid[x, y] == "-")
            {
                count++;
                count += GetVerticalChain(x, y + 1);
            }
        }
        return count;
    }

    int GetHorizontalChain(int x, int y)
    {
        int count = 0;
        if (x < this.GridSize - 1)
        {
            if (temp_Grid[x, y] == "-")
            {
                count++;
                count += GetHorizontalChain(x + 1, y);
            }
        }
        return count;
    }

    bool SetVerticalChain(int x, int y, int index)
    {
        if (x < this.GridSize)
        {
            for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
            {
                SetGrid(x, y + i, m_WordList.GetAnswer(index)[i]);
            }
        }
        else
            return false;
        
        m_WordList.FindDelete(m_WordList.GetAnswer(index));
        m_WordList.UseIndex(index);
        return true;
    }

    bool SetHorizontalChain(int x, int y, int index)
    {
        if (x < this.GridSize)
        {
            for (int i = 0; i < m_WordList.GetAnswer(index).Length; i++)
            {
                SetGrid(x + i, y, m_WordList.GetAnswer(index)[i]);
            }
        }
        else
            return false;

        m_WordList.FindDelete(m_WordList.GetAnswer(index));
        m_WordList.UseIndex(index);
        return true;
    }
}
