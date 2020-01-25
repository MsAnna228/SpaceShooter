using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID; //0 is for triple shot
                           //1 is for speed
                           //2 is for shields
                           //3 is for refilling spears
                           //4 is for refilling health
    [SerializeField]
    private AudioClip _powerupCollectClip;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //pickup collect
        {
            StartCoroutine(GlideToPlayer());
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }        
    }

    IEnumerator GlideToPlayer()
    { 
        while (transform.position != _player.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 2 * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_powerupCollectClip, transform.position);
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        if (other.gameObject.name == "Player 1")
                        {
                            player.RefillP1Ammo();
                        }
                        else if (other.gameObject.name == "Player 2")
                        {
                            player.RefillP2Ammo();
                        }
                        break;

                    case 4:
                        player.RefillHealth();
                        break;

                    default:
                        break;
                }                             
            }        
            Destroy(gameObject);            
        }
    }
}
