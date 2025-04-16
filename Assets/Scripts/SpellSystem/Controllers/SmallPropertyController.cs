using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpellSystem.Controllers
{
    public class SmallPropertyController : MonoBehaviour, IPropertyController
    {
        private StudyableObject _studyableObject;
        private float _scaleFactor = 2f;
        private float _scaleDuration = 1f;
        private Rigidbody _rigidbody;
        private Coroutine _scaleObjectRoutine;

        private void Start()
        {
            _studyableObject = GetComponent<StudyableObject>();

            if (_studyableObject == null)
            {
                Debug.Log("StudyableObject not found");
                enabled = false;
                return;
            }
            
            ScaleObjectRoutine(_studyableObject.InitialScale / _scaleFactor).Forget();

            _rigidbody = GetComponent<Rigidbody>();

            // if (_rigidbody != null)
            // {
            //     _rigidbody.mass *= _scaleFactor / 2;
            // }
        }
        
        private async UniTask ScaleObjectRoutine(Vector3 finalScale)
        {
            var startScale = transform.localScale;
            
            float time = 0;

            while (time < _scaleDuration)
            {
                transform.localScale = Vector3.Lerp(startScale, finalScale, time / _scaleDuration);
                
                time += Time.deltaTime;
                
                await UniTask.Yield();
            }
            
            transform.localScale = finalScale;
        }
    }
}