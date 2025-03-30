using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioEndEvent : MonoBehaviour
{
    public AudioSource audioSource; // Перетащи сюда AudioSource в Inspector

    private bool hasPlayed = false; // Чтобы событие не вызывалось многократно

    void Update()
    {
        if (!audioSource.isPlaying && hasPlayed)
        {
            hasPlayed = false;
            OnAudioEnd();
        }
    }

    public void PlayAudio()
    {
        audioSource.Play();
        hasPlayed = true; // Отмечаем, что аудио началось
    }

    void OnAudioEnd()
    {
        SceneManager.LoadScene("Room 3", LoadSceneMode.Single);
    }
}