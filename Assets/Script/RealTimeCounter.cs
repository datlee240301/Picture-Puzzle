using System.Collections;
using UnityEngine;

public class RealTimeCounter : MonoBehaviour {
    public int secondsCount = 0;
    private bool isRunning = false;

    void Start() {
        StartTimer();
    }

    public void StartTimer() {
        isRunning = true;
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer() {
        isRunning = false;
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer() {
        while (isRunning) {
            yield return new WaitForSeconds(1f); 
            //elapsedTime += 1f; 
            secondsCount++; 
        }
    }
    public int GetSecondsCount() {
        return secondsCount;
    }
}
