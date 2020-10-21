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
    private char[,] Temp_Grid;
    // temp_Grid
    // 0 = dontuse   1 = use
    private WordList m_WordList;
    private int[] SaveIndex = new int[2] { -1, -1 };
    private WordList.Word SaveWord = new WordList.Word();

    void Start()
    {
        TempGridGenerate();
        makeAtOnce();
        showGrid();
        GridGenerate();
    }

    public void TempGridGenerate()
    {  // 생성자
        Temp_Grid = new char[GridSize, GridSize];
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Temp_Grid[i, j] = 'O';
            }
        }
    }

    public void GridGenerate()
    {  // 생성자
        m_Grid = new GameObject[GridSize, GridSize];
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if(Temp_Grid[i, j] == 'O')
                {
                    this.m_Grid[i, j] = Instantiate(GridPrefab, gameObject.transform);
                    this.m_Grid[i, j].transform.localPosition += new Vector3(((i - (GridSize - 1) / 2) * 130), ((j - (GridSize - 2) / 2) * 130), 0);
                    //this.m_Grid[i, j].GetComponent<InputWord>().SetUp(Temp_Grid[i, j].ToString(), 0);
                }
            }
        }
    }

    // 생성부: 그리드를 그리는 하위부문

    private void closeSquare(int x, int y)
    {  // 칸 닫기
        Temp_Grid[x, y] = 'X';
    }

    private void symmetricalCloser(int x, int y)
    {  // 대칭하여 닫기, (그리드의 길이가 홀수일 경우만)
        int median = GridSize / 2;
        closeSquare(x, y);
        int rowDifference = median - x;
        int columnDifference = median - y;
        closeSquare(median + rowDifference, median + columnDifference);
    }

    private void randomlyAndSymmetricallyClose()
    {  // 무작위로 닫되, 닫힌 칸과 대칭되는 칸도 함께 닫기
        int x = Random.Range(0, GridSize);
        int y = Random.Range(0, GridSize);

        if (x < GridSize && y < GridSize && Temp_Grid[x, y] == 'O')
        {
            symmetricalCloser(x, y);
        }
        else
        {
            randomlyAndSymmetricallyClose();
        }
    }

    private void randomlyAndSymmetricallyCloseForLargeSize()
    {
        // 크기가 큰 그리드를 위한 무작위 및 대칭 닫기
        int a = Random.Range(0, GridSize);
        int b = Random.Range(0, GridSize);
        int c = Random.Range(0, GridSize);
        int d = Random.Range(0, GridSize);
        int x = a + b;
        int y = c + d;

        if (x < GridSize && y < GridSize && Temp_Grid[x, y] == 'O')
        {
            symmetricalCloser(x, y);
        }
        else
        {
            randomlyAndSymmetricallyCloseForLargeSize();
        }
    }

    private void recursiveRowCloser(int x, int y)
    {
        // 하나의 행을 한 칸씩 건너뛰어 닫으면서 그와 대칭되는 칸도 함께 닫기
        if (y + 2 <= this.GridSize + 1)
        {
            symmetricalCloser(x, y);
            y = y + 2;
            recursiveRowCloser(x, y);
        }
    }

    public void makeAtOnce()
    {
        for (int i = 0; i < this.GridSize / 2 + 1; i += 2)
        { // (1, 0), 즉 i = 1에서부터 닫아도 좋음
            recursiveRowCloser(i, 0);
        }
        // 한 번 패턴화된 그리드에 대해서 랜덤하게 칸 닫기
        if (this.GridSize == 11)
        {
            for (int i = 1; i < 9; i++)
            { // 그리드의 길이가 11일 경우 i < 9, 13일 경우 i < 14가 적당..
                randomlyAndSymmetricallyClose();
            }
        }
        else if (this.GridSize == 13)
        {
            for (int i = 1; i < 14; i++)
            {
                randomlyAndSymmetricallyClose();
            }
        }
        else if (this.GridSize == 15)
        {
            for (int i = 1; i < 21; i++)
            {
                randomlyAndSymmetricallyClose();
            }
        }
        else if (this.GridSize == 17)
        {
            for (int i = 1; i < 29; i++)
            {
                randomlyAndSymmetricallyCloseForLargeSize();
            }
        }
        else if (this.GridSize == 19)
        {
            for (int i = 1; i < 34; i++)
            {
                randomlyAndSymmetricallyCloseForLargeSize();
            }
        }
        if (wellFormednessValidator() == false)
        { // 정형의 그리드가 아닐 경우, 다시 반복
            openAtOnce();
            makeAtOnce();
        }
    }

    private void openAtOnce()
    { // 모든 칸을 열기, i.e. 그리드 초기화
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                Temp_Grid[i, j] = 'O';
            }
        }
    }
    // 생성부 끝

    // 블록 추출부 - 생성된 그리드가 가지고 있는 블록에 대한 정보를 리스트의 리스트의 형태로 추출하는 하위부문
    private int rightwardSquareCounter(int x, int y)
    { // 입력된 좌표를 포함한 좌표의 오른쪽 행의 빈칸 개수 구하기
        int count = 0;
        if (y < GridSize)
        {
            if (Temp_Grid[x, y] == 'O')
            {
                count++;
                count += rightwardSquareCounter(x, y + 1);
            }
        }
        return count;
    }
    private int leftwardSquareCounter(int x, int y)
    { // 입력된 좌표를 포함한 좌표의 왼쪽 행의 빈칸 개수 구하기
        int count = 0;
        if (y > -1)
        {
            if (Temp_Grid[x, y] == 'O')
            {
                count++;
                count += leftwardSquareCounter(x, y - 1);
            }
        }
        return count;
    }
    private int downwardSquareCounter(int x, int y)
    { // 입력된 좌표를 포함한 좌표의 아래쪽 열의 빈칸 개수 구하기
        int count = 0;
        if (x < GridSize)
        {
            if (Temp_Grid[x, y] == 'O')
            {
                count++;
                count += downwardSquareCounter(x + 1, y);
            }
        }
        return count;
    }
    private int upwardSquareCounter(int x, int y)
    { // 입력된 좌표를 포함한 좌표의 위쪽 열의 빈칸 개수 구하기
        int count = 0;
        if (x > -1)
        {
            if (Temp_Grid[x, y] == 'O')
            {
                count++;
                count += upwardSquareCounter(x - 1, y);
            }
        }
        return count;
    }
    private int horizontalSquareCounter(int x, int y)
    {
        // 입력된 좌표와 연결된 행의 빈칸 개수 구하기
        if (Temp_Grid[x, y] == 'X')
        {
            return -30;
        }
        else
        {
            return rightwardSquareCounter(x, y) + leftwardSquareCounter(x, y) - 1;
        }
    }
    private int verticalSquareCounter(int x, int y)
    {
        // 입력된 좌표와 연결된 열의 빈칸 개수 구하기
        if (Temp_Grid[x, y] == 'X')
        {
            return -30;
        }
        else
        {
            return downwardSquareCounter(x, y) + upwardSquareCounter(x, y) - 1;
        }
    }
    private int getBlockType(int x, int y)
    {
        // 입력된 좌표가 수평방향의 블록과 연결되어 있는지 수직방향의 블록과 연결되어 있는지,
        // 아니면 둘 다 인지 알아내기
        if (horizontalSquareCounter(x, y) > 1 && verticalSquareCounter(x, y) > 1)
        {
            return -50; // 수평방향의 블록, 수직방향의 블록 둘 다와 연결되어 있는 경우
        }
        else if (horizontalSquareCounter(x, y) > 1 && verticalSquareCounter(x, y) == 1)
        {
            return 100; // 수평방향의 블록과 연결되어 있는 경우
        }
        else if (horizontalSquareCounter(x, y) == 1 && verticalSquareCounter(x, y) > 1)
        {
            return -100; // 수직방향의 블록과 연결되어 있는 경우
        }
        else if (horizontalSquareCounter(x, y) == -30 && verticalSquareCounter(x, y) == -30)
        {
            return -30; // 좌표가 닫혀있는 경우, 즉 "X" 인경우
        }
        else
        {
            return -1;
        }
    }
    private int horizontalCoordinateFinder(int x, int y)
    {
        // 입력된 좌표와 수평선상으로 연결되어 있는 블록의 왼쪽끝 좌표 구하기
        if (Temp_Grid[x, y] == 'X')
        { // 입력된 좌표가 닫혀있을 경우
            return -1;
        }
        else
        {
            for (int i = y; i > -1; i--)
            {
                if (Temp_Grid[x, i] == 'X')
                { // ex) (1,3)에서 "X"를 만났을 경우, 블록의 왼쪽끝 좌표는 (1,4)임
                    return i + 1;
                }
            }
            return 0; // "X"를 만나지 않았을 경우, 즉 y축 좌표 0까지 갔을 경우.
        }
    }
    private int verticalCoordinateFinder(int x, int y)
    {
        // 입력된 좌표와 수직선상으로 연결되어 있느 블록의 맨 위 좌표 구하기
        if (Temp_Grid[x, y] == 'X')
        {
            return -1;
        }
        else
        {
            for (int i = x; i > -1; i--)
            {
                if (Temp_Grid[i, y] == 'X')
                { // ex) (2,2)에서 "X"를 만났을 경우, 블록의 맨 위 좌표는 (2,3)임
                    return i + 1;
                }
            }
            return 0; // "X"를 만나지 않았을 경우, 즉 x축 좌표 0까지 갔을 경우.
        }
    }
    private int[] getInfoOfBlock(int x, int y)
    { // 입력된 좌표와 연결되어 있는 블록의 정보 획득
      // 빈칸의 정보: x좌표, y좌표, 길이, 방향(수평, 수직), 교차점의 개수
        int xCoordinate = -1;
        int yCoordinate = -1;
        int length = -1;
        int type = getBlockType(x, y);
        int intersection = -1;

        if (type == 100)
        { // 연결된 블록이 수평인 경우
            xCoordinate = x;
            yCoordinate = horizontalCoordinateFinder(x, y);
            length = horizontalSquareCounter(x, y);
            int count = 0;
            for (int i = yCoordinate; i < length + yCoordinate; i++)
            {
                if (verticalSquareCounter(xCoordinate, i) > 1)
                {
                    count++;
                }
            }
            intersection = count;
        }
        else if (type == -100)
        { // 연결된 블록이 수직인 경우
            xCoordinate = verticalCoordinateFinder(x, y);
            yCoordinate = y;
            length = verticalSquareCounter(x, y);
            int count = 0;
            for (int i = xCoordinate; i < length + xCoordinate; i++)
            {
                if (horizontalSquareCounter(i, yCoordinate) > 1)
                {
                    count++;
                }
            }
            intersection = count;
        }
        else if (type == -50)
        {  // 좌표가 교차점인 경우는, 정보가 중복되므로 닫힌 칸인 경우와 동일하게 처리
            xCoordinate = 0;
            yCoordinate = 0;
            length = 0;
            type = 0;
            intersection = 0;
        }
        else if (type == -30)
        { // 닫힌 칸, 즉 "X"인 경우
            xCoordinate = 0;
            yCoordinate = 0;
            length = 0;
            type = 0;
            intersection = 0;
        }
        int[] info = { xCoordinate, yCoordinate, length, type, intersection };
        return info;
    }
    public List<int[]> getEveryInfoOfBlocks()
    {
        //  n x n 그리드 상에 존재하는 모든 좌표와 연결된 블록의 정보 획득
        List<int[]> everyBlock = new List<int[]>();
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                everyBlock.Add(getInfoOfBlock(i, j));
            }
        }
        return everyBlock;
    }
    // 블록 추출부 끝

    // 그리드 검사부: 생성된 그리드가 제약조건을 만족하는 정형(well-formed)의 그리드인지, 또는 "쓸만한" 그리드인지 검사하는 하위부문
    public int closedSquareCounter()
    { // 닫혀 있는 칸의 개수 새기
        int count = 0;
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (Temp_Grid[i, j] == 'X')
                {
                    count++;
                }
            }
        }
        return count;
    }
    public int openedSquareCounter()
    { // 열린 칸의 개수 새기
        int count = 0;
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (Temp_Grid[i, j] == 'O')
                {
                    count++;
                }
            }
        }
        return count;
    }
    public bool wellFormednessValidator()
    {  // 그리드가 제약조건을 만족하는 정형의 그리드인지 검사하기
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                int[] temp = getInfoOfBlock(i, j);
                int xCoordinate = temp[0];
                int yCoordinate = temp[1];
                int length = temp[2];
                int type = temp[3];
                if (xCoordinate == -1 && yCoordinate == -1 && length == -1 && type == -1)
                {
                    // 고립된 블록(가로 세로의 길이가 1인 블록)이 존재하는 케이스
                    return false;
                }
            }
        }
        return true;
    }

    public void showGrid()
    {   // 그리드 보여주기
        for (int i = 0; i < GridSize; i++)
        {
            Debug.Log(Temp_Grid[i, 0] + " " + Temp_Grid[i, 1] + " " + Temp_Grid[i, 2] + " " + Temp_Grid[i, 3] + " " + Temp_Grid[i, 4] + " " + Temp_Grid[i, 5] + " " + Temp_Grid[i, 6] + " " + Temp_Grid[i, 7] + " " + Temp_Grid[i, 8] + " " + Temp_Grid[i, 9] + " " + Temp_Grid[i, 10]);
        }
    }
}
