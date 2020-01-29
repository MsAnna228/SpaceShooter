using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private float _direction;
    private bool _hurtPlayer = false;
    private float _speed = 4.0f;

    void Start()
    {
        if (transform.position.x < 0)
        {
            _direction = Random.Range(0.1f, 1.0f);
        }
        else if (transform.position.x > 0)
        {
            _direction = Random.Range(-1.0f, -0.1f);
        }
    }

    void Update()
    {
        transform.Translate(new Vector3(_direction * _speed, 0, 0) * Time.deltaTime);

        if (transform.position.x > 13 || transform.position.x < -13) // the number 13 has to be bigger than the random spawned range
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null && _hurtPlayer == false)
            {
                player.Damage();
                _hurtPlayer = true;
                _speed = 6.0f;
            }
        }
    }
}
