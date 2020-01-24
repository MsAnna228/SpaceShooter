using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private GameObject _treePrefab;

    private SpawnManager _spawnManager;
    private StartingGrass _startingGrass;
    public bool gameStarted = false;

    public bool _pastLevelOne = false;
  

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_pastLevelOne == false)
        {
            _startingGrass = GameObject.Find("InitialGrass").GetComponent<StartingGrass>();
            if (_startingGrass == null)
            {
                Debug.LogError("Initial Grass is NULL");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_treePrefab, transform.position, Quaternion.identity); //this tree prefab just moves down the screen. 
            _spawnManager.StartSpawning();
            if (_pastLevelOne == false)
            {
                _startingGrass.StartScrolling(); //if we are past level one this game object is already destroyed so this will throw an error
            }
            gameStarted = true;      
            Destroy(gameObject);
        }
    }  
    
    public void PastLevelOne()
    {
        _pastLevelOne = true;
    }
}
