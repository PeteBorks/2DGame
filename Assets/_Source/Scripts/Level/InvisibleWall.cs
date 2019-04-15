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
        color.a = .5f;
        materialInstance.color = color;
	}
}