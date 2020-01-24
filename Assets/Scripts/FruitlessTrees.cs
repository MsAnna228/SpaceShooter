using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitlessTrees : MonoBehaviour
{

    private float _speed = 4f;
 

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -11)
        {
            Destroy(this.gameObject);
        }
    }
}
