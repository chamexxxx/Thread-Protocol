using System;
using SpellSystem;
using UnityEngine;

public class StudyableObjectRaycaster : MonoBehaviour
{
    public event Action<StudyableObject> StudyableObjectFound;
    public event Action StudyableObjectLost;
    
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _maxStudyDistance = 5f;
    [SerializeField] private LayerMask _studyableLayer;
    
    private StudyableObject _currentStudyableObject;

    private void Start()
    {
        if (_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        var ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxStudyDistance, _studyableLayer))
        {
            var hitObject = hit.collider.gameObject;
            var studyableObject = hitObject.GetComponent<StudyableObject>() 
                            ?? hitObject.GetComponentInParent<StudyableObject>();

            if (studyableObject != null)
            {
                if (_currentStudyableObject != studyableObject)
                {
                    _currentStudyableObject = studyableObject;
                    
                    // StudyableObjectFound?.Invoke(studyableObject);
                }
                
                StudyableObjectFound?.Invoke(studyableObject);
            }
            else
            {
                ClearCurrentStudyableObject();
            }
        }
        else
        {
            ClearCurrentStudyableObject();
        }
    }

    private void ClearCurrentStudyableObject()
    {
        if (_currentStudyableObject != null)
        {
            _currentStudyableObject = null;
            
            StudyableObjectLost?.Invoke();
        }
    }
}
