using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guava : MonoBehaviour
{

    [SerializeField]
    private GameObject _guavaShieldTree;

    [SerializeField]
    private int _guavaID; //0 for good, 1 for rotten

    private Player _player;


    void Start()
    {
        //spawned in position and rotation and rigidbody2D velocity from straw guava boss script

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_guavaID == 0)// if not rotten
            {
                _player.AddScore(10);
            }
            if (_guavaID == 1) //if rotten
            {
                _player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_guavaID == 0) //if not rotten spawn a tree 
            {
                int _posToSpawn = Random.Range(-2, 3);
                GameObject _newTree = Instantiate(_guavaShieldTree, (transform.position * _posToSpawn), Quaternion.identity); //add plus or minus 2 to the spawn in position
                _newTree.transform.parent = transform;
            }          
            Destroy(this.gameObject);
        }
    }
}
