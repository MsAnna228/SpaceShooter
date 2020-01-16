using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingForegroundLeaves : MonoBehaviour
{
    //create speed variable

    [SerializeField]
    private float _speed = 5f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -11)
        {
            Destroy(this.gameObject);
        }      
    }
}
