/**
 * ButtonEnable.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;

[SelectionBase]
public class ButtonEnable : MonoBehaviour
{
    [SerializeField]
    ButtonEnableTargetInfo[] buttonEnableTargets;
    [SerializeField]
    Light l;
    [SerializeField]
    bool useCollision = false;
    [SerializeField]
    DirectorTrigger camTrigger;
    [SerializeField]
    bool objectVisibilityValue;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<TilemapRenderer>())
            OnInteract();
    }

    public void OnInteract()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
        for (int i=0; i < buttonEnableTargets.Length; i++)
        {
            if(buttonEnableTargets[i].targetScript)
            {
                buttonEnableTargets[i].targetScript.Activate();
            }
            if (camTrigger)
                camTrigger.Execute();                
        }
        if(l)
        {
            l.enabled = true;
        }
            
    }

    void OnValidate()
    {
        for (int i = 0; i < buttonEnableTargets.Length; i++)
        {
            if (buttonEnableTargets[i].targetScript)
                buttonEnableTargets[i].name = buttonEnableTargets[i].targetScript.gameObject.name;
        }
    }
}