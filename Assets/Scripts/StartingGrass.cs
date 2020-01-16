using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGrass : MonoBehaviour

{
    [SerializeField]
    private float _speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -11)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartScrolling()
    {
        _speed = 4.0f;
    }

    
}
