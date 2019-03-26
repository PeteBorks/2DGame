/**
 * Button.cs
 * Created by: Pedro Borges
 * Created on: 25/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    public bool use2Doors = false;
    public bool usePlatform = false;
    bool interactPlatform = false;
    bool doors = false;

    public GameObject door1;
    public GameObject door2;
    public GameObject platform;

    public float door1TargetY = 3.25f;
    public float door2TargetY = 3.25f;
    public float platformTargetX = 8;
    public float lerpTime = 5f;

    Vector2 door1CurrentPos;
    Vector2 door2CurrentPos;
    Vector2 platformCurrentPos;
    Vector2 door1Target;
    Vector2 door2Target;
    Vector2 platformTarget;

    private void Start()
    {
        door1CurrentPos = door1.transform.localPosition;
        door2CurrentPos = door2.transform.localPosition;
        platformCurrentPos = platform.transform.localPosition;
        door1Target = new Vector3(door1CurrentPos.x, door1TargetY, 0);
        door2Target = new Vector3(door2CurrentPos.x, door2TargetY, 0);
        platformTarget = new Vector3(platformTargetX, platformCurrentPos.y, 0);
    }

    public IEnumerator OnInteract()
    {
        if (platform)
        {
            interactPlatform = true;
            yield return new WaitForSeconds(lerpTime / 1.5f);
            doors = true;
        }
        else
        {
            doors = true;
            yield return new WaitForSeconds(lerpTime);
        }
            
    }

    private void Update()
    {
        if(interactPlatform)
        {
            platform.transform.localPosition = Vector3.Lerp(platform.transform.localPosition, platformTarget, Time.deltaTime * lerpTime / 2);
        }
        if(doors)
        {
            door1.transform.localPosition = Vector3.Lerp(door1.transform.localPosition, door1Target, Time.deltaTime * lerpTime / 2);
            door2.transform.localPosition = Vector3.Lerp(door2.transform.localPosition, door2Target, Time.deltaTime * lerpTime / 2);
        }
    }

}