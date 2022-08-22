using UnityEngine;

namespace FreeflowCombatSpace
{
    public class FlagAttack : MonoBehaviour
    {
        public bool hitting = false;

        public void Hit()
        {
            hitting = true;
        }

        public void StopHit()
        {
            hitting = false;
        }
    }
}
