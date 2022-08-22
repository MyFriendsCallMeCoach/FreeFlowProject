using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpOn, cpOff;

    public int soundToPlay;


    //Respawn Changed, checkpoint active
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            GameManager.instance.SetSpawnPoint(transform.position);
            Debug.Log("Checkpoint");


            //AudioManager.instance.PlaySFX(soundToPlay);

            foreach ( var cp in FindObjectsOfType<Checkpoint>() )
            {
                cp.cpOff.SetActive( true );
                cp.cpOn.SetActive( false );
            }

            cpOff.SetActive( false );
            cpOn.SetActive( true );
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameManager.instance.Respawn();
            /*//Debug.Log("This will be called every frame");
            if (Input.GetKeyDown("e") && other.tag == "Player")
            {
                GameManager.instance.Respawn();
            }*/
       

    }

}
