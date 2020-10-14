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

    private GameObject[,] Grid;
    private WordList m_WordList;
    private int m_Question = 0;

    void Start()
    {
        GridGenerate(this.GridSize);
        m_WordList = csvReader.GetList();
        CheckList();
        RandomGrid();
    }

    // GridGenerator
    public void GridGenerate(int _wordCount)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 200, GridSize * 200);
        Grid = new GameObject[_wordCount,_wordCount];
        for (int i = 0; i < this.GridSize; i++)
            for (int j = 0; j < this.GridSize; j++)
            {
                this.Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
                this.Grid[i, j].transform.position += new Vector3((i - (GridSize - 1) / 2 + 0.3f) * 0.15f, (j - (GridSize - 2) / 2 - 0.3f) * 0.15f, 0);
            }
    }

    public void SetGrid()
    {
    }

    void CheckList()
    {
        Debug.Log("리스트 체크");
        for (int i = 0; i < m_WordList.Count(); i++)
        {
            if (m_WordList.GetAnswer(i).Length > this.GridSize)
                m_WordList.DontUse(i);

            else
                m_Question++;
        }
    }

    void SetGrid(int x, int y, bool _close = true)
    {
        Grid[x, y].SetActive(_close);
    }

    // 규칙
    // 1. 첫번째로 만들어질땐 무조건 가로먼저.
    void RandomGrid()
    {
        Debug.Log("랜덤 셋팅을 시작합니다.");
        int count = 0;
        while(count == m_Question)
        {
            int x = Random.Range(0, this.GridSize - 1);
            int y = Random.Range(0, this.GridSize - 1);
            int index = Random.Range(0, this.m_Question - 1);

            if (GetVerticalChain(x, y) >= m_WordList.GetAnswer(index).Length)
            {
                if (count / 2 == 0)
                    SetVerticalChain(x, y, index);
                else
                    SetHorizontalChain(x, y, index);
                count++;
            }
            else
                RandomGrid();
        }
    }

    int GetVerticalChain(int x, int y)
    {
        int count = 0;
        if (y < this.GridSize)
            if (Grid[x, y].activeSelf == true)
            {
                count++;
                count += GetVerticalChain(x, y + 1);
            }

        return count;
    }

    int GetHorizontalChain(int x, int y)
    {
        int count = 0;
        if (y < GridSize)
            if (Grid[x, y].activeSelf == true)
            {
                count++;
                count += GetHorizontalChain(x + 1, y);
            }

        return count;
    }

    void SetVerticalChain(int x, int y, int index)
    {
        // Vertical Word 생성
        this.Grid[x, y] = Instantiate(Vertical, this.Grid[x,y].transform);
        this.Grid[x, y].GetComponent<InputWord>().SetUp(m_WordList.GetAnswer(index), m_WordList.GetMeaning(index));
    }

    void SetHorizontalChain(int x, int y, int index)
    {
        // Horizontal Word 생성
        this.Grid[x, y] = Instantiate(Horizontal, this.Grid[x, y].transform);
        this.Grid[x, y].GetComponent<InputWord>().SetUp(m_WordList.GetAnswer(index), m_WordList.GetMeaning(index));
    }
}
