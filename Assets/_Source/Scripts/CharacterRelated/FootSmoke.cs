/**
 * FootSmoke.cs
 * Created by: Pedro Borges
 * Created on: 02/05/19 (dd/mm/yy)
 */

using UnityEngine;

public class FootSmoke : MonoBehaviour
{
    public GameObject smokeLeft;
    public GameObject smokeRight;
    public Transform rightTransform;
    public Transform leftTransform;
    public void SpawnRight()
    {
        Instantiate(smokeRight,rightTransform.position,Quaternion.identity,null);
    }
    public void SpawnLeft()
    {
        GameObject smoke = Instantiate(smokeLeft,leftTransform);

        rightTransform.DetachChildren();

    }
}