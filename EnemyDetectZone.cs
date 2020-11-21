using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectZone : MonoBehaviour
{
    public EnemyController Enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Enemy.GetTaget(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Enemy.GetTaget(other);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Enemy.GetTaget(other);
        }
    }
}
