/**
 * AmbientLightSwap.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;

public class AmbientLightSwap : MonoBehaviour
{
	public Color newColor;
	Color defaultColor;
	public float transitionDuration;
	public AnimationCurve transitionCurve;

	void Start()
	{
		defaultColor = RenderSettings.ambientLight;
	}

	void OnTriggerEnter2D()
	{
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
	}
}