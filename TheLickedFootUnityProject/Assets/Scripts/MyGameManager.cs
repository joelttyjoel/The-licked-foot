using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public Text ClockText;
    public GameObject InfoTv;
    public GameObject InfoFeet;
    public PlayerMouseDragFeet FootLeft;
    public PlayerMouseDragFeet FootRight;
    public Slider HeatSlider;
    public LightningManager MyLightning;
    public DoorController MyDoor;
    public TvController ThisTvController;
    public MonstergirlController ThisMonsterGirlController;
    public Animator LeftHandAnimator;
    public int SecondsShowKidsshowStart = 20;
    public int BuzzingAfterKidsShow = 1;
    public int NewsTime = 66;
    public float CurrentHeatValue = 0;
    public float TickTimeHeat = 0.2f;
    public float IncreaseHeatByXEveryUpdate = 0.001f;
    public float DecreaseHeatByXEveryUpdate = 0.002f;
    public float CurrentLimitTimeFeetIncreaseHeat = 20;//seconds
    public float CurrentDarknessValue = 0;
    public float TickTimeDarkness = 0.2f;
    public float IncreaseDarknessByXEveryUpdate = 0.001f;

    private float TimerLoopingHeat;

    private float TimerLoopingDarkness;

    private bool HeatAddDarkness;

    private float EndTimer = 0;

    public float TimeSecondsIncreaseMinuteClock = 1;
    private int CurrentClockTime4;
    private int CurrentClockTime3;
    private int CurrentClockTime2;
    private int CurrentClockTime1;

    private float SecondTimer;

    public int Sequence;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHeatValue = 0;

        InfoTv.SetActive(true);
        InfoFeet.SetActive(true);

        CurrentClockTime4 = 0;
        CurrentClockTime3 = 1;
        CurrentClockTime2 = 0;
        CurrentClockTime1 = 0;
        ClockText.text = CurrentClockTime4.ToString() + CurrentClockTime3.ToString() + ":" + CurrentClockTime2.ToString() + CurrentClockTime1.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Send game severity to tv
        ThisTvController.GameSeverityFromGamemanager = CurrentDarknessValue;

        FootLeft.InputFromMasterDarkness = CurrentDarknessValue;

        FootRight.InputFromMasterDarkness = CurrentDarknessValue;

        switch (Sequence)
        {
            case 0:
                StartCoroutine(FirstTvSequence());
                Sequence = 1;
                break;
            case 1:
                //Wait for tv controll shit
                break;
            case 2:
                //After intro with tv
                //If darkness above X level, go to next stage
                if(CurrentDarknessValue > 0.1)
                {
                    Sequence = 3;
                }
                break;
            case 3:
                //Darkness 10%
                MyLightning.EnableLightningFromMaster = true;
                if (CurrentDarknessValue > 0.30)
                {
                    Sequence = 4;
                }
                break;
            case 4:
                //Just fire off lighting lady
                MyLightning.DoLadyLightningAsap = true;
                Sequence = 5;
                break;
            case 5:
                //Darkness 30%
                if (CurrentDarknessValue > 0.40)
                {
                    Sequence = 6;
                    MyDoor.FromMasterDoOpenDoor = true;
                }
                break;
            case 6:
                //Just fire off the door
                Sequence = 7;
                break;
            case 7:
                //Darkness 40%
                if (CurrentDarknessValue > 0.50)
                {
                    Sequence = 8;
                    MyLightning.AllowTurnOnBloodFromMaster = true;
                }
                break;
            case 8:
                //Darkness 50%
                if (CurrentDarknessValue > 0.70)
                {
                    Sequence = 9;
                }
                break;
            case 9:
                //Darkness 70%
                if (CurrentDarknessValue > 0.80)
                {
                    Sequence = 10;

                    LeftHandAnimator.SetBool("HandBeUpOrDown_0down1up", true);
                }
                break;
            case 10:
                //Darkness 80%
                if (CurrentDarknessValue > 0.88)
                {
                    Sequence = 12;
                    ThisMonsterGirlController.StateGirlAnimation = 1;
                }
                break;
            //no return
            case 12:
                //Darkness 95-100%
                ThisTvController.IsControlledByGameManager = true;
                ThisTvController.TvStateSequence = 1;
                if (CurrentDarknessValue > 0.94)
                {
                    ThisMonsterGirlController.StateGirlAnimation = 2;
                    Sequence = 13;
                }
                break;
            case 13:
                EndTimer = EndTimer + Time.deltaTime;
                if (EndTimer > 5)
                {
                    Application.Quit();
                }
                break;
        }
        //Calculate darkness
        //Calculate increasing darknes values
        if (TimerLoopingDarkness < 0)
        {
            TimerLoopingDarkness = TickTimeDarkness;

            if (ThisTvController.TvStateSequence == 0 || Sequence > 11)
            {
                CurrentDarknessValue = CurrentDarknessValue + IncreaseDarknessByXEveryUpdate;
            }
        }
        if (CurrentDarknessValue > 1)
        {
            CurrentDarknessValue = 1;
        }
        TimerLoopingDarkness = TimerLoopingDarkness - Time.deltaTime;

        //Get value from feet, turn up or down heat slider
        if (TimerLoopingHeat < 0)
        {
            TimerLoopingHeat = TickTimeHeat;

            if (FootLeft.TimeOutsideBlanket > CurrentLimitTimeFeetIncreaseHeat)
            {
                CurrentHeatValue = CurrentHeatValue + IncreaseHeatByXEveryUpdate;
            }
            if (FootRight.TimeOutsideBlanket > CurrentLimitTimeFeetIncreaseHeat)
            {
                CurrentHeatValue = CurrentHeatValue + IncreaseHeatByXEveryUpdate;
            }
            if (FootLeft.TimeOutsideBlanket < CurrentLimitTimeFeetIncreaseHeat & FootRight.TimeOutsideBlanket < CurrentLimitTimeFeetIncreaseHeat)
            {
                CurrentHeatValue = CurrentHeatValue - DecreaseHeatByXEveryUpdate;
            }

            if(HeatAddDarkness)
            {
                CurrentDarknessValue = CurrentDarknessValue + IncreaseDarknessByXEveryUpdate;
            }
        }
        TimerLoopingHeat = TimerLoopingHeat - Time.deltaTime;
        if(CurrentHeatValue < 0)
        {
            CurrentHeatValue = 0;
        }
        if (CurrentHeatValue > 1)
        {
            CurrentHeatValue = 1;
        }
        HeatSlider.value = CurrentHeatValue;

        //If heat is above X value, increase darkness fast
        if(CurrentHeatValue > 0.95)
        {
            HeatAddDarkness = true;
        }
        else
        {
            HeatAddDarkness = false;
        }

        //Just spaghetti do clock update
        if(SecondTimer > TimeSecondsIncreaseMinuteClock)
        {
            //Every second
            CurrentClockTime1 = CurrentClockTime1 + 1;
            if(CurrentClockTime1 > 9)
            {
                CurrentClockTime1 = 0;
                CurrentClockTime2 = CurrentClockTime2 + 1;
            }
            if(CurrentClockTime2 > 5)
            {
                CurrentClockTime2 = 0;
                CurrentClockTime3 = CurrentClockTime3 + 1;
            }

            ClockText.text = CurrentClockTime4.ToString() + CurrentClockTime3.ToString() + ":" + CurrentClockTime2.ToString() + CurrentClockTime1.ToString();

            SecondTimer = 0;
        }    

        SecondTimer = SecondTimer + Time.deltaTime;
    }

    IEnumerator FirstTvSequence()
    {
        ThisTvController.IsControlledByGameManager = true;
        //Play kids show for X time
        ThisTvController.TvStateSequence = 3;
        yield return new WaitForSeconds(SecondsShowKidsshowStart - 3);
        //Turn off info
        InfoTv.SetActive(false);
        InfoFeet.SetActive(false);
        yield return new WaitForSeconds(3);
        //Do buzzing for Y time
        ThisTvController.TvStateSequence = 1;
        yield return new WaitForSeconds(BuzzingAfterKidsShow);
        //Do News for Z time
        ThisTvController.TvStateSequence = 2;
        yield return new WaitForSeconds(NewsTime);
        //Turn off tv without blood, now game based
        ThisTvController.TvStateSequence = 0;
        Sequence = 2;
        ThisTvController.IsControlledByGameManager = false;
    }
}
