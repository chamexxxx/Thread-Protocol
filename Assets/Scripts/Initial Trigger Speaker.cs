using System;
using UnityEngine;

public class InitialTriggerSpeaker : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered)
        {
            return;
        }

        _isTriggered = true;
        
        _audioSource.Play();
    }
}
