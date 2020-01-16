using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour

{
    [SerializeField]
    private float _speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-8.5f, 1.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((new Vector3(0.8f, 1, 0)) *_speed * Time.deltaTime);
    }
}
