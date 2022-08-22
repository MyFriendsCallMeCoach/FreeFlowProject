using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CMF
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterDefaultInput : CharacterInput
    {
        public Camera CamMain;
        CinemachineFreeLook CineFree;


        public PlayerControls playerControls;
        //P_Movement CC;
        //Anim_Manager anim_Manager;
        //BulletTimeController BTC;

        //Movement
        [Header("Movement")]
        public Vector2 MovementInput;
        public float VerticalMovement;
        public float HorizontalMovement;
        public float MoveAmount;

        public bool Dodge_Input;
        public bool Jump_Input;

        //Aimming
        [Header("Aimming")]
        public Vector2 AimInput;
        public float VerticalAim;
        public float HorizontalAim;
        public float AimAmount;

        /*[Header("Weapon Wheel")]
        public Vector2 WWInput;
        public float VerticalWW;
        public float HorizontalWW;*/

        //Combat
        [Header("Combat")]
        public bool Shoot_Right_Input;
        public bool Shoot_Left_Input;
        public bool BT_Input;
        public bool BT_On_Off = false;
        public bool ReloadRight_Input;
        public bool ReloadLeft_Input;
        public bool Interact_Input;

        [Header("Menus")]
        public bool Pause_Input;


        //bools
        public bool RightHandEquipped;

        public static CharacterDefaultInput instance;

        private void Awake()
        {
            CamMain = Camera.main;
            //CineFree = CamMain.GetComponent<CinemachineFreeLook>();
            instance = this;
            //BTC = GetComponent<BulletTimeController>();
            //anim_Manager = GetComponent<Anim_Manager>();
            //CC = GetComponent<P_Movement>();
            //Cursor.lockState = CursorLockMode.Locked;
        }

        /* private void Update()
         {
             if(GunSystem_R.Instance.isActiveAndEnabled)
             {
                 RightHandEquipped = true;
             }
             else
             {
                 RightHandEquipped = false;
             }
         }*/

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();

                playerControls.PlayerAiming.Aimming.performed += i => AimInput = i.ReadValue<Vector2>();

                //playerControls.PlayerAiming.WeaponWheel.performed += i => WWInput = i.ReadValue<Vector2>();

                //shoot right
                playerControls.PlayerActions.Shot_Right_Hand.performed += i => Shoot_Right_Input = true;
                playerControls.PlayerActions.Shot_Right_Hand.canceled += i => Shoot_Right_Input = false;

                //shoot left
                playerControls.PlayerActions.Shot_Left_Hand.performed += i => Shoot_Left_Input = true;
                playerControls.PlayerActions.Shot_Left_Hand.canceled += i => Shoot_Left_Input = false;

                playerControls.PlayerActions.BulletTime.performed += i => BT_Input = true;
                playerControls.PlayerActions.BulletTime.canceled += i => BT_Input = false;

                //reload right
                //playerControls.PlayerActions.Reload_Right_Hand.performed += i => ReloadRight_Input = true;
                //playerControls.PlayerActions.Reload_Right_Hand.canceled += i => ReloadRight_Input = false;

                //reload left
                playerControls.PlayerActions.Reload_Left_Hand.performed += i => ReloadLeft_Input = true;
                playerControls.PlayerActions.Reload_Left_Hand.canceled += i => ReloadRight_Input = false;

                //dodge
                playerControls.PlayerActions.Dodge.performed += i => Dodge_Input = true;
                playerControls.PlayerActions.Dodge.canceled += i => Dodge_Input = false;

                //jump
                playerControls.PlayerActions.Jump.performed += i => Jump_Input = true;

                //Interact
                playerControls.PlayerActions.Interact.performed += i => Interact_Input = true;
                playerControls.PlayerActions.Interact.canceled += i => Interact_Input = false;

                //Pause/Unpause
                playerControls.UIControl.UI_Pause.performed += i => Pause_Input = true;
                playerControls.UIControl.UI_Pause.canceled += i => Pause_Input = false;

            }



            playerControls.Enable();

        }


        private void OnDisable()
        {
            playerControls.Disable();
        }


        public void HandleAllInput()
        {
            HandleMovementInput();
            HandlePause();
            HandleDodgeInput();
            HandleAimInput();

            /*if (WeaponWheel.instance.OnOff == false)
            {

                HandleAimmingInput();
                //HandleSprintInput();
                //HandleDodgeInput();
                //HandleCombatInputs();
                //HandleInteractInput();
                //HandleReloadInput();

                //HandleBulletTimeInput();
            }
            else if (WeaponWheel.instance.OnOff == true)
            {
                //CineFree.IsValid(false);
            }*/

        }
        private void HandleAimInput()
        {
            VerticalAim = AimInput.y;
            HorizontalAim = AimInput.x;
            AimAmount = Mathf.Clamp01(Mathf.Abs(HorizontalAim) + Mathf.Abs(VerticalAim));
            //anim_Manager.UpdateAnimatorValues(HorizontalMovement, VerticalMovement, CC.isSprinting);
            //CharacterInput.GetVerticalMovementInput();
        }

        private void HandleMovementInput()
        {
            VerticalMovement = MovementInput.y;
            HorizontalMovement = MovementInput.x;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalMovement) + Mathf.Abs(VerticalMovement));
            //anim_Manager.UpdateAnimatorValues(HorizontalMovement, VerticalMovement, CC.isSprinting);
            //CharacterInput.GetVerticalMovementInput();
        }

        private void HandleDodgeInput()
        {
            if (AimInput.y >= 1 || AimInput.y <= -1 || AimInput.x <= -1 || AimInput.x >= 1)//Dodge_Input)
            {
                Dodge_Input = false; 

                DodgeRoll.Instance.DodgeFunction();

            }
        }
        
        private void HandlePause()
        {
            if(!Pause_Input)
            {
                //Pause_Input = false;
                GameManager.instance.PauseUnpause();
            }
            else
            {

            }
        }
        
		//If this is enabled, Up and Down for movement is inverted. Someone could just love south paw movement;
		public bool InvertMovement = false;

        public override float GetHorizontalMovementInput()
		{
			if(InvertMovement)
				return MovementInput.y;
            else
				return MovementInput.x;
        }

		public override float GetVerticalMovementInput()
		{
			if(InvertMovement)
				return MovementInput.x;
            else
				return MovementInput.y;
        }

		public override bool IsJumpKeyPressed()
		{
			return Jump_Input;
        }

        



    }



}
