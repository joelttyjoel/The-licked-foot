using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TvController : MonoBehaviour
{
    public GameObject BloodOnWall;
    public SpriteRenderer TvScreenSpriteRenderer;
    public Sprite TvOff;
    public Sprite TvStatic1;
    public Sprite TvStatic2;
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

    public int TvStateSequence; //0 = off, 1 = static, 2 = news, 3 = cartoon

    public bool EnableBlood;

    private bool IsSlectingStaticTv;
    private bool SelectStaticTv;

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
        switch (TvStateSequence)
        {
            case 0:
                TvScreenSpriteRenderer.sprite = TvOff;
                //Sound play
                TvAudioSource.Stop();
                //light
                lightTv.color = LightOff;
                //show blood
                if (EnableBlood)
                {
                    BloodOnWall.SetActive(true);
                }
                else
                {
                    BloodOnWall.SetActive(false);
                }
                break;
            case 1:
                if(!IsSlectingStaticTv)
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
                //show blood
                if(EnableBlood)
                {
                    BloodOnWall.SetActive(true);
                }
                else
                {
                    BloodOnWall.SetActive(false);
                }
                break;
            case 2:
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
    }

    IEnumerator StaticTvCoroutine()
    {
        IsSlectingStaticTv = true;

        yield return new WaitForSeconds(Random.Range(FlickerTimeMin, FlickerTimeMax));

        SelectStaticTv = !SelectStaticTv;

        IsSlectingStaticTv = false;
    }
}
