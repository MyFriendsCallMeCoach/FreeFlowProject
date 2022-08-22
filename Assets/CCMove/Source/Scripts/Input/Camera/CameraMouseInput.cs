using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This camera input class is an example of how to get input from a connected mouse using Unity's default input system;
    //It also includes an optional mouse sensitivity setting;
    public class CameraMouseInput : CameraInput
    {
        //input axes;
        [Header("Mouse Input Axis")]
        public string mouseHorizontalAxis = "Mouse X";
        public string mouseVerticalAxis = "Mouse Y";
        
        //Invert input options;
        public bool invertHorizontalMouse = false;
        public bool invertVerticalMouse = false;

        //Use this value to fine-tune mouse movement;
        //All mouse input will be multiplied by this value;
        public float mouseInputMultiplier = 0.01f;

        [Header("RightStick Input Axis")]
        public string ConHorizontalAxis = "RightStick X";
        public string ConVerticalAxis = "RightStick Y";

        //Invert input options;
        public bool invertHorizontalCon = false;
        public bool invertVerticalCon = false;

        //Use this value to fine-tune con movement;
        //All Con input will be multiplied by this value;
        public float ConInputMultiplier = 0.01f;

        //If this is enabled, Unity's internal input smoothing is bypassed;
        public bool useRawInput = true;

        [Header("Controller or Mouse")]
        public bool ConOn;

        public override float GetHorizontalCameraInput()
        {
            float _input;
            float _ConInput;
            //Get raw mouse input;
            if (useRawInput)
            {
                 _input = Input.GetAxisRaw(mouseHorizontalAxis);
                 _ConInput = Input.GetAxisRaw(ConHorizontalAxis);
            }
            else
            {
                _input = Input.GetAxis(mouseHorizontalAxis);
                _ConInput = Input.GetAxis(ConHorizontalAxis);
            }

            
            //Since raw mouse input is already time-based, we need to correct for this before passing the input to the camera controller;
            if(Time.timeScale > 0f && Time.deltaTime > 0f)
            {
                _input /= Time.deltaTime;
                _input *= Time.timeScale;
            }
            else
            {
                _input = 0f;
                _ConInput = 0f;
            }


            //Apply mouse sensitivity;
            _input *= mouseInputMultiplier;
            _ConInput *= ConInputMultiplier;

            //Invert input;
            if (invertHorizontalMouse)
            {
                _input *= -1f;
            }

            if (invertHorizontalCon)
            {
                _ConInput *= -1f;
            }

            if(ConOn)
            {
                return _ConInput;
            }
            else
            {
                return _input;
            }
        }

        public override float GetVerticalCameraInput()
        {
            //Get raw mouse input;

            float _input;
            float _ConInput;

            if (useRawInput)
            {
                _input = -Input.GetAxisRaw(mouseVerticalAxis);
                _ConInput = -Input.GetAxisRaw(ConVerticalAxis);
            }
            else
            {
                _input = -Input.GetAxis(mouseVerticalAxis);
                _ConInput = -Input.GetAxis(ConVerticalAxis);
            }
            
            
            //Since raw mouse input is already time-based, we need to correct for this before passing the input to the camera controller;
            if(Time.timeScale > 0f && Time.deltaTime > 0f)
            {
                _input /= Time.deltaTime;
                _input *= Time.timeScale;
            }
            else
            {
                _input = 0f;
                _ConInput = 0f;
            }


            //Apply mouse sensitivity;
            _input *= mouseInputMultiplier;
            _ConInput *= ConInputMultiplier;

            //Invert input;
            if(invertVerticalMouse)
            {
                _input *= -1f;
            }

            if (invertVerticalCon)
            {
                _ConInput *= -1f;
            }

            if(ConOn)
            {
                return _ConInput;
            }
            else
            {
                return _input;
            }

        }
    }
}
