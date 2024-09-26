using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puzzle : MonoBehaviour {
    public NumberBox boxPrefabs;
    public NumberBox[,] boxes;
    public Sprite[] sprites1;
    public Sprite[] sprites2;

    public static int gridSize;
    private Sprite[] selectedSprites;
    public static bool isAlternateMode = false;

    private List<NumberBox> selectedBoxes = new List<NumberBox>();

    private void Start() {
        PlayerPrefs.SetInt(StringManager.layoutId, 0);
        int layoutId = PlayerPrefs.GetInt(StringManager.layoutId);

        if (layoutId == 0) {
            gridSize = 4;
            selectedSprites = sprites1;
        } else if (layoutId == 1) {
            gridSize = 5;
            selectedSprites = sprites2;
        }

        boxes = new NumberBox[gridSize, gridSize];
        Init();
    }

    void Init() {
        List<Vector2> availablePositions = new List<Vector2>();

        for (int y = 0; y < gridSize; y++) {
            for (int x = 0; x < gridSize; x++) {
                availablePositions.Add(new Vector2(x, y));
            }
        }

        int n = 0;
        while (availablePositions.Count > 0) {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 pos = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);

            NumberBox box = Instantiate(boxPrefabs, pos, Quaternion.identity);
            box.Init((int)pos.x, (int)pos.y, n + 1, selectedSprites[n], ClickToSwap);
            boxes[(int)pos.x, (int)pos.y] = box;

            n++;
        }
    }

    void ClickToSwap(int x, int y) {
        if (isAlternateMode) {
            HandleAlternateClick(x, y);
        } else {
            HandleNormalSwap(x, y);
        }
    }

    void HandleNormalSwap(int x, int y) {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        if (dx == 0 && dy == 0) return;

        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];
        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;
        from.UpdatePos(x + dx, y + dy);
        target.UpdatePos(x, y);
        CheckWinCondition();
    }

    void HandleAlternateClick(int x, int y) {
        if (selectedBoxes.Count < 2) {
            NumberBox selectedBox = boxes[x, y];
            selectedBox.Highlight(true); // Đổi màu sang vàng
            selectedBoxes.Add(selectedBox);
        }

        if (selectedBoxes.Count == 2) {
            StartCoroutine(SwapSelectedBoxes());
        }
    }

    IEnumerator SwapSelectedBoxes() {
        NumberBox box1 = selectedBoxes[0];
        NumberBox box2 = selectedBoxes[1];

        Vector2 pos1 = box1.transform.localPosition;
        Vector2 pos2 = box2.transform.localPosition;

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            box1.transform.localPosition = Vector2.Lerp(pos1, pos2, elapsedTime / duration);
            box2.transform.localPosition = Vector2.Lerp(pos2, pos1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        box1.transform.localPosition = pos2;
        box2.transform.localPosition = pos1;

        // Swap trong mảng
        int box1X = box1.x;
        int box1Y = box1.y;
        int box2X = box2.x;
        int box2Y = box2.y;

        boxes[box1X, box1Y] = box2;
        boxes[box2X, box2Y] = box1;

        box1.UpdatePos(box2X, box2Y);
        box2.UpdatePos(box1X, box1Y);

        // Trở lại màu ban đầu
        box1.Highlight(false);
        box2.Highlight(false);
        selectedBoxes.Clear();

        // Kết thúc di chuyển, trở lại trạng thái ban đầu
        isAlternateMode = false;
    }

    int getDx(int x, int y) {
        if (x < gridSize - 1 && boxes[x + 1, y].IsEmpty()) return 1;
        if (x > 0 && boxes[x - 1, y].IsEmpty()) return -1;
        return 0;
    }

    int getDy(int x, int y) {
        if (y < gridSize - 1 && boxes[x, y + 1].IsEmpty()) return 1;
        if (y > 0 && boxes[x, y - 1].IsEmpty()) return -1;
        return 0;
    }

    void CheckWinCondition() {
        int expectedIndex = 1;
        for (int y = gridSize - 1; y >= 0; y--) {
            for (int x = 0; x < gridSize; x++) {
                if (boxes[x, y].index != expectedIndex) return;
                expectedIndex++;
            }
        }
        Debug.Log("Win");
    }

    private void Update() {
        CheckWinCondition();
    }

    public void ToggleAlternateMode() {
        isAlternateMode = true;
    }
}
