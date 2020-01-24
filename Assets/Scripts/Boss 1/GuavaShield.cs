using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuavaShield : MonoBehaviour
{
    private int _health = 2;
    private Player _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "laser")
        {
            Destroy(other.gameObject);
            _health--; 
            
            if (_health < 1)
            {
                if (transform.parent.gameObject.name == "Strawberry Guava Boss") //if this tree is one of the shield objects attached to the main tree
                {
                    //dont destroy this game object! Just set it to inactive. 
                    this.gameObject.SetActive(false);
                }
                else //else this is a guava tree that was spawned from one of the guavas and should be destroyed.
                {
                    Destroy(this.gameObject);
                }
            }
        }
        if (other.gameObject.tag == "Player")
        {
            _player.Damage();
        }
    }

    void OnEnable()
    {
        _health = 2;
    }
}
