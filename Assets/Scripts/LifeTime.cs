using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [Tooltip("The amount of time before the game object is deleted"), Range(0f, 10f)]
    public float LifeTimeAward = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimeAward -= Time.deltaTime;
        if (LifeTimeAward <= 0)
        {
            Destroy(gameObject);
        }
    }
}
