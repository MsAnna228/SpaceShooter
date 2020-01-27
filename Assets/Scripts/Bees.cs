using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bees : MonoBehaviour
{
    ParticleSystem bees;
    ParticleSystem.VelocityOverLifetimeModule beesVelocity;
    ParticleSystem.ShapeModule beesShape;
    private Player _player;
    private float _dist;
    private bool _attackedPlayer = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        beesVelocity = GetComponent<ParticleSystem>().velocityOverLifetime;
        beesShape = GetComponent<ParticleSystem>().shape;
        bees = GetComponent<ParticleSystem>();
        if (bees == null)
        {
            Debug.LogError("bees are NULL");
        }
    }

    void Update()
    {
        _dist = Vector3.Distance(_player.transform.position, transform.position);
        if (_player.transform.position.y > transform.parent.transform.position.y && _dist > 5) //smart enemy behavior - if player is above the hive, then the bees will fly upward, effectively "shooting backwards"
        {
            StartCoroutine(SwarmUpward());
        }
        else if (_dist > 7) //if not near player (or below player), just swirl around the hive.
        {
            beesShape.rotation = new Vector3(270, 0, 0);
            beesVelocity.orbitalX = 1;
            beesVelocity.z = 0.5f;
            bees.startSpeed = 2;
        }
        else if (_dist < 7) //aggressive enemy behavior
        {
            StartCoroutine(SwarmToPlayer());
        }        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.Damage();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SwarmToPlayer()
    {
        bees.startSpeed = 1;
        beesShape.rotation = Vector3.zero;
        beesVelocity.orbitalX = 0;
        beesVelocity.z = 0;
        transform.LookAt(_player.transform);
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator SwarmUpward()
    {
        bees.startSpeed = 1;
        beesShape.rotation = new Vector3(270, 0, 0);
        beesVelocity.orbitalX = 0;
        beesVelocity.z = 0;
        yield return new WaitForSeconds(2.0f);
    }
}
