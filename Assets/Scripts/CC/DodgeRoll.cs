using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class DodgeRoll : MonoBehaviour
{
    

    public static DodgeRoll Instance;
    AdvancedWalkerController controller;
    public Animator CharAnimator;

    //public KeyCode TempRollInput = KeyCode.F;

    public float DelayIFrames = 0.2f;
    public float IFramesDur = 0.5f;

    public float DodgeCoolDown = 1f;
    private float ActCoolDown;

    public float RollDistance = 3f;

    public bool isDodge = false;

    float VerticalAim, HorizontalAim;

    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AdvancedWalkerController>();
        //CharAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DodgeFunction();

        VerticalAim = Mathf.Clamp(CharacterDefaultInput.instance.AimInput.y, -1, 1);
        HorizontalAim = Mathf.Clamp(CharacterDefaultInput.instance.AimInput.x, -1, 1);
    }

    public void DodgeFunction()
    {
        //isDodge = InputManager.instance.Dodge_Input;

        if(ActCoolDown <= 0f && AdvancedWalkerController.instance.currentControllerState == AdvancedWalkerController.ControllerState.Grounded)
        {
            CharAnimator.ResetTrigger("Dodge");
            if(CMF.CharacterDefaultInput.instance.Dodge_Input == true)
            {
                
                Dodge();
                FreeflowCombat.instance.StopAttacking();
            }
        }
        else
        {
            ActCoolDown -= Time.deltaTime;
        }
    }

    void Dodge()
    {
        //Vector3 _velocity = Vector3.zero;
        ActCoolDown = DodgeCoolDown;
        //Health.Invincible(DelayIFrames, IFramesDur)


        controller.AddMomentum(new Vector3(HorizontalAim,0, VerticalAim).normalized * RollDistance);//TurnTowardControllerVelocity.instance.tr.forward * RollDistance);

        CharAnimator.SetTrigger("Dodge");
    }

}

