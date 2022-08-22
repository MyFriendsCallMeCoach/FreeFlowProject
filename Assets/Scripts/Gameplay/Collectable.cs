using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int AddAmount = 1;

    public GameObject CollectableObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            GameManager.instance.AddCoins(AddAmount);
            Destroy(CollectableObj);
            Debug.Log("Collect");
        }


    }

}
