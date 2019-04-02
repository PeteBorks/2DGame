/**
 * DeleteAfterCompletion.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;

public class DeleteAfterCompletion : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}