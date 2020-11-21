using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwakeZone : MonoBehaviour
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
        if (other.tag == "Player")
        {
            Enemy.AwakeEnemy();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Enemy.AwakeEnemy();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Enemy.AwakeEnemy();
        }
    }
}
