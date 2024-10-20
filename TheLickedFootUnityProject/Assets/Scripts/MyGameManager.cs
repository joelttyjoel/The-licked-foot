using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.PlayerSettings;

public class MyGameManager : MonoBehaviour
{
    public TvController ThisTvController;
    public MonstergirlController ThisMonsterGirlController;
    public Animator LeftHandAnimator;
    public int SecondsShowKidsshowStart = 20;
    public int BuzzingAfterKidsShow = 1;
    public int NewsTime = 66;

    private int Sequence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                break;
            case 3:
                
                break;
        }
    }

    IEnumerator FirstTvSequence()
    {
        ThisTvController.IsControlledByGameManager = true;
        //Play kids show for X time
        ThisTvController.TvStateSequence = 3;
        yield return new WaitForSeconds(SecondsShowKidsshowStart);
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
