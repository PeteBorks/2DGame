/**
 * DirectorTrigger.cs
 * Created by: Pedro Borges
 * Created on: 17/04/19 (dd/mm/yy)
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider2D))]
public class DirectorTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Once, Everytime,
    }

    GameObject triggeringGameObject;
    Main main;
    public PlayableDirector director;
    public TriggerType triggerType;
    public UnityEvent OnDirectorPlay;
    public UnityEvent OnDirectorFinish;
    [HideInInspector]
    protected bool alreadyTriggered;

    private void Start()
    {
        main = FindObjectOfType<Main>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        triggeringGameObject = main.playerPawn.gameObject;
        if (other.gameObject != triggeringGameObject)
            return;

        Execute();
    }

    public void Execute()
    {
        if (triggerType == TriggerType.Once && alreadyTriggered)
            return;

        director.Play();
        alreadyTriggered = true;
        OnDirectorPlay.Invoke();
        Invoke("FinishInvoke", (float)director.duration);
    }

    void FinishInvoke()
    {
        OnDirectorFinish.Invoke();
    }

}