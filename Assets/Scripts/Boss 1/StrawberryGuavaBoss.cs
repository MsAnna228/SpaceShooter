using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrawberryGuavaBoss : MonoBehaviour
{
    private Player _player;
    private UIManager _uiManager;

    [SerializeField]
    private float _bossHealth = 100f;
    [SerializeField]
    private GameObject _guavaShieldPrefab;
    [SerializeField]
    private GameObject _individualGuavaTree;
    [SerializeField]
    private GameObject[] _guavaPrefabs;
    [SerializeField]
    private int numberOfGuavas = 15;
    [SerializeField]
    private float guavaSpeed = 5f;
    [SerializeField]
    private GameObject _warningSign;
    [SerializeField]
    private GameObject _initialTree;
    

    private SpriteRenderer _renderer;
    private Vector3 startPoint;                 // Starting position of the guava
    private const float radius = 1F;
    private float _canFire = -5;
    private float _fireRate = 10.0f;
    private float _canRegenShield = -1;
    private float _shieldRegenRate = 30f;
    private float _canSpawnTree = -1;
    private float _treeSpawnRate = 10f;
    private GameObject _playerGameObject;


    void Start()
    {
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (_playerGameObject == null)
        {
            Debug.LogError("Player is NULL");
        }
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("Renderer is NULL");
        }
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UiManager is NULL");
        }
        _uiManager.BossHealthShow(_bossHealth, gameObject.name.ToString()); //set HP
                                                                            //HP Bar appear
        transform.position = new Vector3(0, 10, 0); //appear at the top 
        StartCoroutine(SlideToStarting());          //slide in to a starting position
    }

    IEnumerator SlideToStarting()
    {
        while (transform.position.y > 3.3)
        {
            transform.Translate(Vector3.down * 6 * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            _bossHealth--;
            _uiManager.UpdateBossHealth(_bossHealth);
        }
        if (_bossHealth > 0)
        {
            //Behavior 1: throw guavas radiating out away from the main tree
            if (Time.time > _canFire)
            {
                StartCoroutine(SpawnGuavas(numberOfGuavas, true));
                _canFire = Time.time + _fireRate;
            }
            //Behavior 2: after hp is below 50 start spawning trees in a shield position every thirty seconds
            if (_bossHealth <= 50)
            {
                _renderer.color = Color.yellow;
                if (Time.time > _canRegenShield)
                {
                    GuavaShield();
                    _canRegenShield = Time.time + _shieldRegenRate;
                }
            }
            //Behavior 3: after hp is below 75, change color, and start spawning trees at the location of the player, give warning etc
            if (_bossHealth <= 25)
            {
                _renderer.color = Color.red;
                if (Time.time > _canSpawnTree)
                {
                    StartCoroutine(SendWarningAndAttack());
                    _canSpawnTree = Time.time + _treeSpawnRate;
                }
            }
        }
    }
   
    IEnumerator SpawnGuavas(int _numberOfGuavas, bool _spawnRotten)
    {
        float angleStep = 360f / _numberOfGuavas;
        float angle = 0f;
        int _guavaTypeMax;

        for (int i = 0; i <= _numberOfGuavas - 1; i++)
        {
            // Direction & vector calculations
            float guavaDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float guavaDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
            Vector3 guavaVector = new Vector3(guavaDirXPosition, guavaDirYPosition, 0);
            Vector3 guavaMoveDirection = (guavaVector - startPoint).normalized * guavaSpeed;

            if (_spawnRotten)
            {
                _guavaTypeMax = 2;
            }
            else
            {
                _guavaTypeMax = 1;
            }
            // Create guavas using those vectors for the velocity
            GameObject newGuava = Instantiate(_guavaPrefabs[Random.Range(0, _guavaTypeMax)], startPoint, Quaternion.identity);
            newGuava.GetComponent<Rigidbody2D>().velocity = new Vector3(guavaMoveDirection.x, guavaMoveDirection.y, 0);
            newGuava.transform.parent = transform; 
            Destroy(newGuava, 3F);

            angle += angleStep;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void GuavaShield()
    {
        _guavaShieldPrefab.SetActive(true);
        for (int a = 0; a < _guavaShieldPrefab.transform.childCount; a++)
        {
            _guavaShieldPrefab.transform.GetChild(a).gameObject.SetActive(true);
        }
    }

    IEnumerator SendWarningAndAttack()
    {
        GameObject _newWarning = Instantiate(_warningSign, _playerGameObject.transform.position, Quaternion.identity);
        _newWarning.transform.parent = transform;
        yield return new WaitForSeconds(3.0f);
        GameObject _newIndividualTree = Instantiate(_individualGuavaTree, _newWarning.transform.position, Quaternion.identity);
        _newIndividualTree.transform.parent = transform;
        Destroy(_newWarning.gameObject);       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.KnockBack();
        }
        if (other.gameObject.tag == "Laser")
        {
            _bossHealth -= 5;
            _uiManager.UpdateBossHealth(_bossHealth);

            if (_bossHealth < 1)
            {
                //explode in a victory amount of only good guavas!!
                StartCoroutine(SpawnGuavas(50, false));
                //communicate to UI manager to say "Boss defeated!"
                _uiManager.DisplayMessage("Boss Defeated!!");
                StartCoroutine(BossDefeated());
            }            
        }
    }

    IEnumerator BossDefeated()
    {
        yield return new WaitForSeconds(10.0f);
        GameObject _newInitialTree = Instantiate(_initialTree, transform.position, Quaternion.identity);
        Asteroid _asteroid = _newInitialTree.GetComponent<Asteroid>();
        _asteroid.PastLevelOne();
        Destroy(this.gameObject);
    }
}
