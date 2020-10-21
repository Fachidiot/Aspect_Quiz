using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMake : MonoBehaviour
{
    public GameObject GridPrefab;
    
    private LinkedList<Word> m_LinkMap;
    private LinkedListNode<Word> m_Pointer;
    private GameObject m_WordMgr;

    void Start()
    {
        m_WordMgr = GameObject.Find("WordMgr");
        MakeGrid();
    }


    // Word Mgr에서 한번 쓰인 인덱스는 다시 못쓰도록
    void MakeGrid()
    {
        m_Pointer = m_LinkMap.First;
        int Posx = -400;
        int Posy = -400;
        int Indx = 0;
        int Indy = 0;
        try
        {
            for (int i = 0; i < m_LinkMap.Count; i++)
            {
                GameObject temp = new GameObject();
                for (int j = 0; j < m_Pointer.Value.Answer.Length; j++)
                {
                    temp = Instantiate(GridPrefab, this.gameObject.transform);

                    if(i == 0)
                    {
                        //string tempindex = tempindex = m_Pointer.Value.IndexInfo[0];
                        //var index = tempindex.Split(' ');
                        //Indx = int.Parse(index[0]);
                        //Indy = int.Parse(index[1]);
                    }

                    // Vertical
                    if (i % 2 == 0)
                    {
                        temp.transform.localPosition += new Vector3(Posx + 110f * (j - Indx), Posy + 110f * i, 0f);
                    }
                    // Horizontal
                    else
                    {
                        temp.transform.localPosition += new Vector3(Posx + 110f * i, Posy + 110f * (j - Indy), 0f);
                        if(j == m_Pointer.Value.Answer.Length)
                        {
                            //string tempindex = tempindex = m_Pointer.Value.IndexInfo[0];
                            //var index = tempindex.Split(' ');
                            //Indx = int.Parse(index[0]);
                            //Indy = int.Parse(index[1]);
                        }
                    }
                    temp.name = m_Pointer.Value.Answer + j;
                }
                m_Pointer = m_Pointer.Next;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
