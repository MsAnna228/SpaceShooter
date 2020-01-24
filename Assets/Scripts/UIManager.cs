using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _highScoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _playerOneThrusterBar;
    [SerializeField]
    private Image _playerTwoThrusterBar;
    [SerializeField]
    private Text _p1AmmoText;
    [SerializeField]
    private Text _p2AmmoText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _waveNumberText;
    [SerializeField]
    private Image _bossHPBar;
    [SerializeField]
    private Text _bossHPText;
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Scene scene;
    [SerializeField]
    private GameManager _gameManager;   
    private int _currentScore;
    private int _highScore;
    private Color p1Color = Color.green;
    private Color p2Color = Color.green;

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _waveNumberText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _highScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore");
        _restartButton.gameObject.SetActive(false);
        _highScore = PlayerPrefs.GetInt("HighScore");
    }

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore;
        _currentScore = playerScore;
    }

    public void CheckForBestScore()
    {
        if (_currentScore > _highScore)
        {
            PlayerPrefs.SetInt("HighScore", _currentScore);
            _highScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore");
        }
    }
  
    public void UpdateLives(int currentLives)
    {        
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void UpdateP1Thruster(float thrusterAmount)
    {
        _playerOneThrusterBar.fillAmount = thrusterAmount / 100.0f;
        p1Color = Color.Lerp(Color.red, Color.green, thrusterAmount / 100.0f);
        _playerOneThrusterBar.color = p1Color;
    }
    public void UpdateP2Thruster(float thrusterAmount)
    {
        _playerTwoThrusterBar.fillAmount = thrusterAmount / 100.0f;
        p2Color = Color.Lerp(Color.red, Color.green, thrusterAmount / 100.0f);
        _playerTwoThrusterBar.color = p2Color;
    }

    public void UpdateP1Ammo(float ammo, float maxAmmo)
    {
        _p1AmmoText.text = "Spears: " + ammo.ToString() + "/" + maxAmmo.ToString();
    }

    public void UpdateP2Ammo(float ammo, float maxAmmo)
    {
        _p2AmmoText.text = "Spears: " + ammo.ToString() + "/" + maxAmmo.ToString();
    }

    public void GameOver()
    {
        CheckForBestScore();
        _gameOverText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverTextRoutine());
        _gameManager.GameOver();
    }

    public void NextWave(int currentWave, int maxWave)
    {
        _waveNumberText.text = "Wave: " + currentWave.ToString() + "/" + maxWave.ToString();
        _waveNumberText.fontSize = 60;
        StartCoroutine(WaveTextShowRoutine());
    }

    public void BossText(string bossName)
    {
        _waveNumberText.text = "Boss Incoming!" + bossName;
        _waveNumberText.fontSize = 45;
        StartCoroutine(WaveTextShowRoutine());
    }

    public void BossHealthShow(float health, string name)
    {
        _bossHPBar.gameObject.SetActive(true);
        _bossHPText.text = name;
        _bossHPText.gameObject.SetActive(true);
    }

    public void UpdateBossHealth(float health)
    {
        _bossHPBar.fillAmount = health / 100.0f;
        Color _bossColor = Color.Lerp(Color.red, Color.magenta, health / 100.0f);
        _bossHPBar.color = _bossColor;
    }

    public void DisplayMessage(string message)
    {
        _waveNumberText.text = message;
        StartCoroutine(WaveTextShowRoutine());
    }

    IEnumerator WaveTextShowRoutine()
    {
        _waveNumberText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _waveNumberText.gameObject.SetActive(false);
    }

    IEnumerator FlickerGameOverTextRoutine()
    {
            while(true)
        {
            yield return new WaitForSeconds(0.4f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            _gameOverText.gameObject.SetActive(true);           
        }   
    }        
}
