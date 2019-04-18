/**
 * AmbientLightSwap.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;

public class AmbientLightSwap : MonoBehaviour
{
	Color newColor;
	Color defaultColor;
	Color dColor;
    public bool useCurrentColor = true;
    public Color startingColor;
	public Color targetColor;
	public float transitionDuration;
	public AnimationCurve transitionCurve;

	void Start()
	{
		defaultColor = RenderSettings.ambientLight;
		dColor = defaultColor;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
        if(useCurrentColor)
        {
            defaultColor = RenderSettings.ambientLight;
            newColor = targetColor;
        }
        else
        {
            defaultColor = startingColor;
            newColor = targetColor;
        }
        dColor = defaultColor;
		/*if(targetColor == RenderSettings.ambientLight)
		{
			newColor = defaultColor;
			defaultColor = targetColor;
		}
		else
		{
			defaultColor = dColor;
			newColor = targetColor;
		}*/

		if(collision.gameObject.GetComponent<PlayerController>())
			StartCoroutine(Transition());
	}

	IEnumerator Transition()
	{
		float time = 0;
		while(time < transitionDuration)
		{
			time += Time.deltaTime;
			RenderSettings.ambientLight = Color.Lerp(defaultColor, newColor, transitionCurve.Evaluate(time / transitionDuration));
			yield return null;
		}
        newColor = dColor;
        defaultColor = targetColor;
	}
}