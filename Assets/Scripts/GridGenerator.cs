using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Size")]
    public int GridSize;
    [Tooltip("GridObject")]
    public GameObject GridPrefab;
    public GameObject Vertical;
    public GameObject Horizontal;

    private GameObject[,] m_Grid;
    private int[,] temp_Grid;
    // temp_Grid
    // 0 = dontuse   1 = use
    private WordList m_WordList;
    private int m_Question = 0;
    private int m_QuestionLength = 0;

    void Start()
    {
        TempGenerate(this.GridSize);
        m_WordList = csvReader.GetList();
        CheckList();
        RandomSetGrid();
        PrintGrid();
        GridGenerate(this.GridSize);
    }

    public void TempGenerate(int _wordCount)
    {
        temp_Grid = new int[_wordCount, _wordCount];
        for (int i = 0; i < this.GridSize; i++)
            for (int j = 0; j < this.GridSize; j++)
            {
                temp_Grid[i, j] = 0;
            }
    }

    // GridGenerator
    //public void GridGenerate(int _wordCount)
    //{
    //    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 200, GridSize * 200);
    //    m_Grid = new GameObject[_wordCount,_wordCount];
    //    for (int i = 0; i < this.GridSize; i++)
    //        for (int j = 0; j < this.GridSize; j++)
    //        {
    //            this.m_Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
    //            this.m_Grid[i, j].transform.position += new Vector3((i - (GridSize - 1) / 2 + 0.3f) * 0.15f, (j - (GridSize - 2) / 2 - 0.3f) * 0.15f, 0);
    //        }
    //}

    // GridGenerator
    public void GridGenerate(int _wordCount)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 200, GridSize * 200);
        m_Grid = new GameObject[_wordCount, _wordCount];
        for (int i = 0; i < this.GridSize; i++)
            for (int j = 0; j < this.GridSize; j++)
            {
                if (temp_Grid[i, j] == 0)
                    continue;
                this.m_Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
                this.m_Grid[i, j].transform.position += new Vector3((i - (GridSize - 1) / 2 + 0.3f) * 0.15f, (j - (GridSize - 2) / 2 - 0.3f) * 0.15f, 0);
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

    void PrintGrid()
    {
        for (int i = 0; i < this.GridSize - 1; i++)
        {
            Debug.Log(temp_Grid[i, 0] + " " + temp_Grid[i, 1] + " " + temp_Grid[i, 2] + " " + temp_Grid[i, 3] + " " + temp_Grid[i, 4] + " " + temp_Grid[i, 5] + " " + temp_Grid[i, 6] + " " + temp_Grid[i, 7]);
        }
    }
    
    void RandomVerticalOpen(int index)
    {
        int x = Random.Range(0, this.GridSize - 1);
        int y = Random.Range(0, this.GridSize - 1);

        if (temp_Grid[x, y] == 0 && GetVerticalChain(x,y) >= m_QuestionLength)
        {
            SetVerticalChain(x, y, m_WordList.GetAnswer(index).Length);
        }
        else
            RandomVerticalOpen(index);
    }

    void RandomHorizontalOpen(int index)
    {
        int x = Random.Range(0, this.GridSize - 1);
        int y = Random.Range(0, this.GridSize - 1);

        if (temp_Grid[x, y] == 0 && GetHorizontalChain(x, y) >= m_QuestionLength)
        {
            SetHorizontalChain(x, y, m_WordList.GetAnswer(index).Length);
        }
        else
            RandomHorizontalOpen(index);
    }

    //int GetOpenTemp()
    //{
    //    int count = 0;
    //    for (int i = 0; i < this.GridSize - 1; i++)
    //        for (int j = 0; j < this.GridSize - 1; j++)
    //            if (temp_Grid[i, j] == 1)
    //                count++;
    //    return count;
    //}

    void RandomSetGrid()
    {
        int index = 0;
        while (true)
        {
            if (index == m_Question)
                break;
            RandomVerticalOpen(index);
            index++;

            if (index == m_Question)
                break;
            RandomHorizontalOpen(index);
            index++;
        }
    }

    int GetVerticalChain(int x, int y)
    {
        int count = 0;
        if (y <= this.GridSize - 1)
        {
            if (temp_Grid[x, y] == 0)
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
            if (temp_Grid[x, y] == 0)
            {
                count++;
                count += GetHorizontalChain(x + 1, y);
            }

        return count;
    }

    void SetVerticalChain(int x, int y, int _count)
    {
        if (x < this.GridSize - 1)
            if (_count != 0)
            {
                temp_Grid[x, y] = 1;
                SetVerticalChain(x, y + 1, _count - 1);
            }
    }

    void SetHorizontalChain(int x, int y, int _count)
    {
        if (x < this.GridSize - 1)
            if (_count != 0)
            {
                temp_Grid[x, y] = 1;
                SetHorizontalChain(x + 1, y, _count - 1);
            }
    }
}
