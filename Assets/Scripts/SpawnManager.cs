using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _treePrefab;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] _bossPrefabs;
    [SerializeField]
    private GameObject[] foregroundLeaves;
    [SerializeField]
    private GameObject _grassPrefab;
    [SerializeField]
    private GameObject _rabbit;
    [SerializeField]
    private float spawnTime = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private GameObject _leavesContainer;
    [SerializeField]
    private GameObject _grassContainer;
    [SerializeField]
    private GameObject _rabbitContainer;
    private bool _stopSpawning = false;
    public bool _grassSpawning = true;
    public bool _powerupSpawning = true;
    private int _currentWave = 1;
    private int _targetWave = 1;
    private int _amtToSpawnThisWave;
    private int _startingAmtToSpawn = 3;
    private UIManager _uiManager;
    private int _enemiesLeft;
    private int _bossNumber = 0; //this will be set to -1 once i have more than one boss because it'll increment to 0, which is the first boss in the array of bosses
    

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UIManager is NULL");
        }
    }    
   
    public void StartSpawning()//coming from asteroid script
    {
        _grassSpawning = true;
        _powerupSpawning = true;
        _stopSpawning = false;
        _currentWave = 1;
        _uiManager.NextWave(_currentWave, _targetWave);
        _startingAmtToSpawn++;
        
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnLeavesRoutine());
        StartCoroutine(SpawnGrassRoutine());
        StartCoroutine(SpawnRabbitRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f); //initially wait for three seconds. this only happens once before the main loop. 
        _amtToSpawnThisWave = _startingAmtToSpawn; //set total amount to spawn this wave    
        _enemiesLeft = _amtToSpawnThisWave;        //reset enemies left and _amt to spawn variables  
        while (_stopSpawning == false && _currentWave <= _targetWave) 
        {
            while (_enemiesLeft > 0)// wait until we clear all the trees
            {
                if (_amtToSpawnThisWave > 0) //spawn trees up to the amt in the wave
                {
                    int randomTree = Random.Range(0, 3);
                    Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8, 0);
                    GameObject newEnemy = Instantiate(_treePrefab[randomTree], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    _amtToSpawnThisWave--;   
                    yield return new WaitForSeconds(0.5f); //new enemy within this wave every () seconds 
                }
                yield return new WaitForSeconds(0.5f); //have to have a yield here to not crash the engine. 
            }
            yield return new WaitForSeconds(4.0f); //pause between waves is () seconds
            if (_currentWave < _targetWave)
            {
                _currentWave++;
                _uiManager.NextWave(_currentWave, _targetWave);
                _amtToSpawnThisWave =+ _currentWave; //amt to spawn increases as waves increase
                _enemiesLeft = _amtToSpawnThisWave;
            }
            else
            {
                //_bossNumber++; when there is more than one boss uncomment this and make sure to set it to -1 before start. 
                _stopSpawning = true;
                _uiManager.BossText(_bossPrefabs[_bossNumber].name.ToString());
                Instantiate(_bossPrefabs[_bossNumber]);
                _grassSpawning = false;     
            }
        }        
    }
       
    public void DecreaseSpawnedEnemy()
    {
        _enemiesLeft--; //once this is at zero that means all the trees in this wave have been cleared. 
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_powerupSpawning == true)
        {
            float random = Random.Range(0, 100); // draw a number between 0 and 99
            int lowLim;    // lowLim and hiLim are automatically set for each powerup
            int hiLim;

            int randomPowerup = Random.Range(0, 5);  
            int chanceToSpawn;

            switch (randomPowerup) //matched to what's assigned in inspector
            {
                case 0:
                    chanceToSpawn = 50;
                    //triple shot
                    break;
                case 1:
                    chanceToSpawn = 70;
                    //speed up
                    break;

                case 2:
                    chanceToSpawn = 70;
                    //shield
                    break;
                case 3:
                    chanceToSpawn = 100;
                    //ammo refil
                    break;
                case 4:
                    chanceToSpawn = 30;
                    //health pickup
                    break;
                default:
                    chanceToSpawn = 100;
                    break;
            }
            lowLim = 0;
            hiLim = chanceToSpawn;
            if (random >= lowLim && random < hiLim) 
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 6, 0);
                GameObject newPowerup = Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
                newPowerup.transform.parent = _powerupContainer.transform;
            }
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    IEnumerator SpawnLeavesRoutine()
    {
        while (_grassSpawning == true)
        {
            int randomLeaves = Random.Range(0, 3);
            float xToSpawn = 0;
            if(randomLeaves == 0 || randomLeaves == 1)
            {
                xToSpawn = 8;
            }
            else
            {
                xToSpawn = 0;
            }        
            Vector3 posToSpawn = new Vector3(xToSpawn, 14, 0);
            GameObject newLeaves = Instantiate(foregroundLeaves[randomLeaves], posToSpawn, Quaternion.identity);
            newLeaves.transform.parent = _leavesContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnGrassRoutine()
    {        
        while (_grassSpawning == true)
        {
            Vector3 posToSpawn = new Vector3(0, 13, 0);
            GameObject newGrass = Instantiate(_grassPrefab, posToSpawn, Quaternion.identity);
            newGrass.transform.parent = _grassContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }        
    }

    IEnumerator SpawnRabbitRoutine()
    {
        while (_grassSpawning == true)
        {
            int _xPosToSpawn = Random.Range(0, 2); //will return either 0 or 1
            if (_xPosToSpawn == 1)
            {
                _xPosToSpawn = 12;  
            }
            else
            {
                _xPosToSpawn = -12;
            }
            Vector3 posToSpawn = new Vector3(_xPosToSpawn, Random.Range(-6.0f, 6.0f), 0); //needs to pick a random y, and also choose between x as -10 or +10
            GameObject newRabbit = Instantiate(_rabbit, posToSpawn, Quaternion.identity);
            newRabbit.transform.parent = _rabbitContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
