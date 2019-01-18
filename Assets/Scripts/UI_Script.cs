using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_Script : MonoBehaviour
{


    public int NowIndex;

    public Text[] mission_texts = new Text[3];

    public void Go_Arcade_Mode()
    {
        //SceneManager.LoadScene("SEXBOY");
        Debug.Log("GO_ARCADE_MODE");
    }

    public void Go_VS_Computer_Mode()
    {
        Debug.Log("GO_COMPUTER_MODE");
    }

    public void Go_TimeAttack_Mode()
    {
        Debug.Log("GO_TIMEATTACK_MODE");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Select_Mission(int index)
    {
        NowIndex = index;
        switch (index)
        {
            case 1:
                mission_texts[0].text = "mission 1 text 0";
                mission_texts[1].text = "mission 1 text 1";
                mission_texts[2].text = "mission 1 text 2";
                break;
            case 2:
                mission_texts[0].text = "mission 2 text 0";
                mission_texts[1].text = "mission 2 text 1";
                mission_texts[2].text = "mission 2 text 2";
                break;
            case 3:
                mission_texts[0].text = "mission 3 text 0";
                mission_texts[1].text = "mission 3 text 1";
                mission_texts[2].text = "mission 3 text 2";
                break;
            case 4:
                mission_texts[0].text = "mission 4 text 0";
                mission_texts[1].text = "mission 4 text 1";
                mission_texts[2].text = "mission 4 text 2";
                break;
            case 5:
                mission_texts[0].text = "mission 5 text 0";
                mission_texts[1].text = "mission 5 text 1";
                mission_texts[2].text = "mission 5 text 2";
                break;
        }

    }

    public void Start_Mission()
    {
        Singleton.Instance.selectLevel = NowIndex - 1;
        switch (NowIndex)
        {
            case 1:
                Debug.Log("START_MISSION1111111111111");

                break;
            case 2:
                Debug.Log("START_MISSION22222222222");

                break;
            case 3:
                Debug.Log("START_MISSION333333333333");

                break;
            case 4:
                Debug.Log("START_MISSION444444444444");

                break;
            case 5:
                Debug.Log("START_MISSION5555555555");

                break;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

    }

    public void Start_TimeAttack()
    {
        Debug.Log("START_TIMEATTACK");
    }


}
