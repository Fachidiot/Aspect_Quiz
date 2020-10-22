using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordMgr : MonoBehaviour
{
    [Header("GridMake StartPosition")]
    public float StartX;
    public float StartY;
    [Header("GridSize Max Size")]
    public int MinX = -1030;
    public int MinY = -1030;
    public int MaxX = 1030;
    public int MaxY = 1030;
    [Header("Prefab")]
    public GameObject GridPrefab;
    [Header("Word Count")]
    public int WordCount;

    private int m_CurrentWordCount;
    private List<GameObject> m_GridMgr;
    private List<Word> m_WordList;
    private Word m_TempWord;
    private string m_LastWord;
    private bool m_bIsMake;

    private void Awake()
    {
        m_CurrentWordCount = 0;
        m_bIsMake = false;
        m_GridMgr = new List<GameObject>();
    }

    private void Update()
    {
        if(!m_bIsMake)
            LinkLogic();
    }

    void LinkLogic()
    {
        m_WordList = GameObject.Find("CrossMgr").GetComponent<CrossWord>().GetList();

        if (WordCount <= m_CurrentWordCount)
            m_bIsMake = true;
        // 루트 단어가 만들어 졌을때만
        if (RootSet())
        {
            MakeGrid();
            m_bIsMake = true;
            //CheckQuater();
        }
    }
    void UseWord(int index)
    {
        m_WordList.RemoveAt(index);
    }
    void UseWord(Word _word)
    {
        m_WordList.Remove(_word);
    }


    static int count = 0;
    int CheckWord(int index)
    {
        if (count > 10)
        {
            return -1;
        }
        // 랜덤으로 단어를 갖고 온다.
        int temp = Random.Range(0, m_TempWord.IndexInfo[index].Count);

        string val = m_TempWord.IndexInfo[index][temp];
        // 0 : Word, 1 : index1, 2 : index2
        var temp_string = val.Split(',');
        if (FindWord(temp_string[0]) == null)
        {
            count++;
            temp = CheckWord(index);
        }

        return temp;
    }
    void HorizontalMake()
    {
        for (int i = 0; i < m_TempWord.Length; i++)
        {
            var temp = Instantiate(GridPrefab, this.gameObject.transform);
            m_GridMgr.Add(temp);
            temp.transform.localPosition = new Vector3(StartX, StartY + 110 * i, 0);
            temp.name = m_TempWord.Answer + i;
            temp.tag = "Horizontal";
        }

        m_CurrentWordCount++;
    }
    void VerticalMake()
    {
        for (int i = 0; i < m_TempWord.Length; i++)
        {
            var temp = Instantiate(GridPrefab, this.gameObject.transform);
            m_GridMgr.Add(temp);
            temp.transform.localPosition = new Vector3(StartX + 110 * i, StartY, 0);
            temp.name = m_TempWord.Answer + i;
            temp.tag = "Vertical";
        }

        m_CurrentWordCount++;
    }
    bool CheckVertical()
    {
        // 교차 정보 체크
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        int temp = templist.Count;
        int tempcount = 0;
        for (int i = 0; i < temp; i++)
        {
            if (m_LastWord != null)
            {
                if (templist[i - tempcount].name.Contains(m_LastWord))
                {
                    templist.Remove(templist[i - tempcount]);
                    tempcount++;
                }
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length + 2; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX, StartY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(StartX + ((j - 1) * 110), StartY, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX + 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX + ((j - 1) * 110), StartY + 1, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX - 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX + ((j - 1) * 110), StartY - 1, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }
            }
        }

        return true;
    }
    bool CheckHorizontal()
    {
        // 교차 정보 체크
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        int temp = templist.Count;
        int tempcount = 0;
        for (int i = 0; i < temp; i++)
        {
            if(m_LastWord != null)
            {
                if (templist[i - tempcount].name.Contains(m_LastWord))
                {
                    templist.Remove(templist[i - tempcount]);
                    tempcount++;
                }
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length + 2; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX, StartY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(StartX, StartY + ((j - 1) * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX + 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX + 1, StartY + ((j - 1) * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX - 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX - 1, StartY + ((j - 1) * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }
            }
        }

        return true;
    }

    // 루트가 될 단어를 뽑고 생성해주기
    // proccess
    // 1. 자신 단어가 겹치지 않을때만
    // 2. h로 생성될 단어가 열려 있을때만
    // 3. h로 생성될 단어의 위치가 겹치지 않을때만
    // 4. 생성

    bool RootSet()
    {
        if (m_CurrentWordCount >= WordCount)
        {
            Debug.Log("모든 단어가 완성 되었습니다!");
            return false;
        }

        // 루트가 될 단어를 랜덤으로 뽑는다.
        var index = Random.Range(0, m_WordList.Count);
        m_TempWord = m_WordList[index];

        // 랜덤으로 뽑은 인덱스(알파벳)와 인덱스의 인덱스(알파벳과 교차하는 단어 리스트)
        int rand_alphabetindex = 0;
        int rand_wordindex = 0;

        int safecount = 0;
        while(true)
        {
            // temp = 단어의 알파벳 인덱스
            var temp = Random.Range(0, m_TempWord.IndexInfo.Count);

            // 해당 리스트에 교차되는 단어가 없을때
            if (m_TempWord.IndexInfo[temp].Count <= 0)
            {
                // 만약 전체 인덱스에 교차되는 단어가 1개도 없을때 무한루프에 빠지지 않도록 해준다.
                if (m_TempWord.IndexInfo.Count <= safecount)
                {
                    Debug.Log("교차단어가 없는 단어 발견!");

                    // 단어리스트 오류

                    UseWord(index);
                    return false;
                }
                safecount++;
                continue;
            }
            
            // 교차 단어가 있을때
            rand_alphabetindex = temp;
            count = 0;
            rand_wordindex = CheckWord(rand_alphabetindex);
            if (rand_wordindex == -1)
            {
                Debug.Log("교차 단어중 생성가능한게 없습니다.");
                continue;
            }
            break;
        }

        // 교차 정보
        string val = m_TempWord.IndexInfo[rand_alphabetindex][rand_wordindex];
        // 0 : Word, 1 : index1, 2 : index2
        var temp_string = val.Split(',');
        int _1 = int.Parse(temp_string[1]);
        int _2 = int.Parse(temp_string[2]);

        // 교차 정보 체크
        if(CheckVertical())
        {
            Debug.Log("Root Word Make");
            Debug.Log(m_TempWord.Answer + " 단어를 생성합니다.");
            VerticalMake();
            m_TempWord.m_bIsOpen[_1] = false;

            // 삭제전 단어 저장
            m_LastWord = m_TempWord.Answer;
            // 단어 생성후에 단어를 리스트에서 삭제해준다.
            UseWord(index);

            // 생성후에는 다음에 만들 단어를 저장해준다.
            m_TempWord = FindWord(temp_string[0]);
            m_TempWord.m_bIsOpen[_2] = false;
            // 다음 생성될 단어의 위치도 저장해준다.
            StartX = StartX + (_1 * 110);
            StartY = StartY - (_2 * 110);

            if(CheckHorizontal())
            {
                if (m_CurrentWordCount >= WordCount)
                {
                    Debug.Log("모든 단어가 완성 되었습니다!");
                    return false;
                }

                // 저장된 단어를 생성해준다.
                Debug.Log("Second Word Make");
                Debug.Log(m_TempWord.Answer + " 단어를 생성합니다.");
                HorizontalMake();

                return true;
            }
            return false;
        }

        return false;
    }
    
    // 루트 단어에 맞춰 생성해주기
    void MakeGrid()
    {
        // 단어가 없다면 return
        if (m_WordList.Count <= 0)
            return;

        bool IsMake = false;
        // 만약 다음에 만들 단어의 양옆이 비어있을때만 생성
        for (int i = 0; i < m_TempWord.IndexInfo.Count; i++)
        {
            // 해당 인덱스가 열려있을때
            if (m_TempWord.m_bIsOpen[i] == true && !IsMake)
            {
                // 인덱스의 끝일때
                if (i + 1 != m_TempWord.Length)
                {
                    // 인덱스의 다음 인덱스가 비어있는지 확인 해준다.
                    if (m_TempWord.m_bIsOpen[i + 1] != true)
                    { // 비어있지 않을때
                      // 다시 찾는다.
                        continue;
                    }
                }
                // 인덱스의 처음이 아닐때
                if (i - 1 != -1)
                {
                    // 인덱스의 이전 인덱스가 비어있는지 확인 해준다.
                    if (m_TempWord.m_bIsOpen[i - 1] != true)
                    { // 비어있지 않을때
                      // 다시 찾는다.
                        continue;
                    }
                }


                int rand_wordindex = 0;
                // 랜덤으로 단어를 갖고 온다.
                for (int j = 0; j < m_TempWord.IndexInfo[i].Count; j++)
                {
                    count = 0;
                    rand_wordindex = CheckWord(i);
                    if (rand_wordindex == -1)
                    {
                        Debug.Log("교차 단어중 생성가능한게 없습니다.");

                        // 단어를 리스트에서 지워준다.
                        UseWord(m_TempWord);

                        return;
                    }
                    string temp = m_TempWord.IndexInfo[i][rand_wordindex];
                    var temp_string = temp.Split(',');
                    // 단어가 있을때
                    if(HasWord(temp_string[0]))
                    { // 단어를 만들어준다.
                        // 0 : Word, 1 : index1, 2 : index2
                        int _1 = int.Parse(temp_string[1]);
                        int _2 = int.Parse(temp_string[2]);

                        // 단어를 리스트에서 지워준다.
                        UseWord(m_TempWord);

                        // 다음에 만들 단어를 저장해준다.
                        m_TempWord = FindWord(temp_string[0]);
                        m_LastWord = temp_string[0];
                        Debug.Log(m_TempWord.Answer + " 단어를 생성합니다.");
                        //Debug.Log(_1 + " Index, " + _2 + " Index");
                        m_TempWord.m_bIsOpen[_2] = false;
                        // 다음 생성될 단어의 위치도 저장해준다.
                        StartX = StartX - (_2 * 110);
                        StartY = StartY + (_1 * 110);

                        IsMake = true;
                        break;
                    }
                }
            }
        }

        // 다음 생성될 단어를 못찾은 경우
        if(!IsMake)
            return;

        if (m_CurrentWordCount >= WordCount)
        {
            Debug.Log("모든 단어가 완성 되었습니다!");
            return;
        }

        Debug.Log("Third Word Make");
        VerticalMake();

        // 단어가 없다면 return
        if (m_WordList.Count <= 0)
            return;

        //  -   -   -   -   -   -   -   -   -
        //  -   -   -   -   a   0   0   -   -
        //  -   -   -   -   p   -   -   -   -
        //  -   -   -   o   p   e   n   -   -
        // 마지막으로 한번만 더 생성 가능한지 체크해준다.
        bool IsMake2 = false;

        // 만약 다음에 만들 단어의 양옆이 비어있을때만 생성
        for (int i = 0; i < m_TempWord.IndexInfo.Count; i++)
        {
            // 해당 인덱스가 열려있을때
            if (m_TempWord.m_bIsOpen[i] == true && !IsMake2)
            {
                // 인덱스의 끝일때
                if (i + 1 != m_TempWord.Length)
                {
                    // 인덱스의 다음 인덱스가 비어있는지 확인 해준다.
                    if (m_TempWord.m_bIsOpen[i + 1] != true)
                    { // 비어있지 않을때
                      // 다시 찾는다.
                        continue;
                    }
                }
                // 인덱스의 처음이 아닐때
                if (i - 1 != -1)
                {
                    // 인덱스의 이전 인덱스가 비어있는지 확인 해준다.
                    if (m_TempWord.m_bIsOpen[i - 1] != true)
                    { // 비어있지 않을때
                      // 다시 찾는다.
                        continue;
                    }
                }


                int rand_wordindex = 0;
                // 랜덤으로 단어를 갖고 온다.
                for (int j = 0; j < m_TempWord.IndexInfo[i].Count; j++)
                {
                    count = 0;
                    rand_wordindex = CheckWord(i);
                    if (rand_wordindex == -1)
                    {
                        Debug.Log("교차 단어중 생성가능한게 없습니다.");

                        // 단어를 리스트에서 지워준다.
                        UseWord(m_TempWord);

                        return;
                    }
                    string temp = m_TempWord.IndexInfo[i][rand_wordindex];
                    var temp_string = temp.Split(',');
                    // 단어가 있을때
                    if (HasWord(temp_string[0]))
                    { // 단어를 만들어준다.
                        // 0 : Word, 1 : index1, 2 : index2
                        int _1 = int.Parse(temp_string[1]);
                        int _2 = int.Parse(temp_string[2]);

                        // 단어를 리스트에서 지워준다.
                        UseWord(m_TempWord);

                        // 다음에 만들 단어를 저장해준다.
                        m_TempWord = FindWord(temp_string[0]);
                        Debug.Log(m_TempWord.Answer + " 단어를 생성합니다.");
                        //Debug.Log(_1 + " Index, " + _2 + " Index");
                        m_TempWord.m_bIsOpen[_2] = false;
                        // 다음 생성될 단어의 위치도 저장해준다.
                        StartX = StartX + (_1 * 110);
                        StartY = StartY - (_2 * 110);

                        IsMake2 = true;
                        break;
                    }
                }
            }
        }

        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        for (int i = 0; i < templist.Count; i++)
        {
            if (templist[i].name.Contains(m_LastWord))
                templist.RemoveAt(i);
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX, StartY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(StartX, StartY + (j  * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return;
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX + 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX + 1, StartY + (j * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return;
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX - 110, StartY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(StartX - 1, StartY + (j * 110), 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return;
                }
            }
        }

        if (m_CurrentWordCount >= WordCount)
        {
            Debug.Log("모든 단어가 완성 되었습니다!");
            return;
        }

        // 단어를 리스트에서 지워준다.
        UseWord(m_TempWord);

        Debug.Log("Fourth Word Make");
        HorizontalMake();
    }


    // 4개의 면중에서 가장 많이 생성된 면을 제외한 면에서 생성을 시작해준다.
    private struct QuaterSection
    {
        public int Section1;
        public int Section2;
        public int Section3;
        public int Section4;

        public struct IntQuater
        {
            public int MinX;
            public int MaxX;
            public int MinY;
            public int MaxY;

            public IntQuater(int _1, int _2, int _3, int _4)
            {
                MinX = _1;
                MaxX = _2;
                MinY = _3;
                MaxY = _4;
            }
        }
        
        //public IntQuater Section1_MinMax;
        //public IntQuater Section2_MinMax;
        //public IntQuater Section3_MinMax;
        //public IntQuater Section4_MinMax;

        public int Rank()
        {
            if (Section2 > Section3)
            {
                if (Section2 > Section1)
                {
                    if (Section2 > Section4)
                    {
                        return 2;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 > Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
            else
            {
                if (Section3 > Section1)
                {
                    if (Section3 > Section4)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 > Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
        public int DisRank()
        {
            if(Section2 < Section3)
            {
                if(Section2 < Section1)
                {
                    if(Section2 < Section4)
                    {
                        return 2;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if(Section1 < Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
            else
            {
                if (Section3 < Section1)
                {
                    if (Section3 < Section4)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 < Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
        public int GetX(int sectionum, int randindex)
        {
            switch (sectionum)
            {
                case 1:
                    return randindex * -110;
                case 2:
                    return randindex * -110;
                case 3:
                    return randindex * 110;
                case 4:
                    return randindex * 110;
            }

            return 0;
        }
        public int GetY(int sectionum, int randindex)
        {
            switch (sectionum)
            {
                case 1:
                    return randindex * 110;
                case 2:
                    return randindex * -110;
                case 3:
                    return randindex * -110;
                case 4:
                    return randindex * 110;
            }

            return 0;
        }
    }

    void CheckQuater()
    {
        // 생성된 Grid를 가져온다.
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        QuaterSection Section = new QuaterSection();

        //Section.Section1_MinMax = new QuaterSection.IntQuater(MinX, 0, 0, MaxY);
        //Section.Section2_MinMax = new QuaterSection.IntQuater(MinX, MinY, 0, 0);
        //Section.Section3_MinMax = new QuaterSection.IntQuater(0, MinY, MaxX, 0);
        //Section.Section4_MinMax = new QuaterSection.IntQuater(0, 0, MaxX, MaxY);

        for (int i = 0; i < templist.Count; i++)
        {
            // 1분면 ( - , + )
            if (templist[i].transform.localPosition.x < 0 && templist[i].transform.localPosition.y > 0)
            {
                Section.Section1++;
            }

            // 2분면 ( - , - )
            if (templist[i].transform.localPosition.x < 0 && templist[i].transform.localPosition.y < 0)
            {
                Section.Section2++;
            }

            // 3분면 ( + , - )
            if (templist[i].transform.localPosition.x > 0 && templist[i].transform.localPosition.y < 0)
            {
                Section.Section3++;
            }

            // 4분면 ( + , + )
            if (templist[i].transform.localPosition.x > 0 && templist[i].transform.localPosition.y > 0)
            {
                Section.Section4++;
            }
        }

        // 가장 적게 생성된면을 불러온다.
        var num = Section.DisRank();
        //Debug.Log(num + " 분면에 가장 적은 단어가 생성되었습니다.");

        // 가장 적게 생성된면의 랜덤으로 좌표를 구한다.
        StartX = Section.GetX(num, Random.Range(0, 9));
        StartY = Section.GetY(num, Random.Range(0, 9));

        // 구해진 좌표 체크 : 생성전에 생성될 위치에 자리가 있는지 확인
        var templist2 = new List<GameObject>();
        templist2.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist2.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));

        for (int i = 0; i < templist2.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX, StartY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist2[i].transform.localPosition == new Vector3(StartX + (j * 110), StartY, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    CheckQuater();
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX + 110, StartY + (j * 110), 0));
                if (templist2[i].transform.localPosition == new Vector3(StartX + (j * 110), StartY + 1, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    CheckQuater();
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(StartX - 110, StartY + (j * 110), 0));
                if (templist2[i].transform.localPosition == new Vector3(StartX + (j * 110), StartY - 1, 0))
                { // 비어있지 않을때
                    Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    CheckQuater();
                }
            }
        }
    }

    public void RESET()
    {
        Debug.Log("Reset Start");
        Resetting();
        m_bIsMake = false;
        m_CurrentWordCount = 0;
    }
    public IEnumerator Reset()
    {
        Debug.Log("Reset Start");
        Resetting();
        m_bIsMake = false;
        yield return new WaitForSeconds(2f);
    }
    void Resetting()
    {
        var temp = m_GridMgr.Count;
        for (int i = 0; i < temp; i++)
        {
            Destroy(m_GridMgr[i]);
        }
    }

    Word FindWord(string _word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.Contains(_word))
                return m_WordList[i];
        }

        return null;
    }

    bool HasWord(string _word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.Contains(_word))
                return true;
        }

        return false;
    }
}
