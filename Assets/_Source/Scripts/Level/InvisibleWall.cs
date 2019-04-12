/**
 * InvisibleWall.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using UnityEngine.Tilemaps;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
	Material materialInstance;

	void Start()
	{
		materialInstance = GetComponent<TilemapRenderer>().material;
	}
	
	void OnTriggerEnter2D()
	{	
		Color color = materialInstance.color;
		while(color.a > 150f)
		{
			color.a -= 10f * Time.deltaTime;
		}
	}
}