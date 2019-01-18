using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Difficulty_Select : MonoBehaviour {

    public List<GameObject> Texts = new List<GameObject>();
    int pivot;
    bool isanimated;

    public int NowDifficulty = 1;

    // Use this for initialization
    void Start()
    {
        isanimated = false;
        pivot = 1;
    }

    // Update is called once per frame

    public void PressUpArrow()
    {
        if (isanimated)
            return;
        StartCoroutine(TextUp());

    }

    public void PressDownArrow()
    {
        if (isanimated)
            return;
        StartCoroutine(TextDown());

    }

    IEnumerator TextUp()
    {
        if (NowDifficulty < 2)
            NowDifficulty++;
        else
            NowDifficulty = 0;

        float Timer = 0.5f;
        isanimated = true;

        Texts[pivot - 1].GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0);

        while (Timer >= 0)
        {
            Timer -= Time.deltaTime;

            Texts[pivot - 1].transform.localPosition = new Vector3(0, -100, 0);
            Texts[pivot - 1].GetComponent<RectTransform>().localScale += new Vector3(0.01f, 0.01f, 0);


            Texts[pivot].transform.Translate(new Vector3(0, 1.4f * 1.7f, 0));
            Texts[pivot].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);
            Texts[pivot].GetComponent<RectTransform>().localScale -= new Vector3(0.01f, 0.01f, 0);


            Texts[pivot + 1].transform.Translate(new Vector3(0, 1.4f * 1.7f, 0));
            Texts[pivot + 1].GetComponent<Text>().color += new Color32(0, 0, 0, 5);
            Texts[pivot + 1].GetComponent<RectTransform>().localScale += new Vector3(0.01f, 0.01f, 0);


            yield return new WaitForEndOfFrame();
        }
        Texts[3] = Texts[0];

        for (int i = 0; i < 3; i++)
        {
            Texts[i] = Texts[i + 1];
        }
        isanimated = false;
    }

    IEnumerator TextDown()
    {
        if (NowDifficulty > 0)
            NowDifficulty--;
        else
            NowDifficulty = 2;


        float Timer = 0.5f;
        isanimated = true;

        Texts[pivot + 1].GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0);

        while (Timer >= 0)
        {
            Timer -= Time.deltaTime;

            Texts[pivot + 1].transform.localPosition = new Vector3(0, 50, 0);
         //   Texts[pivot + 1].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);
            Texts[pivot + 1].GetComponent<RectTransform>().localScale += new Vector3(0.01f,0.01f, 1);


            Texts[pivot].transform.Translate(new Vector3(0, -1.4f * 1.7f, 0));
            Texts[pivot].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);
            Texts[pivot].GetComponent<RectTransform>().localScale -= new Vector3(0.01f, 0.01f, 1);

            Texts[pivot - 1].transform.Translate(new Vector3(0, -1.4f * 1.7f, 0));
            Texts[pivot - 1].GetComponent<Text>().color += new Color32(0, 0, 0, 5);
            Texts[pivot - 1].GetComponent<RectTransform>().localScale += new Vector3(0.01f, 0.01f, 0);


            Debug.Log("HI");

            yield return new WaitForEndOfFrame();



        }

        Texts[3] = Texts[2];

        for (int i = 3; i > 0; i--)
        {
            Texts[i] = Texts[i - 1];
        }

        for (int i = 0; i < 4; i++)
        {
          //  Texts[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }



        Texts[0] = Texts[3];
        isanimated = false;


      
    }


    public void Click_Start()
    {
        switch (NowDifficulty)
        {
            case 0:
                Debug.Log("LOAD_SCENE_EASY_COMPUTER_MODE");
                break;
            case 1:
                Debug.Log("LOAD_SCENE_NORMAL_COMPUTER_MODE");
                break;
            case 2:
                Debug.Log("LOAD_SCENE_HARD_COMPUTER_MODE");
                break;
        }
        
    }

}
