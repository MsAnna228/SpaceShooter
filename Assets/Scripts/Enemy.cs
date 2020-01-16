using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{

    private Player _player;
    private Animator _enemyAnim;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _fruit;
    

    [SerializeField]
    private float _speed = 4;   

    [SerializeField]
    private int treeID; //assigned in inspector
    //0 is for banana
    //1 is for papaya
    //2 is for kalo

    void Start()
    {
        //_player = GameObject.Find("Player").GetComponent<Player>();// there's two players sometimes though.
        //if (_player == null)
        //{
          //  Debug.Log("Player component is NULL");
        //}
        _enemyAnim = GetComponent<Animator>();
        if (_enemyAnim == null)
        {
            Debug.LogError("Animator is NULL.");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audiosource on Enemy is NULL");
        }        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);   
        if(transform.position.y <= -6)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8, 0);
            if (_enemyAnim.GetBool("OnFruitDrop") == true)
            {
                Destroy(this.gameObject);
            }         
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null && _enemyAnim.GetBool("OnFruitDrop") == false)
            {
                player.Damage();
            }
            DropFruit();
            _enemyAnim.SetBool("OnFruitDrop", true);
            if (treeID == 0)
            {
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);//to compensate for large art. take out later once art is adjusted
            }
        }     

        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            DropFruit();
            _enemyAnim.SetBool("OnFruitDrop", true);
            if (treeID == 0)
            {
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);//to compensate for large art. take out later once art is adjusted
            }
        }
    }

    void DropFruit()
    {
        if (_enemyAnim.GetBool("OnFruitDrop") == false)
        {
            _audioSource.Play();
            int fruitAmount = Random.Range(0, 3);
            while (fruitAmount > 0)
            {
                fruitAmount--;
                Vector3 posToSpawn = new Vector3(transform.position.x + Random.Range(-2.0f, 2.0f), transform.position.y + Random.Range(-3.0f, 0.0f), 0);
                GameObject newFruit = Instantiate(_fruit, posToSpawn, Quaternion.identity);
            }
        }       
    }
}
