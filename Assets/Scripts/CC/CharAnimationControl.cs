using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class CharAnimationControl : MonoBehaviour
{
    AdvancedWalkerController controller;
    public Animator CharAnimator;

    public GameObject DustKickUp, LandingDust;

    public Transform RootPOS, LeftFootPOS,RightFootPOS;
    //private Vector3 FeetBill;

    public AudioSource L_Step, R_Step, landingAudio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AdvancedWalkerController>();
        CharAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //FeetBill = FootLocation;

        Vector3 _velocity = controller.GetVelocity();
        //Get controller grounded status;
        bool _isGrounded = controller.IsGrounded();
        //Pass values to animator component;
        CharAnimator.SetBool("IsGrounded", _isGrounded);
        CharAnimator.SetFloat("Speed", _velocity.magnitude);

        //We check to see if the player has double jumped
        if(controller.JumpsLeft <= 0)
        {
            CharAnimator.SetBool("DoubleJump", true);
            if((controller.JumpsLeft == 0))
            {
                //CharAnimator.SetBool("DoubleJump", false);
            }
        }
    }

    public void FootPar_1()
    {
        DustKickUpSpawnR();
    }

    public void FootPar_2()
    {
        DustKickUpSpawnL();
    }

    public void RL_Event()
    {
        LandingParticle();
    }


    public void Landing_Event()
    {
        LandingParticle();
    }


    public void DustKickUpSpawnL()
    {
        Instantiate(DustKickUp, new Vector3(LeftFootPOS.position.x, LeftFootPOS.position.y, LeftFootPOS.position.z), Quaternion.identity);
        L_Step.Play();
    }

    public void DustKickUpSpawnR()
    {
        Instantiate(DustKickUp, new Vector3(RightFootPOS.position.x, RightFootPOS.position.y, RightFootPOS.position.z), Quaternion.identity);
        R_Step.Play();
    }

    public void LandingParticle()
    {
        Instantiate(LandingDust, new Vector3(RootPOS.position.x, RootPOS.position.y, RootPOS.position.z), Quaternion.identity);
        landingAudio.Play();
    }

}
