using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class PlayerLedgeChecker : MonoBehaviour
{
    public static PlayerLedgeChecker instance;

    [SerializeField] Transform HandPostion, StandPostion;

    [SerializeField] private float yOffset = 6.5f;

    private Vector3 NewHandPOS,NewHandROT;

    //public Component PlayerRotScript;

    public float rotatespeed;

    public Quaternion Q;

    public GameObject Ledge;

    // Start is called before the first frame update
    void Start()
    {
        //instance =s
        //Self = Self;
        //PlayerRotScript = GetComponentInChildren<TurnTowardControllerVelocity>();
        //we do this to make sure due to the player pivot and center may differ. A Y offset is added to adjust to the animations hand postion
        NewHandPOS = new Vector3(HandPostion.position.x, HandPostion.position.y - yOffset, HandPostion.position.z);
        NewHandROT = new Vector3(0, HandPostion.transform.rotation.y, 0);
        //NewHandPOS = new Vector3(0, HandPostion.transform.rotation.y, 0);
        //HandPostion.position.y = HandPostion.position.y - yOffset;
    }

    private void Update()
    {
        NewHandROT = Ledge.transform.position - transform.position;
        //Q = Quaternion.LookRotation(NewHandROT);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Q, Time.deltaTime * rotatespeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LedgeChecker"))
        {
            var player = other.GetComponentInParent<AdvancedWalkerController>();
            player.GrabLedge(NewHandPOS, Q/*HandPostion.rotation*/, this);

            AdvancedWalkerController.instance.CharAnimator.SetBool("LedgeGrabbed", true);

            //AdvancedWalkerController.instance.ledgetest();

            TurnTowardControllerVelocity.instance.tr.localRotation = HandPostion.rotation;
            TurnTowardControllerVelocity.instance.enabled = false;

            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("LedgeChecker"))
        {
            //TurnTowardControllerVelocity.instance.enabled = true;
        }
    }
}
