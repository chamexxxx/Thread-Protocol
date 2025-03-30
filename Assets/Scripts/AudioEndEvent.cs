using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioEndEvent : MonoBehaviour
{
    public AudioSource audioSource; // Перетащи сюда AudioSource в Inspector

    private bool hasPlayed = false; // Чтобы событие не вызывалось многократно
    
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null)
            StartCoroutine(WaitForAudioEnd());
    }

    private System.Collections.IEnumerator WaitForAudioEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        OnAudioEnd();
    }

    private void OnAudioEnd()
    {
        Debug.Log("Аудио закончилось!");
        
        SceneManager.LoadScene("Room 3", LoadSceneMode.Single);
    }
}