using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TvController : MonoBehaviour
{
    public GameObject BloodOnWall;
    public SpriteRenderer TvScreenSpriteRenderer;
    public Sprite TvOff;
    public Sprite TvStatic1;
    public Sprite TvStatic2;
    public SpriteRenderer Button;
    public Sprite OnButton;
    public Sprite OffButton;
    public float LimitNewsVoice = 1f;
    public Sprite TvNews1;
    public Sprite TvNews2;
    public float TimePerFrame = 0.1f;
    public float Frames = 5;
    public Sprite Cartoon1;
    public Sprite Cartoon2;
    public Sprite Cartoon3;
    public Sprite Cartoon4;
    public Sprite Cartoon5;
    public AudioSource TvAudioSource;
    public AudioClip StaticClip;
    public AudioClip News;
    public AudioClip Cartoon;
    public Light2D lightTv;
    public Color LightOff;
    public Color LightStatic1;
    public Color LightStatic2;
    public Color LightNews;
    public Color LightCartoon;
    public float FlickerTimeMin = 0.1f;
    public float FlickerTimeMax = 0.3f;
    public float MaxOnTimeTvByUser = 15f;
    public float MinOnTimeTvByUser = 2f;

    public int TvStateSequence; //0 = off, 1 = static, 2 = news, 3 = cartoon

    //input from gamemanager
    public float GameSeverityFromGamemanager; //0-1

    public bool EnableBlood;
    public bool IsControlledByGameManager;

    private bool IsSlectingStaticTv;
    private bool SelectStaticTv;

    private bool TvStartedByPlayer;
    private float CurrentTimeLeftOnByUser;

    private bool IsPlayingStatic;
    private bool isPlayingNews;
    private bool isPlayingCartoon;

    private Sprite[] FrameArray = new Sprite[5];
    private int FrameCartoon;
    private float CartoonTimer;

    private float updateStep = 0.1f;
    private float clipLoudness;
    private int sampleDataLength = 1024;
    private float[] clipSampleData;

    // Start is called before the first frame update
    void Start()
    {
        TvScreenSpriteRenderer.sprite = TvOff;

        TvAudioSource.Stop();

        lightTv.color = LightOff;

        FrameArray[0] = Cartoon1;
        FrameArray[1] = Cartoon2;
        FrameArray[2] = Cartoon3;
        FrameArray[3] = Cartoon4;
        FrameArray[4] = Cartoon5;

        clipSampleData = new float[sampleDataLength];

        BloodOnWall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //show blood
        if (EnableBlood)
        {
            BloodOnWall.SetActive(true);
        }
        else
        {
            BloodOnWall.SetActive(false);
        }

        switch (TvStateSequence)
        {
            case 0:
                //Button on off
                Button.sprite = OffButton;

                TvScreenSpriteRenderer.sprite = TvOff;
                //Sound play
                TvAudioSource.Stop();
                //light
                lightTv.color = LightOff;
                break;
            case 1:
                //Button on off
                Button.sprite = OnButton;

                if (!IsSlectingStaticTv)
                {
                    StartCoroutine(StaticTvCoroutine());
                }
                if (SelectStaticTv)
                {
                    TvScreenSpriteRenderer.sprite = TvStatic1;
                    lightTv.color = LightStatic1;
                }
                else
                {
                    TvScreenSpriteRenderer.sprite = TvStatic2;
                    lightTv.color = LightStatic2;
                }
                //Sound play
                isPlayingNews = false;
                isPlayingCartoon = false;
                if (!IsPlayingStatic)
                {
                    TvAudioSource.Stop();
                    IsPlayingStatic = true;
                }
                else
                {
                    TvAudioSource.loop = true;
                    TvAudioSource.clip = StaticClip;
                    if (!TvAudioSource.isPlaying)
                    {
                        TvAudioSource.Play();
                    }
                }
                break;
            case 2:
                //Button on off
                Button.sprite = OnButton;
                //Sound play
                IsPlayingStatic = false;
                isPlayingCartoon = false;
                if (!isPlayingNews)
                {
                    TvAudioSource.Stop();
                    isPlayingNews = true;
                }
                else
                {
                    TvAudioSource.loop = false;
                    TvAudioSource.clip = News;
                    if(!TvAudioSource.isPlaying)
                    {
                        TvAudioSource.Play();
                    }
                }
                //Color
                lightTv.color = LightNews;

                //Get loudness
                TvAudioSource.clip.GetData(clipSampleData, TvAudioSource.timeSamples);
                clipLoudness = 0f;
                foreach (var sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }
                clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
                if (clipLoudness > LimitNewsVoice)
                {
                    TvScreenSpriteRenderer.sprite = TvNews1;
                }
                else
                {
                    TvScreenSpriteRenderer.sprite = TvNews2;
                }
                //show blood
                BloodOnWall.SetActive(false);
                break;
            case 3:
                //Button on off
                Button.sprite = OnButton;

                CartoonTimer = CartoonTimer - Time.deltaTime;
                if(CartoonTimer < 0)
                {
                    TvScreenSpriteRenderer.sprite = FrameArray[FrameCartoon];

                    CartoonTimer = TimePerFrame;
                    FrameCartoon++;
                    if(FrameCartoon > Frames - 1)
                    {
                        FrameCartoon = 0;
                    }
                }
                //Sound play
                IsPlayingStatic = false;
                isPlayingNews = false;
                if (!isPlayingCartoon)
                {
                    TvAudioSource.Stop();
                    isPlayingCartoon = true;
                }
                else
                {
                    TvAudioSource.loop = false;
                    TvAudioSource.clip = Cartoon;
                    if (!TvAudioSource.isPlaying)
                    {
                        TvAudioSource.Play();
                    }
                }
                //Color
                lightTv.color = LightCartoon;
                //show blood
                BloodOnWall.SetActive(false);
                break;
        }

        //tv started by player
        if(!IsControlledByGameManager)
        {
            if (TvStartedByPlayer & CurrentTimeLeftOnByUser > 0)
            {
                CurrentTimeLeftOnByUser = CurrentTimeLeftOnByUser - Time.deltaTime;

                if (GameSeverityFromGamemanager > 0.5f)
                {
                    TvStateSequence = 1;
                }
                else
                {
                    TvStateSequence = 3;
                }
            }
            else
            {
                TvStateSequence = 0;
            }
        }
    }

    void OnMouseUp()
    {
        if(!IsControlledByGameManager & TvStateSequence == 0)
        {
            //Start tv for X seconds
            TvStartedByPlayer = true;
            CurrentTimeLeftOnByUser = (1 - GameSeverityFromGamemanager) * MaxOnTimeTvByUser;
            
            if(CurrentTimeLeftOnByUser < MinOnTimeTvByUser)
            {
                CurrentTimeLeftOnByUser  = MinOnTimeTvByUser;
            }
        }
    }

    IEnumerator StaticTvCoroutine()
    {
        IsSlectingStaticTv = true;

        yield return new WaitForSeconds(Random.Range(FlickerTimeMin, FlickerTimeMax));

        SelectStaticTv = !SelectStaticTv;

        IsSlectingStaticTv = false;
    }
}
