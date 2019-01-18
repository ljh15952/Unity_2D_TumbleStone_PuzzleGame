//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    [Range(0, 1)]
//    public float value;
//    public Material TransitionMaterial;
//    //Sprite 
//    public Sprite selectSprite;
//    public Sprite[] block;

//    //블럭
//    private const float blockSize = 1.875f;

//    [Header("Player")]
//    public GameObject player;
//    private Vector2Int playerPosition;
//    private LineRenderer line;

//    //필드
//    private Vector2 position =new Vector2(-300,64);
//    private Vector2Int filedSize = new Vector2Int(5, 14);
//    private GameObject[,] filedBlock;
//    private int[,] filed;
//    private int[] list;
//    //입력
//    private Vector2 begin;
//    private Vector2 end;

//    //UI
//    private int selectIndex;
//    private Vector2Int[] selectPoint;
//    public GameObject selectBlock;
//    private bool clear = false;
//    private int stack = 0;

//    [Header("Panel")]
//    public GameObject winPanel;
//    public GameObject lostPanel;

//    private void Start()
//    {
//        list = new int[3];
//        line = player.GetComponent<LineRenderer>();
//        StartGame();
//        LoadStage();
//        StartCoroutine("StartAnimation");

//    }
//    private void Update()
//    {
//        MouseClick();


//    }
//    private void LateUpdate()
//    {
//        line.SetPosition(1, player.transform.position);
//        line.SetPosition(0, filedBlock[filedSize.y- 1, playerPosition.x].transform.position);

//    }

//    private void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        TransitionMaterial.SetFloat("_Cutoff", value);
//        Graphics.Blit(source, destination, TransitionMaterial);
//    }
//    private void MouseClick()
//    {
//        player.transform.position = filedBlock[playerPosition.y, playerPosition.x].transform.position;
//        if (Input.GetMouseButtonDown(0))
//            begin = Input.mousePosition;
//        else if (Input.GetMouseButtonUp(0))
//        {
//            end = Input.mousePosition;
//            Vector2 diffend = end - begin;
//            if (diffend.magnitude > 100 && Mathf.Abs(diffend.x) > Mathf.Abs(diffend.y))
//            {
//                if (diffend.x > 0)
//                {
//                    Debug.Log("오른쪽");
//                    playerPosition.x += 1;
//                }
//                else
//                {
//                    Debug.Log("왼쪽");

//                    playerPosition.x -= 1;
//                }
//            }
//            else if (diffend.magnitude > 100)
//            {
//                if (diffend.y > 0)
//                {
//                    ClickBlock(playerPosition.x, 0);
//                }

//                else Debug.Log("아래쪽");

//            }
//        }
//        playerPosition.x = Mathf.Clamp(playerPosition.x, 0, 4);

//    }
//    private void StartGame()
//    {
//        filed = new int[filedSize.y, filedSize.x];
//        filedBlock = new GameObject[filedSize.y, filedSize.x];
//        selectPoint = new Vector2Int[3];
//        playerPosition = new Vector2Int(2, 0);

//        GameObject blocks = new GameObject("Blocks");
//        GameObject selects = new GameObject("Selects");
//        selects.AddComponent<SpriteRenderer>().sprite = selectSprite;
//        for (int y = 0; y < filedSize.y; y++)
//        {
//            for (int x = 0; x < filedSize.x; x++)
//            {
//                filedBlock[y, x] = new GameObject("Block_" + x.ToString() + "," + y.ToString());
//                filedBlock[y, x].transform.parent = blocks.transform;
//                filedBlock[y, x].transform.position = new Vector3(
//                    block[0].bounds.size.x * blockSize * x,
//                    block[0].bounds.size.y * (blockSize / 2 + blockSize * y))
//                    - Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height) - position);
//                filedBlock[y, x].transform.localScale = Vector3.one * blockSize;
//                filedBlock[y, x].AddComponent<SpriteRenderer>().sprite = null;
//            }
//        }
//        selects.transform.localScale = Vector3.one * blockSize;
//        selects.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
//        selects.GetComponent<SpriteRenderer>().size = new Vector2(selectSprite.bounds.size.x * filedSize.x, selectSprite.bounds.size.y * filedSize.y);
//        selects.GetComponent<SpriteRenderer>().sortingOrder=-25;
//        selects.transform.position = (filedBlock[0, 0].transform.position + filedBlock[filedSize.y - 1, filedSize.x -1].transform.position) / 2;
//    }

//    private void LoadStage()
//    {
//        TextAsset text = Resources.Load<TextAsset>("Stage_"+Singleton.Instance.selectLevel);
//        string[] lines = text.text.Split('\n');
//        for (int y = 0; y < filedSize.y; y++)
//        {
//            for (int x = 0; x < filedSize.x; x++)
//            {
//                int index = int.Parse(lines[filedSize.y - y - 1][x].ToString());
//                SetBlock(x, y, index);
//            }
//        }

//    }
//    private void ClickBlock(int x, int y)
//    {
//        for (int i = y; i < filedSize.y; i++)
//        {

//            if (filed[i, x] != 0 )
//            {
//                SoundManager.Instance.PlayEffect("Hit");
//                Debug.Log(string.Format("{0},{1}", x, i));
//                StartCoroutine(EnableBlock(x, i));
//                list[selectIndex] = filed[i, x];
//                RemoveBlock(x, i);
//                selectIndex += 1;
//                break;
//            }
//        }
//        if (selectIndex == 3)
//        {
//            selectIndex = 0;
//            clear = false;

//            if (CheckSelectBlock())
//            {
//                for (int i = 0; i < filedSize.y; i++)
//                {
//                    for (int j = 0; j < filedSize.x; j++)
//                    {
//                        if (filed[i, j] != 0)
//                        {
//                            return;
//                        }
//                    }
//                }
//                winPanel.SetActive(true);
//                Debug.Log("All Clear");
//            }
//            else
//            {
//                lostPanel.SetActive(true);
//            }
//        }

//    }
//    private bool CheckSelectBlock()
//    {
//        return (list[0] == list[1] && list[1] == list[2] && list[2] == list[0]);
//    }
//    private void SetBlock(int x, int y, int number)
//    {
//        if (number != 0)
//        {
//            filed[y, x] = number + 1;
//            filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = block[number + 1];
//        }
//        else
//        {

//            filed[y, x] = 0;
//            filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = null;
//        }
//    }
//    private void RemoveBlock(int x, int y)
//    {
//        filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = null;
//        filed[y, x] = 0;
//    }
//    IEnumerator EnableBlock(int x, int y)
//    {
//        int index = selectIndex;
//        float timer = -stack * 0.20f;
//        GameObject image = selectBlock.transform.GetChild(index).GetChild(0).gameObject;
//        GameObject sprite = new GameObject();
//        Vector3 pos = Camera.main.ScreenToWorldPoint(selectBlock.transform.GetChild(index).localPosition + new Vector3(Screen.width, Screen.height) / 2);
//        pos.z = 0;
//        sprite.AddComponent<SpriteRenderer>().sprite = filedBlock[y, x].GetComponent<SpriteRenderer>().sprite;
//        sprite.transform.localScale = Vector3.one * blockSize;
//        sprite.transform.position = filedBlock[y, x].transform.position;

//        image.GetComponent<Image>().sprite = filedBlock[y, x].GetComponent<SpriteRenderer>().sprite;
//        while (timer < 1)
//        {
//            timer += Time.deltaTime * 2;
//            sprite.transform.position = Vector3.Lerp(filedBlock[y, x].transform.position, pos, timer);
//            sprite.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, Vector3.forward * 360, timer);
//            Debug.Log(filedBlock[y, x].transform.position);
//            yield return null;
//        }
//        Destroy(sprite);
//        stack += 1;
//        image.gameObject.SetActive(true);
//        if (stack == 3)
//        {
//            stack = 0;
//            for (int i = 0; i < 3; i++)
//            {
//                GameObject a = selectBlock.transform.GetChild(i).GetChild(0).gameObject;
//                a.gameObject.SetActive(false);
//            }
//        }
//    }
//    IEnumerator StartAnimation()
//    {
//        float timer = 1;
//        while(timer > 0)
//        {
//            value = timer;
//            timer -= Time.deltaTime / 2;
//            timer = Mathf.Clamp01(timer);
//            yield return null;
//        }
//    }
//    IEnumerator EndAnimation(int num)
//    {
//        float timer = 0;
//        while (timer < 1)
//        {
//            value = timer;
//            timer += Time.deltaTime / 2;
//            timer = Mathf.Clamp01(timer);
//            yield return null;
//        }
//        UnityEngine.SceneManagement.SceneManager.LoadScene(num);

//    }


//    public void CallMenu()
//    {
//        StartCoroutine(EndAnimation(0));
//    }
//    public void CallReGame()
//    {
//        StartCoroutine(EndAnimation(Application.loadedLevel));
//    }
//    public void CallNextGame()
//    {
//        Singleton.Instance.selectLevel += 1;
//        StartCoroutine(EndAnimation(Application.loadedLevel));
//    }
//}
