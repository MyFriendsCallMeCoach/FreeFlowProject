using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    [Tooltip("This is how attractive this objective finds the player. A higher numbers makes the object find them more dashing")]
     public float AttractSpeed = 10f;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            transform.position = Vector3.MoveTowards(transform.position,other.transform.position, AttractSpeed * Time.deltaTime);
        }
    }
}
