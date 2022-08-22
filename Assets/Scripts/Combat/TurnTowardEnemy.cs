using DG.Tweening;
using FreeflowCombatSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardEnemy : MonoBehaviour
{
    public static TurnTowardEnemy instance;

    private void Awake()
    {
        instance = this;
    }

    public void RoatateToEnemy(FreeflowCombatEnemy target, float duration)
    {
        //OnTrajectory.Invoke(target);
        transform.DOLookAt(target.transform.position, .2f);
        //transform.DOMove(TargetOffset(target.transform), duration);
    }
}
