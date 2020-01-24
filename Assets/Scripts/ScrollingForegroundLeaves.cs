using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingForegroundLeaves : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;

    private bool _moving = true;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        StartCoroutine(CheckSpeedRoutine());
    }

    void Update()
    {
       transform.Translate(Vector3.down * _speed * Time.deltaTime);
       if (transform.position.y < -11)
       {
           Destroy(this.gameObject);
       }
    }    

    IEnumerator CheckSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (_spawnManager._grassSpawning)
            {
                _speed = 5f;
            }
            else
            {
                _speed = 0f;
            }
        }
    }
}
