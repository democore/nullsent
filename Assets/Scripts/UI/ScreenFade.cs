using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : Utility.Singleton<ScreenFade> {

    public Animator anim;

    void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }

    void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }
}
