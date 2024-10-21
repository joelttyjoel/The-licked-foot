using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstergirlController : MonoBehaviour
{
    public Animator GirlAnimator;
    public int StateGirlAnimation;

    // Start is called before the first frame update
    void Start()
    {
        StateGirlAnimation = 0;
    }

    // Update is called once per frame
    void Update()
    {
           

        //Set animator variable to local
        GirlAnimator.SetInteger("GirlState_0down_1up_2jump", StateGirlAnimation);
    }
}
