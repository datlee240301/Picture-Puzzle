using System;
using System.Collections;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    [SerializeField] private int id;
    int x = 0;
    int y = 0;
    private Action<int, int> swapFunc = null;
    public bool enableTouch = true;
    public bool isStatic = false;
    public event Action<NumberBox> OnBoxSelected;
    public bool isSelected = false;

    public void Init(int i, int j, int id, Sprite sprite, Action<int, int> swapFunc)
    {
        this.id = id;
        if (sprite != null)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        UpdatePos(i, j, true);
        this.swapFunc = swapFunc;
    }

    public bool getEnableTouch() { return enableTouch; }
    public void setEnableTouch(bool value) { enableTouch = value; }

    public void UpdatePos(int i, int j, bool instant = false)
    {
        x = i;
        y = j;
        if (instant)
        {
            this.gameObject.transform.position = new Vector2(i, j);
        }
        else
        {
            StartCoroutine(SmoothMove(new Vector2(i, j)));
        }
    }

    public bool IsEmpty()
    {
        int[] validLevels = { 1, 2, 3, 4, 5 };
        int[] validLevels1 = { 6, 7, 8, 9, 10 };
        if (Array.Exists(validLevels, level => level == PlayerPrefs.GetInt(StringManager.levelSelect)))
        {
            return id == 18;
        }
        else if (Array.Exists(validLevels1, level => level == PlayerPrefs.GetInt(StringManager.levelSelect)))
        {
            return id == 32;
        }
        return false;
    }

    public int GetId()
    {
        return id;
    }

    private void OnMouseDown() {
        if (enableTouch && !isStatic) {
            Puzzle puzzle = FindObjectOfType<Puzzle>();
            if (puzzle.swapMode) {
                OnBoxSelected?.Invoke(this);
            } else if (swapFunc != null) {
                swapFunc(x, y);
            }
            //FindObjectOfType<UiManager>().MinusStep();
        }
    }

    private IEnumerator SmoothMove(Vector2 targetPos)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;
        Vector2 startingPos = transform.position;
        FindObjectOfType<SoundManager>().PlayMoveBoxSound();
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startingPos, targetPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    private void Update()
    {
        if (FindObjectOfType<Puzzle>().winPanel.activeSelf || FindObjectOfType<Puzzle>().lostPanel.activeSelf)
        {
            FindObjectOfType<UiPlaySceneManager>().HideObject();
            enableTouch = false;
        }
        else
            enableTouch = true;
    }
}
