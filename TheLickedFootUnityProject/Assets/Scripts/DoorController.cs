using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioSource DoorSound;
    public SpriteRenderer SpriteDoor;
    public float OpenTimeDoor = 1.5f;

    public bool FromMasterDoOpenDoor;

    // Start is called before the first frame update
    void Start()
    {
        SpriteDoor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(FromMasterDoOpenDoor)
        {
            FromMasterDoOpenDoor = false;

            StartCoroutine(DoorOpenSequence());
        }
    }

    IEnumerator DoorOpenSequence()
    {
        SpriteDoor.enabled = true;
        DoorSound.Play();

        yield return new WaitForSeconds(OpenTimeDoor);

        SpriteDoor.enabled = false;
    }
}
