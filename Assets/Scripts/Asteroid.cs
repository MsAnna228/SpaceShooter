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
  

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        _startingGrass = GameObject.Find("InitialGrass").GetComponent<StartingGrass>();
        if(_startingGrass == null)
        {
            Debug.LogError("Initial Grass is NULL");
        }
        
    }

    void Update()
    {
        //transform.Rotate(0, 0, Time.deltaTime * _speed, Space.Self);
        //rotate on the zed axis
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Instantiate(_treePrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            _startingGrass.StartScrolling();
            gameStarted = true;
      
            Destroy(gameObject);
        }
    }
   
}
