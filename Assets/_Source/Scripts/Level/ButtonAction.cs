/**
 * ButtonAction.cs
 * Created by: Pedro Borges
 * Created on: 25/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

[SelectionBase]
public class ButtonAction : MonoBehaviour
{
    [SerializeField]
    float timeClock;
    [SerializeField]
    ButtonActionTargetInfo[] buttonActionTargets;
    public float transitionDuration = 1;
    
    public AnimationCurve transitionCurve;
    public Sprite activated;
    public Sprite targetSprite;
    SpriteRenderer sprite;
    Sprite defaultSprite;
    Sprite defaultTargetSprite;
    Light [] l;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        l = GetComponentsInChildren<Light>();
        defaultSprite = sprite.sprite;
        defaultTargetSprite = buttonActionTargets[0].buttonTarget.GetComponent<SpriteRenderer>().sprite;
    }

    public IEnumerator OnInteract()
    {
        if(activated)
            sprite.sprite = activated;
        foreach(Light light in l)
            light.color = Color.green;
        float time = 0;
      
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            for(int i=0; i < buttonActionTargets.Length; i++)
            {
                buttonActionTargets[i].buttonTarget.localPosition = Vector3.Lerp(buttonActionTargets[i].origin, buttonActionTargets[i].target, transitionCurve.Evaluate(time / transitionDuration));
                if (buttonActionTargets[i].useDifSprite)
                {
                    buttonActionTargets[i].buttonTarget.GetComponent<SpriteRenderer>().sprite = targetSprite;
                    buttonActionTargets[i].buttonTarget.GetComponentInChildren<Light>().color = Color.green;
                    
                }
                    
            }
            yield return null;
        }
        if (timeClock > 0)
            StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(timeClock);
        float time = 0;
        while(time < transitionDuration)
        {
            time += Time.deltaTime;
            for(int i=0; i<buttonActionTargets.Length;i++)
            {
                buttonActionTargets[i].buttonTarget.localPosition = Vector3.Lerp(buttonActionTargets[i].target, buttonActionTargets[i].origin, transitionCurve.Evaluate(time / transitionDuration));
            }
            yield return null;
        }
        sprite.sprite = defaultSprite;
        for (int i = 0; i < buttonActionTargets.Length; i++)
            if (buttonActionTargets[i].useDifSprite)
            {
                buttonActionTargets[i].buttonTarget.GetComponent<SpriteRenderer>().sprite = defaultTargetSprite;
                buttonActionTargets[i].buttonTarget.GetComponentInChildren<Light>().color = Color.red;
            }
        foreach (Light light in l)
            light.color = Color.red;
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