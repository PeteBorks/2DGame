/**
 * InvisibleWall.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
	[SerializeField]
	AnimationCurve transitionCurve;
	[SerializeField]
	float duration;
	[SerializeField]
	float targetAlpha = 0.5f;
	Material materialInstance;

	[SerializeField]
	GameObject [] objectsToToggle;

	void Start()
	{
		materialInstance = GetComponent<TilemapRenderer>().material;
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<PlayerController>() || collision.gameObject.CompareTag("Stellar"))
		{
			StartCoroutine(TransitionIn());
			
		}
        	
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.GetComponent<PlayerController>() || collision.gameObject.CompareTag("Stellar"))
		{
			StartCoroutine(TransitionOut());
			
		}
	}

    IEnumerator TransitionIn()
	{
		Color color = materialInstance.color;
		float time = 0;
		float currentAlpha = color.a;
        if (objectsToToggle != null)
        {
            foreach (GameObject obj in objectsToToggle)
            {
                obj.SetActive(true);
            }
        }
        while (time < duration)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(currentAlpha, targetAlpha, transitionCurve.Evaluate(time / duration));
			materialInstance.color = color;
			yield return null;
		}
	}

	IEnumerator TransitionOut()
	{
		Color color = materialInstance.color;
		float time = 0;
		float currentAlpha = color.a;
        if (objectsToToggle != null)
        {
            foreach (GameObject obj in objectsToToggle)
            {
                obj.SetActive(false);
            }
        }
        while (time < duration)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(currentAlpha, 1, transitionCurve.Evaluate(time / duration));
			materialInstance.color = color;
			yield return null;
		}
	}

    public void tIn()
    {
        StartCoroutine(TransitionIn());
    }
    public void tOut()
    {
        StartCoroutine(TransitionOut());
    }
}