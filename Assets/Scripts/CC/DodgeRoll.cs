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
    public float ActCoolDown;

    public float RollDistance = 3f;

    public bool isDodge = false;

    public float VerticalAim, HorizontalAim;

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
              
            Dodge();
            
        }
        else
        {
            EnableScripts();
            ActCoolDown -= Time.deltaTime;
        }
    }

    void Dodge()
    {
        if(VerticalAim >= 0.5 || VerticalAim <= -0.5 || VerticalAim <= -0.5 || VerticalAim >= 0.5 || HorizontalAim >= 0.5 || HorizontalAim <= -0.5 || HorizontalAim <= -0.5 || HorizontalAim >= 0.5)
        {
            FreeflowCombat.instance.StopAttacking();
            //Vector3 _velocity = Vector3.zero;
            ActCoolDown = DodgeCoolDown;
            //Health.Invincible(DelayIFrames, IFramesDur)
            DisableScripts();
            MeshTrail.instance.PlayerMeshTrail();
            //controller.AddMomentum(new Vector3(HorizontalAim,0, VerticalAim).normalized * RollDistance);//TurnTowardControllerVelocity.instance.tr.forward * RollDistance);

            controller.transform.Translate(CharacterDefaultInput.instance.DodgeDirection * RollDistance * Time.deltaTime);


            CharAnimator.SetTrigger("Dodge");
        }
        
    }

    [Tooltip("Add the scripts you want to disable when attacking and they will be automatically re-enabled. You will most probably have to add your movement script.")]
    public MonoBehaviour[] scriptsToDisable;

    // method for reenabling the disabled scripts during attacking
    void EnableScripts()
    {
        foreach (var script in scriptsToDisable)
        {
            script.enabled = true;
        }
    }

    // method for disabling all set scripts in preparation for attacking
    void DisableScripts()
    {
        foreach (var script in scriptsToDisable)
        {
            script.enabled = false;
        }
    }


}

