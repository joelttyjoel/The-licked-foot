using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    public TvController TheTv;
    public AudioSource LightiningSource;
    public Animator LightningAnimation;
    public float MinSecondsBetweenLightnings;
    public float MaxSecondsBetweenLightnings;

    public bool EnableLightningFromMaster;
    public bool DoLadyLightningAsap;
    public bool AllowTurnOnBloodFromMaster;

    private float CurrentTimeLeftNoLightning;

    // Start is called before the first frame update
    void Start()
    {
        AllowTurnOnBloodFromMaster = false;
    }

    // Update is called once per frame
    void Update()
    {
        //special trigger with fat bitch lightining
        if(DoLadyLightningAsap)
        {
            //Rerset light timer
            CurrentTimeLeftNoLightning = UnityEngine.Random.Range(MinSecondsBetweenLightnings, MaxSecondsBetweenLightnings);

            LightiningSource.Play();

            LightningAnimation.SetInteger("DoFlash_0NoFlash_1FlashWithout_2FlashWith", 2);

            DoLadyLightningAsap = false;
        }

        CurrentTimeLeftNoLightning = CurrentTimeLeftNoLightning - Time.deltaTime;

        if (EnableLightningFromMaster & CurrentTimeLeftNoLightning < 0)
        {
            //Make lightning
            CurrentTimeLeftNoLightning = UnityEngine.Random.Range(MinSecondsBetweenLightnings, MaxSecondsBetweenLightnings);

            LightiningSource.Play();

            LightningAnimation.SetInteger("DoFlash_0NoFlash_1FlashWithout_2FlashWith", 1);

            if(AllowTurnOnBloodFromMaster)
            {
                TheTv.EnableBlood = true;
            }
        }

        //If not equal to idle, then reset
        if (!LightningAnimation.GetCurrentAnimatorStateInfo(0).IsName("Flash_Idle"))
        {
            LightningAnimation.SetInteger("DoFlash_0NoFlash_1FlashWithout_2FlashWith", 0);
        }
    }
}
