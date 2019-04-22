/**
 * LookAt.cs
 * Created by: Pedro Borges
 * Created on: 25/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class LookAt : MonoBehaviour
{
    float dirNum;
    [SerializeField]
    SpriteRenderer sprite;

    [HideInInspector]
    public Light light1;
    [HideInInspector]
    public Light light2;
    public GameObject target;
    public bool isOn = true;

    void Update()
    {
        if(isOn && target)
        {
            Vector3 heading = target.transform.position - transform.position;
            switch (AngleDir(transform.forward, heading, transform.up))
            {
                case 1:
                    if (sprite.flipX)
                    {
                        lightsRight();
                    }

                    sprite.flipX = false;
                    break;

                case -1:
                    if (!sprite.flipX)
                    {
                        lightsLeft();
                    }
                    sprite.flipX = true;
                    break;
            }
        }
        
    }

    public void lightsRight()
    {
        light1.transform.localPosition = new Vector3(0.11f, light1.transform.localPosition.y, -0.03f);
        light2.transform.localPosition = new Vector3(0.34f, light2.transform.localPosition.y, -0.03f);
    }

    public void lightsLeft()
    {
        light1.transform.localPosition = new Vector3(-0.11f, light1.transform.localPosition.y, -0.03f);
        light2.transform.localPosition = new Vector3(-0.34f, light2.transform.localPosition.y, -0.03f);
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}