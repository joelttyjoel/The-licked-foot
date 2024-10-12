using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseDragFeet : MonoBehaviour
{
    public Scene ThisScene;
    public string FootName;
    public int Number;

    private Camera TheCamera;

    private Vector3 ThisObjectScreenPoint;
    private Vector3 MouseScreenPoint;

    // Start is called before the first frame update
    void Start()
    {
        TheCamera = ThisScene.Gameobject.Find("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        ThisObjectScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        MouseScreenPoint = TheCamera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        transform.x = MouseScreenPoint.Vector3.x;
        transform.y = MouseScreenPoint.Vector3.y;
    }
}
