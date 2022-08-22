using UnityEngine;

namespace FreeflowCombatSpace
{
    [System.Serializable]
    public class AttackAnimations
    {
        [Tooltip("The animation state name of the attack.")]
        public string animationName;
        [Tooltip("The distance from the enemy to stop at when playing this animation.")]
        public float attackDistance;
    }
}

