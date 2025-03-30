using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSystem.Controllers
{
    public class LargePropertyController : MonoBehaviour, IPropertyController
    {
        private float _scaleFactor = 2f;
        private float _scaleDuration = 1f;

        private void Start()
        {
            StartCoroutine(ScaleObject());

            GetComponent<Rigidbody>().mass *= _scaleFactor * 2;
        }

        private void OnDestroy()
        {
            // сбрасываем все установленные значения
        }
        
        private IEnumerator ScaleObject()
        {
            var startScale = transform.localScale;
            
            float time = 0;

            while (time < _scaleDuration)
            {
                transform.localScale = Vector3.Lerp(startScale, Vector3.one * _scaleFactor, time / _scaleDuration);
                
                time += Time.deltaTime;
                
                yield return null;
            }

            transform.localScale = Vector3.one * _scaleFactor;
        }
    }
}