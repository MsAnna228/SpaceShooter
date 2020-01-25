using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2.0f;
    private float _maxSpeed = 10.0f;
    private float _minSpeed = 5.0f;
    private float _acceleration = 10.0f;
    private float _deceleraton = 10.0f;
    [SerializeField]
    private float horizontalBounds = 13f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _puaaRunAwayPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1;
    private float _playerTwoCanFire = -1;
    private int _p1Ammo = 15;
    private int _p2Ammo = 15;
    private int _p1MaxAmmo = 15; //can be upgradeable
    private int _p2MaxAmmo = 15;
    private int _shieldHealth = 3;   

    [SerializeField]
    private Image _thrusterBar;
    private float _thrusterAmount = 100.0f; //this could be upgradeable

    [SerializeField]
    private int _lives = 3;    
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int score;
    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Animator _playerAnim;
    private Asteroid _asteroid;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private int coolDownTime;


    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _outOfThrusterClip;
    [SerializeField]
    private AudioClip _rechargingThrusterClip;
    [SerializeField]
    private AudioClip _thrusterRechargedClip;
    [SerializeField]
    private AudioClip _outOfAmmoClip;
    [SerializeField]
    private AudioClip _cameraShakeClip;
    [SerializeField]
    private AudioClip _damageClip;
    [SerializeField]
    private AudioClip _healClip;
    [SerializeField]
    private AudioClip _shieldBrokenClip;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    private CameraShake _cameraShake;
    private SpriteRenderer _shieldColor;

    
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        switch (buildIndex)
        {
            case 0:
                break;
            case 1: //single player mode starts in the middle
                transform.position = new Vector3(0, 0, 0);
                break;
            case 2: //co-op mode starting positions 
                if (this.gameObject.name == "Player 1")
                {
                    transform.position = new Vector3(-5, 0, 0);                    
                }
                else if (this.gameObject.name == "Player 2")
                {
                    transform.position = new Vector3(5, 0, 0);
                }
                break;
            default:
                break;
        }

        _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
        if (_shieldColor == null)
        {
            Debug.LogError("the Color on shield is NULL");
        }

        _cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.LogError("camera shake on main camera is NULL");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UIManager is NULL");
        }

        _asteroid = GameObject.Find("InitialTree").GetComponent<Asteroid>();
        if(_asteroid == null)
        {
            Debug.LogError("Asteroid/Initial Tree is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer on Player is NULL");
        }

        _playerAnim = GetComponent<Animator>();
        if(_playerAnim == null)
        {
            Debug.LogError("Animator on Player is NULL");
        }
    }

    void Update()
    {
        if (isPlayerOne)
        {
            CalculateMovement();
            if (CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
            {
                Firing();
            }
        }

        if (isPlayerTwo)
        {
            CalculateMovementPlayerTwo();
            if (CrossPlatformInputManager.GetButtonDown("Fire2") && Time.time > _playerTwoCanFire)
            {
                FiringPlayerTwo();
            }
        }        
    }

    void Firing()
    {  
        _canFire = Time.time + _fireRate;

        if (_p1Ammo > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, (new Vector3((transform.position.x - 0.5f), (transform.position.y + 0.5f), 0)), Quaternion.identity);
            }
            _p1Ammo -= 1;
            _uiManager.UpdateP1Ammo(_p1Ammo, _p1MaxAmmo);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = _outOfAmmoClip;
            _audioSource.Play();
        }
    }

    void FiringPlayerTwo()
    {
        _playerTwoCanFire = Time.time + _fireRate;

        if (_p2Ammo > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, (new Vector3((transform.position.x - 0.5f), (transform.position.y + 0.5f), 0)), Quaternion.identity);
            }
            _p2Ammo -= 1;
            _uiManager.UpdateP2Ammo(_p2Ammo, _p2MaxAmmo);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = _outOfAmmoClip;
            _audioSource.Play();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");// Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");//Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //thruster logic
        if ((Input.GetKey(KeyCode.LeftShift) && _speed < _maxSpeed) && _thrusterAmount > 0)
        {
            _speed += _acceleration * Time.deltaTime;
            _thrusterAmount -= 1.0f;
            _uiManager.UpdateP1Thruster(_thrusterAmount);            
            if (_thrusterAmount < 1.0f)
            {
                _audioSource.clip = _outOfThrusterClip;
                _audioSource.Play();
                _thrusterAmount = 0.0f;
                _uiManager.UpdateP1Thruster(_thrusterAmount);
                StartCoroutine(ThrusterCoolDownRoutine());
            }
        }
        else if (_speed > _minSpeed && _speed > _deceleraton * Time.deltaTime)
        {
            _speed -= _deceleraton * Time.deltaTime;            
        }               

        //flip on the x if running left or right
        if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
        }

        //animation logic 
        if (horizontalInput == 0) //if we are not moving left or right
        {
            if (_asteroid.gameStarted == false) //if the game hasn't started
            {
                _playerAnim.SetBool("Forward", true); //play idle animation facing forward
            }
            else //if the game has started
            {
                Debug.Log("run forward animation goes here");//play idle animation running forward
            }
        }
        else if(_playerAnim.GetBool("OnPuaa") == false) //otherwise, if we ARE moving left or right AND we are not on the puaa
        {
            _playerAnim.SetBool("Forward", false); //run side to side
        }    

        //y boundaries    
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 1.5f), 0);

        //screenwrapping on the x   
        if (transform.position.x > horizontalBounds)
        {
            transform.position = new Vector3((horizontalBounds * -1), transform.position.y, 0);
        }
        else if (transform.position.x < (horizontalBounds * -1))
        {
            transform.position = new Vector3(horizontalBounds, transform.position.y, 0);
        }
    }

    void CalculateMovementPlayerTwo()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal_2");// Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical_2");//Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //thruster logic
        if ((Input.GetKey(KeyCode.RightShift) && _speed < _maxSpeed) && _thrusterAmount > 0)
        {
            _speed += _acceleration * Time.deltaTime;
            _thrusterAmount -= 1.0f;
            _uiManager.UpdateP2Thruster(_thrusterAmount);
            if (_thrusterAmount < 1.0f)
            {
                _audioSource.clip = _outOfThrusterClip;
                _audioSource.Play();
                _thrusterAmount = 0.0f;
                _uiManager.UpdateP2Thruster(_thrusterAmount);
                StartCoroutine(P2ThrusterCoolDownRoutine());
            }
        }
        else if (_speed > _minSpeed && _speed > _deceleraton * Time.deltaTime)
        {
            _speed -= _deceleraton * Time.deltaTime;
        }

        //flip on the x if running left or right
        if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
        }

        //animation logic 
        if (horizontalInput == 0) //if we are not moving left or right
        {
            if (_asteroid.gameStarted == false) //if the game hasn't started
            {
                _playerAnim.SetBool("Forward", true); //play idle animation facing forward
            }
            else //if the game has started
            {
                Debug.Log("run forward animation goes here");//play idle animation running forward
            }
        }
        else if (_playerAnim.GetBool("OnPuaa") == false) //otherwise, if we ARE moving left or right AND we are not on the puaa
        {
            _playerAnim.SetBool("Forward", false); //run side to side
        }

        //y boundaries    
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 1.5f), 0);

        //screenwrapping on the x   
        if (transform.position.x > horizontalBounds)
        {
            transform.position = new Vector3((horizontalBounds * -1), transform.position.y, 0);
        }
        else if (transform.position.x < (horizontalBounds * -1))
        {
            transform.position = new Vector3(horizontalBounds, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            if (_shieldHealth > 0)
            {
                _shieldHealth -= 1;               
                if (_shieldHealth == 2)
                {
                    _shieldColor.color = Color.cyan;
                }
                if (_shieldHealth == 1)
                {
                    _shieldColor.color = Color.red;
                }
            }
            else
            {
                _audioSource.clip = _shieldBrokenClip;
                _audioSource.Play();
                _shieldVisualizer.SetActive(false);
                _isShieldActive = false;
                return;
            }
        }
        else
        {
            _lives--;
            StartCoroutine(_cameraShake.Shake(1.0f, 0.15f));
            _audioSource.clip = _damageClip;
            _audioSource.Play();
            CheckDamageVisuals();        
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                _uiManager.GameOver();
                Destroy(gameObject);
            }
        }       
    }

    void CheckDamageVisuals()
    {
        if (_lives == 3)
        {
            _rightDamage.SetActive(false);
            _leftDamage.SetActive(false);
        }
        if (_lives == 2)
        {
            _rightDamage.SetActive(true);
            _leftDamage.SetActive(false);
        }
        else if (_lives == 1)
        {
            _leftDamage.SetActive(true);
        }
    }

    public void RefillHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            CheckDamageVisuals();
            _audioSource.clip = _healClip;
            _audioSource.Play();
        }
    }

    IEnumerator ThrusterCoolDownRoutine()
    {
        coolDownTime = 0; //this could be upgradeable, for a shorter cooldown time.
        _audioSource.clip = _rechargingThrusterClip;// plays once at the beginning of the recharge time
        _audioSource.Play();
        while (coolDownTime < 10)
        {
            yield return new WaitForSeconds(1.0f);
            _uiManager.UpdateP1Thruster(coolDownTime * 10.0f);
            coolDownTime += 1;            
        }
        //once cool down time is over...
        _audioSource.clip = _thrusterRechargedClip;
        _audioSource.Play();
        _thrusterAmount = 100.0f;
        _uiManager.UpdateP1Thruster(_thrusterAmount);
    }

    IEnumerator P2ThrusterCoolDownRoutine()
    {
        coolDownTime = 0;
        _audioSource.clip = _rechargingThrusterClip;
        _audioSource.Play();
        while (coolDownTime < 10)
        {
            yield return new WaitForSeconds(1.0f);
            _uiManager.UpdateP2Thruster(coolDownTime * 10.0f);
            coolDownTime += 1;
        }
        _audioSource.clip = _thrusterRechargedClip;
        _audioSource.Play();
        _thrusterAmount = 100.0f;
        _uiManager.UpdateP2Thruster(_thrusterAmount);
    }

   public void RefillP1Ammo()
    {
        _p1Ammo = 15;
        _uiManager.UpdateP1Ammo(_p1Ammo, _p1MaxAmmo);
    }

    public void RefillP2Ammo()
    {
        _p2Ammo = 15;
        _uiManager.UpdateP2Ammo(_p2Ammo, _p2MaxAmmo);
    }

    public void ShieldsActive()
    {
        _shieldHealth = 3;
        _shieldColor.color = Color.green;
        _shieldVisualizer.SetActive(true);
        _isShieldActive = true;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine ());
    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {        
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;        
    }

    public void SpeedActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
        _playerAnim.SetBool("OnPuaa", true);
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        if (_speed < _minSpeed)
        {
            _speed = _minSpeed;
        }
        _playerAnim.SetBool("OnPuaa", false);
        Instantiate(_puaaRunAwayPrefab, transform.position, Quaternion.identity);
    }

    public void AddScore(int points) //passed through points by the enemy
    {
        score += points;
        _uiManager.UpdateScore(score);     //communicate to UI Manager to update score, we are passing through our current score.
    }

    public void KnockBack()
    {
        int _knockBackAmount = 5;
        while (_knockBackAmount > 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            _knockBackAmount--;
        }
    }

    public void SlowDownPlayer()
    {
        StartCoroutine(SlowDown());
    }
      
    IEnumerator SlowDown()
    {
        StartCoroutine(_cameraShake.Shake(1.0f, 0.1f));
        _spriteRenderer.color = Color.gray;
        _speed = 2;
        yield return new WaitForSeconds(3.0f);
        _speed = _minSpeed;
        _spriteRenderer.color = Color.white;        
    }
}
