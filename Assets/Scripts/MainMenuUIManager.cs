using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Image _rotatingImage1; // Первая вращающаяся картинка
    [SerializeField] private Image _rotatingImage2; // Вторая вращающаяся картинка
    [SerializeField] private float _rotationSpeed = 10f; // Скорость вращения

    private void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Update()
    {
        // Вращаем обе картинки с постоянной скоростью
        if (_rotatingImage1 != null)
        {
            _rotatingImage1.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
        
        if (_rotatingImage2 != null)
        {
            _rotatingImage2.transform.Rotate(Vector3.forward, -_rotationSpeed * Time.deltaTime);
        }
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
    }

    private void OnExitButtonClicked()
    {
        ExitGame();
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }
}
