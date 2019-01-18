using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Range(0, 1)]
    public float value;
    public Material TransitionMaterial;
    //Sprite 
    public Sprite selectSprite;
    public Sprite[] block;

    //블럭
    private const float blockSize = 1.875f;

    [Header("Player")]
    public GameObject player;
    private Vector2Int playerPosition;
    private LineRenderer line;
    public ParticleSystem lineEffect;
    //필드
    private Vector2 position = new Vector2(-300, 64);
    public Vector2Int filedSize = new Vector2Int(5, 14);
    private GameObject[,] filedBlock;
    public int[,] filed;
    private bool[,] filedSelect;
    private int[] list;
    //입력
    private Vector2 begin;
    private Vector2 end;
    Color[,] colors = new Color[6, 2]
    {
        { Color.red, Color.white },
        { Color.yellow, Color.white },
        { Color.green, Color.white },
        { Color.blue, Color.white },
        { Color.magenta, Color.white },
        { Color.gray, Color.white }
    };

    //UI
    private int selectIndex;
    private Vector2Int[] selectPoint;
    public GameObject selectBlock;
    private bool clear = false;
    private bool playGame;
    private int stack = 0;
    private int combo;
    private float time;

    public Image game_bg;

    [Header("Panel")]
    public GameObject maskFiled;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject lostPanel;
    [Header("Text")]
    public Text comboText;
    public Text timeText;


    public bool isEnd = false;

    private void Start()
    {
        list = new int[3];
        playGame = true;
        line = player.GetComponent<LineRenderer>();
        StartGame();
        LoadStage();
        StartCoroutine("StartAnimation");
    }
    private void LoadStage()
    {
        // TextAsset text = Resources.Load<TextAsset>("Stage_" + Singleton.Instance.selectLevel);
        TextAsset text = Resources.Load<TextAsset>("Stage_" + 0);

        string[] lines = text.text.Split('\n');
        for (int y = 0; y < filedSize.y; y++)
        {
            for (int x = 0; x < filedSize.x; x++)
            {
                int index = int.Parse(lines[filedSize.y - y - 1][x].ToString());
                SetBlock(x, y, index);
            }
        }

    }
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            playGame = !playGame;
            pausePanel.SetActive(!playGame);
        }
        if (playGame)
        {
            comboText.text = "Combo : " + combo;
            timeText.text = "Time : " + (int)time;
            time += Time.deltaTime;
            MouseClick();
        }

    }
    private void LateUpdate()
    {
        line.SetPosition(1, player.transform.position);

        line.SetPosition(0, filedBlock[filedSize.y-1, playerPosition.x].transform.position);


    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        TransitionMaterial.SetFloat("_Cutoff", value);
        Graphics.Blit(source, destination, TransitionMaterial);
    }
    private void MouseClick()
    {
        player.transform.position = filedBlock[playerPosition.y, playerPosition.x].transform.position;
        if (Input.GetMouseButtonDown(0))
            begin = Input.mousePosition;
        else if (Input.GetMouseButtonUp(0))
        {
            end = Input.mousePosition;
            Vector2 diffend = end - begin;
            if (diffend.magnitude > 30 && Mathf.Abs(diffend.x) > Mathf.Abs(diffend.y))
            {
                if (diffend.x > 0)
                {
                    Debug.Log("오른쪽");
                    playerPosition.x += 1;
                }
                else
                {
                    Debug.Log("왼쪽");

                    playerPosition.x -= 1;
                }
            }
            else
            {
                if (diffend.y > 0)
                {
                    ClickBlock(playerPosition.x, 0);
                }

                else Debug.Log("아래쪽");

            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {

            playerPosition.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            playerPosition.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ClickBlock(playerPosition.x, 0);
        }
        playerPosition.x = Mathf.Clamp(playerPosition.x, 0, 4);

    }
    private void StartGame()
    {
        filed = new int[filedSize.y, filedSize.x];
        filedSelect = new bool[filedSize.y, filedSize.x];
        filedBlock = new GameObject[filedSize.y, filedSize.x];
        selectPoint = new Vector2Int[3];
        playerPosition = new Vector2Int(2, 0);

        GameObject blocks = new GameObject("Blocks");
        GameObject selects = new GameObject("Selects");
        selects.AddComponent<SpriteRenderer>().sprite = selectSprite;

        var offset = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height) - position);

        for (int y = 0; y < filedSize.y; y++)
        {
            for (int x = 0; x < filedSize.x; x++)
            {
                var curBlock = filedBlock[y, x] = new GameObject("Block_" + x.ToString() + "," + y.ToString());
                curBlock.transform.parent = blocks.transform;
                curBlock.transform.position = Vector3.Scale(block[0].bounds.size, new Vector3(blockSize * x, (blockSize / 2 + blockSize * y))) - offset;
                curBlock.transform.localScale = Vector3.one * blockSize;

                var spriteRenderer = curBlock.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = null;
                spriteRenderer.sortingOrder = 100;
                spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            }
        }
        selects.transform.position = (filedBlock[0, 0].transform.position + filedBlock[filedSize.y - 1, filedSize.x - 1].transform.position) / 2;
        selects.transform.localScale = Vector3.one * blockSize;

        var selectsSpriteRenderer = selects.GetComponent<SpriteRenderer>();
        selectsSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        selectsSpriteRenderer.size = new Vector2(selectSprite.bounds.size.x * filedSize.x, selectSprite.bounds.size.y * filedSize.y);
        selectsSpriteRenderer.sortingOrder = -25;

        maskFiled.transform.position = filedBlock[filedSize.y/2, filedSize.x/2].transform.position - new Vector3(0, block[0].bounds.size.y*blockSize) / 2;
        maskFiled.transform.localScale = Vector2.Scale((block[0].bounds.size * blockSize), filedSize);



        RectTransform gamebackgroundRectTransform = game_bg.GetComponent<RectTransform>();
        gamebackgroundRectTransform.pivot = Vector2.zero;
        gamebackgroundRectTransform.localPosition = Camera.main.WorldToScreenPoint(filedBlock[0, 0].transform.position - (block[0].bounds.size * blockSize)/2) - new Vector3(Screen.width,Screen.height)/2;
        gamebackgroundRectTransform.sizeDelta = Camera.main.WorldToScreenPoint(Vector2.Scale((block[0].bounds.size * blockSize), filedSize)) - new Vector3(Screen.width, Screen.height) / 2; 
    }
    private void ClickBlock(int x, int y) //////
    {
        for (int i = y; i < filedSize.y; i++)
        {

            if (filed[i, x] != 0 && filedSelect[i, x] == false)
            {
                SoundManager.Instance.PlayEffect("Hit");
                Debug.Log(string.Format("{0},{1}", x, i));
                StartCoroutine(EnableBlock(x, i));
                list[selectIndex] = filed[i, x];
                filedSelect[i, x] = true;
                selectIndex += 1;
                /////중화가 추가해준 코드
                SetEffectColor(filed[i,x]);
                /////
                RemoveBlock(x, i);
                break;
            }
        }
        if (selectIndex == 3)
        {
            selectIndex = 0;
            clear = false;

            if (CheckSelectBlock())
            {
                combo += 1;
                for (int i = 0; i < filedSize.y; i++)
                {
                    for (int j = 0; j < filedSize.x; j++)
                    {
                        if (filed[i, j] != 0)
                        {
                            return;
                        }
                    }
                }
                isEnd = true;
                winPanel.SetActive(true);
                Debug.Log("All Clear");
            }
            else
            {
                lostPanel.SetActive(true);
            }
        }

    }

   

    private bool CheckSelectBlock()
    {
        return (list[0] == list[1] && list[1] == list[2] && list[2] == list[0]);
    }
    private void SetBlock(int x, int y, int number)
    {
        if (number != 0)
        {
            filed[y, x] = number;
            filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = block[number - 1];
        }
        else
        {

            filed[y, x] = 0;
            filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    public void RemoveBlock(int x, int y)
    {
        filedBlock[y, x].GetComponent<SpriteRenderer>().sprite = null;
        filed[y, x] = 0;
    }
    IEnumerator EnableBlock(int x, int y)
    {
        int index = selectIndex;
        float timer = -stack * 0.20f;
        GameObject image = selectBlock.transform.GetChild(index).GetChild(0).gameObject;
        GameObject sprite = new GameObject();
        Vector3 pos = Camera.main.ScreenToWorldPoint(selectBlock.transform.GetChild(index).localPosition + new Vector3(Screen.width, Screen.height) / 2);
        pos.z = 0;
        sprite.AddComponent<SpriteRenderer>().sprite = filedBlock[y, x].GetComponent<SpriteRenderer>().sprite;
        sprite.transform.localScale = Vector3.one * blockSize;
        sprite.transform.position = filedBlock[y, x].transform.position;

        image.GetComponent<Image>().sprite = filedBlock[y, x].GetComponent<SpriteRenderer>().sprite;
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            sprite.transform.position = Vector3.Lerp(filedBlock[y, x].transform.position, pos, timer);
            sprite.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, Vector3.forward * 360, timer);
            Debug.Log(filedBlock[y, x].transform.position);
            yield return null;
        }
        Destroy(sprite);
        stack += 1;
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        if (stack == 3)
        {
            stack = 0;
            for (int i = 0; i < 3; i++)
            {
                GameObject a = selectBlock.transform.GetChild(i).GetChild(0).gameObject;
                a.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator StartAnimation()
    {
        float timer = 1;
        while (timer > 0)
        {
            value = timer;
            timer -= Time.deltaTime / 2;
            timer = Mathf.Clamp01(timer);
            yield return null;
        }
    }
    IEnumerator EndAnimation(int num)
    {
        float timer = 0;
        while (timer < 1)
        {
            value = timer;
            timer += Time.deltaTime / 2;
            timer = Mathf.Clamp01(timer);
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);

    }
    public void CallMenu()
    {
        StartCoroutine(EndAnimation(0));
    }
    public void CallReGame()
    {
        StartCoroutine(EndAnimation(Application.loadedLevel));
    }
    public void CallNextGame()
    {
        Singleton.Instance.selectLevel += 1;
        StartCoroutine(EndAnimation(Application.loadedLevel));
    }
    public void SetEffectColor(int num)
    {
        int y;
        num -= 1;
        var lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.startColor = colors[num, 0];
        lineRenderer.endColor = colors[num, 1];

        var image = game_bg.GetComponent<Image>();
        image.color = colors[num,0] * new Color(1f, 1f, 1f, 100f / 255f);

        for (y = 0; y < filedSize.y - 1; y++)
        {
            if (filed[y + 1, playerPosition.x] != 0)
                break;
        }

        var sh = lineEffect.shape;
        lineEffect.gameObject.transform.position = (filedBlock[0, playerPosition.x].transform.position);

        var a = lineEffect.startColor;
        a = colors[num, 0];
        lineEffect.startColor = a;
        lineEffect.Play();
    }
    //////////


}
