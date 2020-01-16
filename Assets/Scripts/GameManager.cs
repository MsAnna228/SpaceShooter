using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    Scene _currentScene;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _pauseButton;

    private Animator _pauseMenuAnimator;



    private void Start()
    {
        //_pauseMenu.SetActive(false);
        _pauseButton.SetActive(true);
        _pauseMenuAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            _currentScene = SceneManager.GetActiveScene();
            if (_currentScene.buildIndex == 0)// if you hit esc on the main menu it quits the application
            {
                Application.Quit();
            }
        }

        if (Input.GetKey(KeyCode.P))
        {
            //pause the game 
            OnPauseButtonClicked();
        }
    }

    public void OnRestartClicked()
    {
        if (_isGameOver == true)
        {
            _currentScene = SceneManager.GetActiveScene();//gets reference to current game scene, whether single player or co-op mode

            SceneManager.LoadScene(_currentScene.buildIndex);//reloads current game scene
        }
    }

    public void OnPauseButtonClicked()
    {
        //_pauseMenu.SetActive(true);
        _pauseMenuAnimator.SetBool("isPaused", true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void OnResumeButtonClicked()
    {

        _pauseMenuAnimator.SetBool("isPaused", false);
        //_pauseMenu.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1.0f;

    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        _isGameOver = true;
        
    }
}
