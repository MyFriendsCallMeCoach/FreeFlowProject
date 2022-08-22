using UnityEngine;

public static class Extentions
{


    public static void CrossFade(this Animator _Animator, CrossFadeSettings settings)
    {
        _Animator.CrossFade(
            settings.StateName,
            settings.TransitionDuration,
            settings.Layer,
            settings.TimeOffset);
    }

    public static void CrossFadeInFixedTime(this Animator _Animator, CrossFadeSettings settings)
    {
        _Animator.CrossFadeInFixedTime(
            settings.StateName,
            settings.TransitionDuration,
            settings.Layer,
            settings.TimeOffset);
    }

}
