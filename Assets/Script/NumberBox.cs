using System;
using UnityEngine;

public class NumberBox : MonoBehaviour {
    public int index = 0;
    public int x = 0;
    public int y = 0;
    private Action<int, int> swapFunc = null;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc) {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        UpdatePos(i, j);
        this.swapFunc = swapFunc;
    }

    public void UpdatePos(int i, int j) {
        x = i;
        y = j;
        this.gameObject.transform.localPosition = new Vector2(i, j);
    }

    public bool IsEmpty() {
        return index == (Puzzle.gridSize * Puzzle.gridSize);
    }

    public void Highlight(bool isActive) {
        if (isActive) {
            spriteRenderer.color = Color.yellow; // Đổi màu sang vàng
        } else {
            spriteRenderer.color = Color.white; // Trở lại màu trắng
        }
    }

    private void OnMouseDown() {
        if (Input.GetMouseButtonDown(0) && swapFunc != null) {
            swapFunc(x, y);
        }
    }
}
