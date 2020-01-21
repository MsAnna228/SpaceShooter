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
    private bool _grassSpawning = true;
  

    //while current wave < target wave
    //set amt of enemies to spawn this wave
    //spawn those enemies
    //increment wave amount
    //announce current wave

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnLeavesRoutine());
        StartCoroutine(SpawnGrassRoutine());
        StartCoroutine(SpawnRabbitRoutine());
    }

    IEnumerator SpawnEnemyRoutine()

    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            int randomTree = Random.Range(0, 3);
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8, 0);
            GameObject newEnemy = Instantiate(_treePrefab[randomTree], posToSpawn, Quaternion.identity);
            //newEnemy.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);//start small
            //Color tmp = newEnemy.transform.GetComponent<SpriteRenderer>().color; //get reference to alpha variable
            //tmp.a = 0f; //set alpha reference to zero
            //newEnemy.transform.GetComponent<SpriteRenderer>().color = tmp; //update actual to match reference
            newEnemy.transform.parent = _enemyContainer.transform; 
            yield return new WaitForSeconds(5.0f);
        }       
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 5);
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 6, 0);
            GameObject newPowerup = Instantiate (powerups [randomPowerup], posToSpawn, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(5.0f, 7.0f));
        }
    }

    IEnumerator SpawnLeavesRoutine()
    {
        while (_stopSpawning == false)
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
        while (_stopSpawning == false)
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
