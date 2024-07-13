using System.Collections;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;
    public NumberBox[,] boxes;
    public int mapId;
    public GameObject winPanel;
    public GameObject lostPanel;
    public GameObject[] stars;
    bool hasSetStar = false;
    public bool swapMode = false; // Biến công khai để kiểm tra chế độ hoán đổi

    // Để giữ nguyên public sprite arrays
    public Sprite[] sprites1;
    public Sprite[] sprites2;
    public Sprite[] sprites3;
    public Sprite[] sprites4;
    public Sprite[] sprites5;
    public Sprite[] sprites6;
    public Sprite[] sprites7;
    public Sprite[] sprites8;
    public Sprite[] sprites9;
    public Sprite[] sprites10;
    public Sprite[] sprites11;
    public Sprite[] sprites12;
    public Sprite[] sprites13;
    public Sprite[] sprites14;
    public Sprite[] sprites15;

    private NumberBox selectedBox1;
    private NumberBox selectedBox2;
    [SerializeField] GameObject[] moveButtons;
    [SerializeField] GameObject noticePanel;

    void Start()
    {
        Init();
    }

    void Init()
    {
        mapId = PlayerPrefs.GetInt(StringManager.levelSelect);
        switch (mapId)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                CreateMap(3, 6, 18, 2);
                //PlayerPrefs.SetInt(StringManager.layout1, 1);
                //PlayerPrefs.SetInt(StringManager.layout2, 0);
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                CreateMap(4, 8, 32, 3);
                //PlayerPrefs.SetInt(StringManager.layout1, 0);
                //PlayerPrefs.SetInt(StringManager.layout2, 1);
                break;
            default:
                Debug.LogError("Invalid mapId");
                break;
        }
    }

    void CreateMap(int width, int height, int numElements, int nonMovableCount)
    {
        boxes = new NumberBox[width, height];
        int[] movableIndices = new int[numElements - nonMovableCount]; // Số lượng phần tử có thể di chuyển

        for (int i = 0; i < numElements - nonMovableCount; i++)
        {
            movableIndices[i] = i + nonMovableCount + 1; // ID bắt đầu từ nonMovableCount + 1 và tăng dần
        }
        ShuffleArray(movableIndices);

        Sprite[][] spriteSets = new Sprite[][] {
        sprites1, sprites2, sprites3, sprites4, sprites5,
        sprites6, sprites7, sprites8, sprites9, sprites10,
        sprites11, sprites12, sprites13, sprites14, sprites15
    };

        if (mapId < 1 || mapId > spriteSets.Length)
        {
            Debug.LogError("Invalid mapId");
            return;
        }

        Sprite[] selectedSprites = spriteSets[mapId - 1];
        int n = 0;

        // Số lượng ô cố định ở bên phải
        int rightNonMovableCount = Mathf.Min(nonMovableCount, width);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == height - 1 && x >= width - rightNonMovableCount) // Kiểm tra ô cố định
                {
                    int id = x - (width - rightNonMovableCount) + 1; // ID cho các ô cố định bắt đầu từ 1 và tăng dần từ phải sang trái
                    NumberBox box = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity);
                    box.Init(x, y, id, null, ClickToSwap); // Không truyền sprite vào
                    box.isStatic = true; // Đặt thuộc tính isStatic thành true cho ô cố định
                    boxes[x, y] = box;
                }
                else if (n < movableIndices.Length)
                {
                    int index = movableIndices[n];
                    NumberBox box = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity);
                    box.Init(x, y, index, selectedSprites[index - 1], ClickToSwap);
                    box.OnBoxSelected += OnBoxSelected; // Đăng ký sự kiện khi box được chọn
                    boxes[x, y] = box;
                    n++;
                }
                else
                {
                    boxes[x, y] = null;
                }
            }
        }
    }



    public void ToggleSwapMode()
    {
        if (PlayerPrefs.GetInt(StringManager.diamondNumber) > 0)
        {
            //FindObjectOfType<UiManager>().MinusDiamond();
            if (moveButtons[0].activeSelf)
            {
                moveButtons[0].SetActive(false);
                moveButtons[1].SetActive(true);
            }
            else
            {
                moveButtons[0].SetActive(true);
                moveButtons[1].SetActive(false);
            }

            swapMode = !swapMode;
            if (!swapMode)
            {
                selectedBox1 = null;
                selectedBox2 = null;
                ResetBoxSelection();
            }

            foreach (var box in boxes)
            {
                if (box != null)
                {
                    box.setEnableTouch(!swapMode);
                }
            }
        }
        else
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        noticePanel.SetActive(true);
        StartCoroutine(HidePanel(noticePanel));
    }

    void ResetBoxSelection()
    {
        FindObjectOfType<UiPlaySceneManager>().MinusDiamond();
        foreach (var box in boxes)
        {
            if (box != null)
            {
                box.isSelected = false;
            }
        }
    }

    IEnumerator HidePanel(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }

    void OnBoxSelected(NumberBox selectedBox)
    {
        if (!swapMode || selectedBox.isSelected) return;

        if (selectedBox1 == null)
        {
            selectedBox1 = selectedBox;
            selectedBox.isSelected = true;
            StartCoroutine(ShrinkAndSwap(selectedBox1));
        }
        else if (selectedBox2 == null)
        {
            selectedBox2 = selectedBox;
            selectedBox.isSelected = true; // Đánh dấu box đã được chọn
            StartCoroutine(ShrinkAndSwap(selectedBox2));
        }
    }

    IEnumerator ShrinkAndSwap(NumberBox box)
    {
        // Thu nhỏ box
        float shrinkScale = 0.8f; // Tỉ lệ thu nhỏ
        Vector3 originalScale = box.transform.localScale;

        box.transform.localScale = originalScale * shrinkScale;

        // Chờ 0.2 giây để box thu nhỏ
        yield return new WaitForSeconds(0.2f);

        // Kiểm tra xem đã có cả hai box được chọn chưa
        if (selectedBox1 != null && selectedBox2 != null)
        {
            // Hoán đổi vị trí
            yield return StartCoroutine(SwapBoxes(selectedBox1, selectedBox2));
            selectedBox1 = null;
            selectedBox2 = null;
            ToggleSwapMode(); // Tắt chế độ hoán đổi sau khi hoán đổi xong
            moveButtons[0].SetActive(true);
            moveButtons[1].SetActive(false);
        }

        // Phóng to lại box sau khi hoàn thành hoặc nếu không có box thứ hai được chọn
        box.transform.localScale = originalScale;
    }

    IEnumerator SwapBoxes(NumberBox box1, NumberBox box2)
    {
        FindObjectOfType<SoundManager>().PlayExchangeSound();
        Vector2 pos1 = box1.transform.position;
        Vector2 pos2 = box2.transform.position;

        // Thu nhỏ box trước khi hoán đổi
        float shrinkScale = 0.8f; // Tỉ lệ thu nhỏ
        Vector3 originalScale1 = box1.transform.localScale;
        Vector3 originalScale2 = box2.transform.localScale;

        box1.transform.localScale = originalScale1 * shrinkScale;
        box2.transform.localScale = originalScale2 * shrinkScale;

        // Hoán đổi vị trí
        float elapsedTime = 0f;
        float duration = 0.5f; // Thời gian để hoán đổi vị trí

        while (elapsedTime < duration)
        {
            box1.transform.position = Vector2.Lerp(pos1, pos2, elapsedTime / duration);
            box2.transform.position = Vector2.Lerp(pos2, pos1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Phóng to lại box sau khi hoán đổi
        box1.transform.localScale = originalScale1;
        box2.transform.localScale = originalScale2;

        // Hoán đổi vị trí trong mảng boxes
        int x1 = Mathf.RoundToInt(pos1.x);
        int y1 = Mathf.RoundToInt(pos1.y);
        int x2 = Mathf.RoundToInt(pos2.x);
        int y2 = Mathf.RoundToInt(pos2.y);

        NumberBox temp = boxes[x1, y1];
        boxes[x1, y1] = boxes[x2, y2];
        boxes[x2, y2] = temp;

        // Cập nhật vị trí mới cho box
        box1.UpdatePos(x2, y2, true);
        box2.UpdatePos(x1, y1, true);
    }



    private void Update()
    {
        if (CheckWinCondition() && !hasSetStar)
        {
            FindObjectOfType<UiPlaySceneManager>().PlusDiamond(4);
            winPanel.SetActive(true);
            SetNumbersOfStars();
            FindObjectOfType<NumberBox>().enableTouch = false;
            FindObjectOfType<SoundManager>().PlayWinSound();
        }
        FindObjectOfType<UiPlaySceneManager>().UpdateLevelText();
    }

    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        from.UpdatePos(x + dx, y + dy);
        if (target != null)
        {
            target.UpdatePos(x, y);
        }
        if (winPanel.activeSelf) SetNumbersOfStars();
        if (CheckWinCondition())
        {
            winPanel.SetActive(true);
            GetComponentInChildren<NumberBox>().setEnableTouch(false);
        }
    }

    int getDx(int x, int y)
    {
        if (x < boxes.GetLength(0) - 1 && (boxes[x + 1, y] == null || boxes[x + 1, y].IsEmpty()))
        {
            return 1;
        }
        if (x > 0 && (boxes[x - 1, y] == null || boxes[x - 1, y].IsEmpty()))
        {
            return -1;
        }
        return 0;
    }

    int getDy(int x, int y)
    {
        if (y < boxes.GetLength(1) - 1 && (boxes[x, y + 1] == null || boxes[x, y + 1].IsEmpty()))
        {
            return 1;
        }
        if (y > 0 && (boxes[x, y - 1] == null || boxes[x, y - 1].IsEmpty()))
        {
            return -1;
        }
        return 0;
    }

    bool CheckWinCondition()
    {
        int width = boxes.GetLength(0);
        int height = boxes.GetLength(1);

        if (mapId >= 1 && mapId <= 5)
        {
            // Layout 1 - 5: Kiểm tra theo thứ tự 18, 1, 2, ..., 17 từ trên xuống dưới, từ trái qua phải
            int expectedId = 18;

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (boxes[x, y] != null && boxes[x, y].GetId() != expectedId)
                    {
                        return false;
                    }
                    expectedId = (expectedId % 18) + 1; // Quay lại 1 sau khi đạt tới 18
                }
            }
        }
        else if (mapId >= 6 && mapId <= 10)
        {
            // Layout 6 - 10: Kiểm tra theo thứ tự 32, 1, 2, ..., 31 từ trên xuống dưới, từ trái qua phải
            int expectedId = 32;

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (boxes[x, y] != null && boxes[x, y].GetId() != expectedId)
                    {
                        return false;
                    }
                    expectedId = (expectedId % 32) + 1; // Quay lại 1 sau khi đạt tới 32
                }
            }
        }
        else
        {
            Debug.LogError("Invalid mapId");
            return false;
        }

        return true;
    }

    void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }
    }

    void SetNumbersOfStars()
    {
        int secondCount = FindObjectOfType<RealTimeCounter>().secondsCount;
        int starsCount = 0;
        if (secondCount <= 10)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            starsCount = 3;
        }
        else if (secondCount <= 20)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            starsCount = 2;
        }
        else if (secondCount <= 30)
        {
            stars[0].SetActive(true);
            starsCount = 1;
        }

        int currentStarsCount = PlayerPrefs.GetInt(StringManager.level1Stars, 0);
        if (starsCount > currentStarsCount)
        {
            PlayerPrefs.SetInt(StringManager.level1Stars, starsCount);
        }

        switch (mapId)
        {
            case 1:
                UpdateStarCount(StringManager.level1Stars, starsCount);
                break;
            case 2:
                UpdateStarCount(StringManager.level2Stars, starsCount);
                break;
            case 3:
                UpdateStarCount(StringManager.level3Stars, starsCount);
                break;
            case 4:
                UpdateStarCount(StringManager.level4Stars, starsCount);
                break;
            case 5:
                UpdateStarCount(StringManager.level5Stars, starsCount);
                break;
            case 6:
                UpdateStarCount(StringManager.level6Stars, starsCount);
                break;
            case 7:
                UpdateStarCount(StringManager.level7Stars, starsCount);
                break;
            case 8:
                UpdateStarCount(StringManager.level8Stars, starsCount);
                break;
            case 9:
                UpdateStarCount(StringManager.level9Stars, starsCount);
                break;
            case 10:
                UpdateStarCount(StringManager.level10Stars, starsCount);
                break;
            case 11:
                UpdateStarCount(StringManager.level11Stars, starsCount);
                break;
            case 12:
                UpdateStarCount(StringManager.level12Stars, starsCount);
                break;
            case 13:
                UpdateStarCount(StringManager.level13Stars, starsCount);
                break;
            case 14:
                UpdateStarCount(StringManager.level14Stars, starsCount);
                break;
            case 15:
                UpdateStarCount(StringManager.level15Stars, starsCount);
                break;
            default:
                break;
        }
        SaveLevel();
        hasSetStar = true;
    }

    void UpdateStarCount(string levelKey, int newStarsCount)
    {
        int currentStarsCount = PlayerPrefs.GetInt(levelKey, 0);
        if (newStarsCount > currentStarsCount)
        {
            PlayerPrefs.SetInt(levelKey, newStarsCount);
        }
    }

    void SaveLevel()
    {
        int nextLevel = mapId + 1;
        PlayerPrefs.SetInt(StringManager.currentLevel, nextLevel);
    }
}
