using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
  
    void Update()
    {
        transform.Translate(new Vector3 (0, (_speed *Time.deltaTime), 0));

        if (transform.position.y > 6)
        {

            //check if object has a parent
            //destroy the parent too!
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
