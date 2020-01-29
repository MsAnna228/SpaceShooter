using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrop : MonoBehaviour
{
    private float _speed = 4.0f;
    private UIManager _uiManager;


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("Uimanager is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _uiManager.EggSplat();
            Destroy(this.gameObject);
        }
    }
}
