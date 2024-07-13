using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip moveBoxSound;
    public AudioClip winSound;
    public AudioClip exchangeSound;
    bool hasPlayWinSound = false;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt(StringManager.musicOn) == 1)
            audioSource.volume = 1;
        else audioSource.volume = 0;
    }

    public void PlayMusic() {
        audioSource.volume = 1;
    }

    public void StopMusic() {
        audioSource.volume = 0;
    }

    public void PlayMoveBoxSound() {
        audioSource.PlayOneShot(moveBoxSound);
    }
    
    public void PlayWinSound() {
        if (!hasPlayWinSound) {
        audioSource.PlayOneShot(winSound);
        hasPlayWinSound = true;
        }
    }
    
    public void PlayExchangeSound() {
        audioSource.PlayOneShot(exchangeSound);
    }
    
    public void StopBGMusic() { 
        audioSource.Stop();
    }
}
