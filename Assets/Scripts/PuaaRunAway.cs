using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuaaRunAway : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private int randomDir;

    private void Start()
    {
        randomDir = Random.Range(0, 2);
    }


    void Update()
    {
        if (randomDir == 1)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            
            if(transform.position.y > 11)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            if(transform.position.x < -11)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
