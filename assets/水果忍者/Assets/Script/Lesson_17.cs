using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson_17 : MonoBehaviour
{

    private int colliderCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //碰撞发生的时候会执行该函数
    void OnCollisionEnter2D(Collision2D coll)
    {
        colliderCount++;
        Destroy(coll.gameObject);
        if(colliderCount==3)
        {
            print("hello");
        }
    }
}
