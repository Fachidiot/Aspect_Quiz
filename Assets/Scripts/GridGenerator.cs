using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Size")]
    public int GridSize;
    [Tooltip("GridObject")]
    public GameObject GridPrefab;

    private GameObject[,] Grid;

    void Start()
    {
        GridGenerate(GridSize);
    }

    // GridGenerator
    public void GridGenerate(int _wordCount)
    {
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridSize * 200, GridSize * 200);
        this.Grid = new GameObject[_wordCount,_wordCount];
        this.GridSize = _wordCount;
        for (int i = 0; i < this.GridSize; i++)
            for (int j = 0; j < this.GridSize; j++)
            {
                this.Grid[i, j] = Instantiate(GridPrefab, this.gameObject.transform);
                Grid[i, j].transform.position += new Vector3((i - (GridSize - 1) / 2 + 0.3f) * 0.15f, (j - (GridSize - 2) / 2 - 0.3f) * 0.15f, 0);
                Debug.Log(Grid[i, j].transform.position);
            }
    }

    void SetGrid(int x, int y, bool _close = true)
    {
        this.Grid[x, y].SetActive(_close);
    }
}
