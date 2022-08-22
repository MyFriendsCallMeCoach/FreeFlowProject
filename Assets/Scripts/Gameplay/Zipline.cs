using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    [SerializeField] private Zipline TargetZip;
    [SerializeField] private float ZippingSpeed = 500f;

    [SerializeField] private float InteractThreshold = 1f;

    [SerializeField] private LineRenderer Cable;

    public Transform ZipTransform;
    public float ZipScale = 0.2f;

    private bool Zippling = false;
    private GameObject LocalZip;
    

    private void Awake()
    {
        Cable.SetPosition(0, ZipTransform.position);
        Cable.SetPosition(1, TargetZip.ZipTransform.position);
        //Cable = GetComponentInChildren<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Zippling || LocalZip == null) return;


        LocalZip.GetComponent<Rigidbody>().AddForce((TargetZip.ZipTransform.position - ZipTransform.position).normalized * ZippingSpeed * Time.deltaTime, ForceMode.Acceleration);
        if (Vector3.Distance(LocalZip.transform.position, TargetZip.ZipTransform.position) <= InteractThreshold || CMF.AdvancedWalkerController.instance.currentControllerState == CMF.AdvancedWalkerController.ControllerState.Jumping)
        {
            ResetZipline();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void StartZipping(GameObject _player)
    {
        if (Zippling) return;

        _player = GameObject.FindWithTag("GameController");
        LocalZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        LocalZip.transform.position = ZipTransform.position;
        LocalZip.transform.localScale = new Vector3(ZipScale, ZipScale, ZipScale);
        LocalZip.AddComponent<Rigidbody>().useGravity = false;
        LocalZip.GetComponent<Collider>().isTrigger = true;
        //CC_Capsule.enabled = false;
        //_player.GetComponent<Rigidbody>().useGravity = false;
        //_player.GetComponent<Rigidbody>().isKinematic = true;
        CMF.AdvancedWalkerController.instance.currentControllerState = CMF.AdvancedWalkerController.ControllerState.ZipLining;
        //CMF.CharacterDefaultInput.instance.playerControls.Disable();
        _player.transform.parent = LocalZip.transform;
        Zippling = true;

    }

    private void ResetZipline()
    {
        if (!Zippling) return;

        GameObject _player = LocalZip.transform.GetChild(0).gameObject;
        CMF.AdvancedWalkerController.instance.currentControllerState = CMF.AdvancedWalkerController.ControllerState.Falling;
        //CMF.CharacterDefaultInput.instance.playerControls.Enable();
        //CC_Capsule.enabled = true;
        _player.transform.parent = null;
        Destroy(LocalZip);
        LocalZip = null;
        Zippling = false;

        Debug.Log("Zipline Reset");
    }

}
