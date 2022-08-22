using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZiplineCheck : MonoBehaviour
{
    [SerializeField] private float CheckZipOffset = 1f;
    [SerializeField] private float CheckZipRadius = 2f;

    // Update is called once per frame
    void Update()
    {
        if(CMF.AdvancedWalkerController.instance.currentControllerState == CMF.AdvancedWalkerController.ControllerState.Falling ||
            CMF.AdvancedWalkerController.instance.currentControllerState == CMF.AdvancedWalkerController.ControllerState.Rising || 
            CMF.AdvancedWalkerController.instance.currentControllerState == CMF.AdvancedWalkerController.ControllerState.Jumping)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, CheckZipOffset, 0), CheckZipRadius,Vector3.up);
            foreach(RaycastHit Hit in hits)
            {
                if(Hit.collider.tag == "Zipline")
                {
                    Hit.collider.GetComponent<Zipline>().StartZipping(gameObject);
                }
            }
        }
    }
}
