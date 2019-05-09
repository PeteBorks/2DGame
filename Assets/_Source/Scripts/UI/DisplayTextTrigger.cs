/**
 * DisplayTextTrigger.cs
 * Created by: Pedro Borges
 * Created on: 03/05/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;
using TMPro;

public class DisplayTextTrigger : MonoBehaviour
{ 
    TextMeshProUGUI textReference;
    public string text;
    bool once;

    private void Start()
    {
        foreach (TextMeshProUGUI t in FindObjectsOfType<TextMeshProUGUI>())
            if (t.transform.parent.gameObject.name == "MessageCanvas")
                textReference = t;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() && !once)
        {
            Animator animator = textReference.gameObject.GetComponent<Animator>();
            once = true;
            textReference.text = text;
            animator.enabled = true;
            animator.Play("TextWaypointAnimation", 0,0);

        }
    }



}