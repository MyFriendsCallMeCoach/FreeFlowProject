using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraStates : MonoBehaviour
{
    //public Animator CamAnim;

    public Animator CamAnim;
    public string GraphName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            Debug.Log("Entering camera volume");

            CamAnim.Play( GraphName );

        }
               
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            Debug.Log("Entering camera volume");

            CamAnim.Play(GraphName);

        }
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log( "OnTriggerExit" );

        if (other.CompareTag("GameController"))
        {
            Debug.Log("Leaving camera volume");
            //SetSaloon( false );
            //CamAnim.CrossFade( "DefaultCam", 0.0f, 0 );
            CamAnim.Play( "DefaultCam" );
        }
    }
}
