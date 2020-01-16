using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDrop : MonoBehaviour
{

    private float _speed = 3.5f;
    private Player _player;
    [SerializeField]
    private AudioClip _audioClip;


    void Start()
    {
        

     
     
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -11)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.AddScore(10);
            }
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            Destroy(this.gameObject);            
        }
    }
}
