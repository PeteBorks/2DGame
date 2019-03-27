/**
 * ButtonAction.cs
 * Created by: Pedro Borges
 * Created on: 25/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    ButtonActionTargetInfo[] buttonActionTargets;
    public float transitionDuration = 1;
    public AnimationCurve transitionCurve;

    public IEnumerator OnInteract()
    {
        float time = 0;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            for(int i=0; i < buttonActionTargets.Length; i++)
            {
                buttonActionTargets[i].buttonTarget.localPosition = Vector3.Lerp(buttonActionTargets[i].origin, buttonActionTargets[i].target, transitionCurve.Evaluate(time / transitionDuration));
            }
            yield return null;
        }            
    }

    void OnValidate()
    {
        if(buttonActionTargets != null && buttonActionTargets.Length != 0)
        {
            for(int i=0; i < buttonActionTargets.Length; i++)
            {
                if (buttonActionTargets[i].buttonTarget)
                {
                    buttonActionTargets[i].name = buttonActionTargets[i].buttonTarget.name;
                    buttonActionTargets[i].origin = buttonActionTargets[i].buttonTarget.localPosition;
                }
            }
        }
    }
}