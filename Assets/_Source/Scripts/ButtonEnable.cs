/**
 * ButtonEnable.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class ButtonEnable : MonoBehaviour
{
    [SerializeField]
    ButtonEnableTargetInfo[] buttonEnableTargets;
    Light l;

    public void OnInteract()
    {
        for(int i=0; i < buttonEnableTargets.Length; i++)
        {
            buttonEnableTargets[i].targetScript.Activate();
        }
        l.enabled = true;
    }

    void OnValidate()
    {
        l = GetComponentInChildren<Light>();
        for (int i = 0; i < buttonEnableTargets.Length; i++)
        {
            buttonEnableTargets[i].name = buttonEnableTargets[i].targetScript.gameObject.name;
        }
    }
}