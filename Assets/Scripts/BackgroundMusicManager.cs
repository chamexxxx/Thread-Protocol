using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Tooltip("Список всех аудиоклипов для фоновой музыки")]
    [SerializeField] private List<AudioClip> musicClips;

    [Tooltip("Список всех объектов, на которые нужно повесить AudioSource")]
    [SerializeField] private List<GameObject> speakers;
    
    [SerializeField] private float _maxDistance = 15f;

    private AudioSource[] audioSources;

    void Start()
    {
        // Создаем список AudioSource на всех указанных GameObject
        audioSources = new AudioSource[speakers.Count];
        
        for (int i = 0; i < speakers.Count; i++)
        {
            audioSources[i] = speakers[i].AddComponent<AudioSource>();
            audioSources[i].loop = false;  // Чтобы не зацикливалось
            audioSources[i].playOnAwake = false;
            audioSources[i].spatialBlend = 1f;  // Для 3D звука (0 - 2D, 1 - 3D)
            audioSources[i].dopplerLevel = 0f;  // Для отсутствия эффекта Доплера (если не нужен)
            audioSources[i].maxDistance = _maxDistance;  // Максимальное расстояние, на котором слышен звук
            audioSources[i].rolloffMode = AudioRolloffMode.Linear;  // Для линейного падения звука по мере удаления
            audioSources[i].volume = 1f;
        }

        // Запускаем воспроизведение музыки
        StartCoroutine(PlayMusic());
    }

    // Метод для поочередного воспроизведения всех аудиоклипов
    private IEnumerator PlayMusic()
    {
        foreach (var clip in musicClips)
        {
            // Получаем текущее время DSP для синхронизации
            double dspTime = AudioSettings.dspTime + 0.1f; // Добавляем небольшую задержку, чтобы синхронизация была точной

            // Запуск одного клипа на всех динамиках с использованием dspTime
            foreach (var audioSource in audioSources)
            {
                audioSource.clip = clip;
                // audioSource.Play();
                audioSource.PlayScheduled(dspTime);
            }

            // Ждем, пока музыка не закончится, прежде чем начать следующую
            yield return new WaitForSeconds(clip.length);
        }
    }
}