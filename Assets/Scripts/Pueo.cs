using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pueo : MonoBehaviour
{

    private bool _hurtPlayer = false;
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float speedUpDown = 1;
    [SerializeField]
    private float distanceUpDown = 1;
    [SerializeField]
    private GameObject _eggPrefab;

    private float _speedModifier;
    private bool _eggDropped = false;
    private bool _attackedPowerup = false;

    private void Start()
    {
        if (transform.position.x < 0)
        {
            _speedModifier = Random.Range(0.1f, 1.0f);
            transform.position = new Vector3(transform.position.x, -4, transform.position.z);
        }
        else if (transform.position.x > 0)
        {
            _speedModifier = Random.Range(-1.0f, -0.1f);
            transform.position = new Vector3(transform.position.x, 4, transform.position.z);
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {

        transform.Translate(new Vector3(_speed, Mathf.Sin(speedUpDown * Time.time) * distanceUpDown, transform.position.z) * Time.deltaTime);
        if (transform.position.x > 13 || transform.position.x < -13) // the number 13 has to be bigger than the random spawned range which is set in the spawn manager. 
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y > 2.5 && _eggDropped == false)
        {
            _eggDropped = true;
            StartCoroutine(DropAnEgg());
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
        if (other.gameObject.tag == "Powerup" && _attackedPowerup == false) //attached collider that extends below the pueo so it can tell if powerups are below it. 
        {
            StartCoroutine(DropAnEgg());
            _attackedPowerup = true;
        }
    }

    IEnumerator DropAnEgg()
    {
        Instantiate(_eggPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        _eggDropped = false;
        _attackedPowerup = false;
    }
}
