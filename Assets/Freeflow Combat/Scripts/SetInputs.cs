using UnityEngine;

namespace FreeflowCombatSpace
{
    public class SetInputs : MonoBehaviour
    {
        FreeflowCombat freeFlowCombat;
        //public AudioSource whooshSound;
        //public TrailRenderer tr;

        // Start is called before the first frame update
        void Start()
        {
            freeFlowCombat = GetComponent<FreeflowCombat>();
        }

        // Update is called once per frame
        void Update()
        {
            //if (CMF.InputManager.instance.Shoot_Right_Input) {
                //freeFlowCombat.Attack();
                //if (freeFlowCombat.isTraversing) {
                    //if (!whooshSound.isPlaying) whooshSound.Play();
                //}
            //}

            //if (freeFlowCombat.isTraversing || freeFlowCombat.isAttacking) tr.enabled = true;
            //else tr.enabled = false;

            // SETTING THE INPUTS
            freeFlowCombat.xInput = CMF.CharacterDefaultInput.instance.MovementInput.x;
            freeFlowCombat.yInput = CMF.CharacterDefaultInput.instance.MovementInput.y;
        }
    }
}

