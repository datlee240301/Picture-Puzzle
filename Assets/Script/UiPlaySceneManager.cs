using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiPlaySceneManager : MonoBehaviour
{
    [SerializeField] GameObject[] starsOfLevel1;
    [SerializeField] GameObject[] starsOfLevel2;
    [SerializeField] GameObject[] starsOfLevel3;
    [SerializeField] GameObject[] starsOfLevel4;
    [SerializeField] GameObject[] starsOfLevel5;
    [SerializeField] GameObject[] starsOfLevel6;
    [SerializeField] GameObject[] starsOfLevel7;
    [SerializeField] GameObject[] starsOfLevel8;
    [SerializeField] GameObject[] starsOfLevel9;
    [SerializeField] GameObject[] starsOfLevel10;
    [SerializeField] GameObject[] starsOfLevel11;
    [SerializeField] GameObject[] starsOfLevel12;
    [SerializeField] GameObject[] starsOfLevel13;
    [SerializeField] GameObject[] starsOfLevel14;
    [SerializeField] GameObject[] starsOfLevel15;
    [SerializeField] GameObject[] blueLevelButtons;
    [SerializeField] GameObject[] levelPictures;
    [SerializeField] GameObject[] musicIcons;
    [SerializeField] GameObject noticePanel;
    [SerializeField] TextMeshProUGUI diamondNumberText;
    [SerializeField] TextMeshProUGUI diamondNumberInShopText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI stepsText;
    [SerializeField] TextMeshProUGUI levelWinText;
    [SerializeField] GameObject forbiddenBox;
    [SerializeField] GameObject forbiddenBox1;
    [SerializeField] RectTransform shopPanel;
    Vector2 firstPos;
    int diamondNumber;
    int stepsNumber = 10;

    public Camera mainCamera;
    private void Start()
    {


        diamondNumber = PlayerPrefs.GetInt(StringManager.diamondNumber);
        diamondNumberText.text = diamondNumber.ToString();
        FindObjectOfType<SoundManager>().StopBGMusic();
        levelText.text = "Level: " + PlayerPrefs.GetInt(StringManager.levelSelect).ToString();
        //stepsText.text = "Steps: " + stepsNumber.ToString();
        mainCamera = Camera.main;
        int levelId = PlayerPrefs.GetInt(StringManager.levelSelect);
        if (levelPictures.Length == 10)
        {
            if (levelId >= 1 && levelId <= 10)
            {
                levelPictures[levelId - 1].SetActive(true);
            }
        }
        Vector3 newPosition = mainCamera.transform.position;
        if (PlayerPrefs.GetInt(StringManager.layout2) == 1)
        {
            forbiddenBox.transform.position = new Vector3(3.66f, 6.72f, 1280.341f);
            forbiddenBox1.SetActive(true);
            Debug.Log("Layout 2");
            newPosition.y = 4.35f;
            newPosition.x = 1.41f;
            mainCamera.transform.position = newPosition;
            mainCamera.orthographicSize = 6.14f;
        }
    }

    private void Update()
    {
        //if(SceneManager.GetActiveScene().name == "PlayScene")
        //{
        //    if (FindObjectOfType<Puzzle>().winPanel.activeSelf)
        //    {
        //        PlusDiamond(4);
        //    }
        //}
    }

    public void PlusDiamond(int number)
    {
        int award = diamondNumber + number;
        diamondNumber = award;
        //diamondNumberText.text = diamondNumber.ToString();
        PlayerPrefs.SetInt(StringManager.diamondNumber, diamondNumber);
    }

    public void MinusStep()
    {
        //stepsNumber--;
        //stepsText.text = "Steps: " + stepsNumber.ToString();
        //if (stepsNumber == 0) {
        //    stepsNumber = 0;
        //    stepsText.text = "Steps: " + stepsNumber.ToString();
        //    StopCount();
        //    FindObjectOfType<Puzzle>().lostPanel.SetActive(true);
        //}
    }

    private void UpdateAllStars()
    {
        UpdateStars(PlayerPrefs.GetInt(StringManager.level1Stars), starsOfLevel1);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level2Stars), starsOfLevel2);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level3Stars), starsOfLevel3);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level4Stars), starsOfLevel4);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level5Stars), starsOfLevel5);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level6Stars), starsOfLevel6);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level7Stars), starsOfLevel7);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level8Stars), starsOfLevel8);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level9Stars), starsOfLevel9);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level10Stars), starsOfLevel10);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level11Stars), starsOfLevel11);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level12Stars), starsOfLevel12);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level13Stars), starsOfLevel13);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level14Stars), starsOfLevel14);
        UpdateStars(PlayerPrefs.GetInt(StringManager.level15Stars), starsOfLevel15);
    }

    private void UpdateStars(int starsCount, GameObject[] stars)
    {
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }
        for (int i = 0; i < starsCount; i++)
        {
            if (i < stars.Length)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //public void LevelButton(int levelId)
    //{
    //    if (levelId <= PlayerPrefs.GetInt(StringManager.currentLevel))
    //    {
    //        PlayerPrefs.SetInt(StringManager.levelSelect, levelId);
    //        if (levelId > 0 && levelId <= 5)
    //        {
    //            PlayerPrefs.SetInt(StringManager.layout2, 0);
    //        }
    //        else if (levelId > 5 && levelId <= 10)
    //        {
    //            PlayerPrefs.SetInt(StringManager.layout2, 1);
    //        }
    //        LoadScene("PlayScene");
    //    }
    //    else EnableObject(noticePanel);
    //}

    public void NextLevel()
    {
        if(PlayerPrefs.GetInt(StringManager.levelSelect) == 10) LoadScene("SelectLevelScene");
        if (PlayerPrefs.GetInt(StringManager.levelSelect) < 10)
        {
            int nextLevel = PlayerPrefs.GetInt(StringManager.levelSelect) + 1;
            PlayerPrefs.SetInt(StringManager.levelSelect, nextLevel);
            if (nextLevel > 0 && nextLevel <= 5)
            {
                PlayerPrefs.SetInt(StringManager.layout2, 0);
            }
            else if (nextLevel > 5 && nextLevel <= 10)
            {
                PlayerPrefs.SetInt(StringManager.layout2, 1);
            }
            LoadScene("PlayScene");
        }
    }

    public void StopCount()
    {
        FindObjectOfType<RealTimeCounter>().StopTimer();
    }

    public void StartCount()
    {
        FindObjectOfType<RealTimeCounter>().StartTimer();
    }

    void UpdateBlueLevelButton()
    {
        int currentLevel = PlayerPrefs.GetInt(StringManager.currentLevel);

        for (int i = 0; i < blueLevelButtons.Length; i++)
        {
            blueLevelButtons[i].SetActive(i >= currentLevel - 1);
        }
    }

    public void MusicButton()
    {
        if (PlayerPrefs.GetInt(StringManager.musicOn) == 0)
        {
            PlayerPrefs.SetInt(StringManager.musicOn, 1);
            musicIcons[0].SetActive(false);
            musicIcons[1].SetActive(true);
            FindObjectOfType<SoundManager>().PlayMusic();
        }
        else if (PlayerPrefs.GetInt(StringManager.musicOn) == 1)
        {
            PlayerPrefs.SetInt(StringManager.musicOn, 0);
            musicIcons[0].SetActive(true);
            musicIcons[1].SetActive(false);
            FindObjectOfType<SoundManager>().StopMusic();
        }
    }
    void ShowButtonStatus()
    {
        if (PlayerPrefs.GetInt(StringManager.musicOn) == 0)
        {
            musicIcons[0].SetActive(true);
            musicIcons[1].SetActive(false);
        }
        else if (PlayerPrefs.GetInt(StringManager.musicOn) == 1)
        {
            musicIcons[0].SetActive(false);
            musicIcons[1].SetActive(true);
        }
    }

    public void ShowPanel(RectTransform panel)
    {
        panel.anchoredPosition = new Vector2(0, 0);
    }

    public void HidePanel(RectTransform panel)
    {
        panel.anchoredPosition = firstPos;
    }

    public void BuyDiamond(int number)
    {
        int newNumber = diamondNumber + number;
        PlayerPrefs.SetInt(StringManager.diamondNumber, newNumber);
        diamondNumber = newNumber;
        diamondNumberText.text = diamondNumber.ToString();
        diamondNumberInShopText.text = diamondNumber.ToString();
    }

    public void MinusDiamond()
    {
        int newNumber = diamondNumber - 2;
        PlayerPrefs.SetInt(StringManager.diamondNumber, newNumber);
        diamondNumber = newNumber;
        diamondNumberText.text = PlayerPrefs.GetInt(StringManager.diamondNumber).ToString();
    }

    public void HideObject()
    {
        forbiddenBox.SetActive(false);
    }

    public void UpdateLevelText()
    {
        //levelWinText.text = "Level " + PlayerPrefs.GetInt(StringManager.levelSelect).ToString();
    }

    public IEnumerator DisableObject(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }
    public void EnableObject(GameObject obj)
    {
        obj.SetActive(true);
        StartCoroutine(DisableObject(noticePanel));
    }
}
