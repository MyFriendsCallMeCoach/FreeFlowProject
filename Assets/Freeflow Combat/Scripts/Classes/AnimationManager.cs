using UnityEngine;

namespace FreeflowCombatSpace
{
    public class AnimationManager
    {
        string currentState;
        Animator anim;

        //constructor
        public AnimationManager (Animator animator)
        {
            anim = animator;
        }


        //actual animation playing function
        public void Play(string state, float time = 0.15f)
        {
            if (currentState == state || state.Length == 0) return;
            
            anim.CrossFadeInFixedTime(state, time, 0);
            currentState = state;
        }
    }
}
