using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 1:Red
 2:Yellow
 3:green
 4:blue
 5:purple
 6:grey
*/
/*
알고리즘
1.제일 가까운 블록 검사 
2.3개를 만족할시 차례차례터트림
3.2개를 만족할시 제일 가까운 블록에서 그 다음줄까지 검사
4.차례차례터트림
*/
//11222
//31433
//04240
//00200
//00200
//00000
//00000
//00000
//00000
//00000
//00000
//00000
//00000
//00000
public class AI_Manager : MonoBehaviour
{

    public GameManager GM;

    private Vector2Int lastIndex;

    public int[] Index_Array = new int[5];
    public List<int> Real_Array = new List<int>();

    bool isfound = false;

    public int[] Up_Array = new int[5];
    public int[] UpUp_Array = new int[5];
    public Vector2Int[] Release_index_Array = new Vector2Int[5];
    public Vector2Int[] Release_index_Array2 = new Vector2Int[5];

    // Use this for initialization
    public int ReleaseNum = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isfound)
            return;

        SetArray();
        isfound = true;


    }

    void SetArray()
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 14; i++)
            {
                if (GM.filed[i, j] != 0)
                {
                    Index_Array[j] = GM.filed[i, j];
                    Release_index_Array[j] = new Vector2Int(i, j);
                    if (i < 13)
                    {
                        Up_Array[j] = GM.filed[i + 1, j];
                    }
                    if (i < 12)
                        UpUp_Array[j] = GM.filed[i + 2, j];
                    break;
                }
            }
        }
       CheckIndex();
    }

    void CheckIndex()
    {
        for (int i = 0; i < 5; i++)
        {
            Real_Array.Clear();

            Real_Array.Add(Index_Array[i]);

            Release_index_Array2[ReleaseNum] = Release_index_Array[i];
            ReleaseNum++;

            for (int j = i + 1; j < 14; j++)
            {
                if (GM.filed[j, i] != 0)
                {
                    if (GM.filed[Release_index_Array[i].y, Release_index_Array[i].x] == GM.filed[Release_index_Array[j].y, Release_index_Array[j].x])
                    {
                        Release_index_Array2[0] = Release_index_Array[j];
                        ReleaseNum++;

                        if (ReleaseBlock())
                            return;
                    }
                }
            }

            //if (Index_Array[i] == Up_Array[i])
            //{
            //    Real_Array.Add(Up_Array[i]);
            //    if (ReleaseBlock())
            //        return;
            //    if (Up_Array[i] == UpUp_Array[i])
            //    {
            //        Real_Array.Add(UpUp_Array[i]);
            //        if (ReleaseBlock())
            //            return;
            //    }
            //}

        }




        //for (int i = 0; i < 4; i++)
        //{
        //    Real_Array.Clear();

        //    Real_Array.Add(Index_Array[i]);

        //    for (int j = i + 1; j < 5; j++)
        //    {
        //        if (Index_Array[i] == Index_Array[j])
        //        {
        //            Real_Array.Add(Index_Array[j]);
        //           // Release_index_Array[ReleaseNum] = new Vector2Int(, 0);

        //            if (ReleaseBlock())
        //              return;
        //        }
        //    }
        //    if (Index_Array[i] == Up_Array[i])
        //    {
        //        Real_Array.Add(Up_Array[i]);
        //        if (ReleaseBlock())
        //            return;
        //        if (Up_Array[i] == UpUp_Array[i])
        //        {
        //            Real_Array.Add(UpUp_Array[i]);
        //            if (ReleaseBlock())
        //                return;
        //        }
        //    }
        //}

    }

    bool ReleaseBlock()
    {
        if (Real_Array.Count > 2)
        {
      //      Debug.Log(GM.filed[Release_index_Array[Real_Array[0]].x, Release_index_Array[Real_Array[0]].y]);
       //     Debug.Log(GM.filed[Release_index_Array[Real_Array[0]].x+1, Release_index_Array[Real_Array[0]].y]);

            //if (Release_index_Array[Real_Array[0]].x + 1 < 14)
            //{
            //    if (GM.filed[Release_index_Array[Real_Array[0]].x, Release_index_Array[Real_Array[0]].y] == GM.filed[Release_index_Array[Real_Array[0]].x + 1, Release_index_Array[Real_Array[0]].y])
            //        GM.RemoveBlock(Release_index_Array[Real_Array[2]].y, Release_index_Array[Real_Array[2]].x + 1);
            //}
            //if (Release_index_Array[Real_Array[0]].x + 2<14)
            //{
            //    if (GM.filed[Release_index_Array[Real_Array[0]].x, Release_index_Array[Real_Array[0]].y] == GM.filed[Release_index_Array[Real_Array[0]].x + 2, Release_index_Array[Real_Array[0]].y])
            //        GM.RemoveBlock(Release_index_Array[Real_Array[2]].y, Release_index_Array[Real_Array[2]].x + 2);
            //}
            for (int i = 0; i < 3; i++)
            {
            //   GM.RemoveBlock(Release_index_Array[Real_Array[i]].y, Release_index_Array[Real_Array[i]].x);
                GM.RemoveBlock(Release_index_Array[i].y, Release_index_Array[i].x);

            }
            SetArray();
            return true;
        }
        else
            return false;
    }
}
