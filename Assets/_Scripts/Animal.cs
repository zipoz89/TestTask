using System;
using System.Collections.Generic;
using _Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using DG.Tweening;

public class Animal : MonoBehaviour
{
    [FormerlySerializedAs("navMeshAgent")] [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float acceptableDistance;
    [SerializeField] private AudioSource sleepSoundSource;
    [SerializeField] private VisualEffect particles;
    [FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI tmp;

    public bool IsGoingToDestination =>  _destination != null;
    private Transform _destination;
    
    
    public Action OnDestinationReached;
    public Action OnNeedFulFilled;
    private AnimalNeed _currentNeed;

    public void StartNeed(AnimalNeed need)
    {
        _currentNeed = need;
        _currentNeed.OnNeedFulfiled += NeedFulfilledByAnimal;
        ProgressInCurrentNeed();
    }

    public void ProgressInCurrentNeed()
    {
        _currentNeed.ProgressActionChain();
    }

    public void CustomUpdate()
    {
        CheckIfDestinationReached();
        TeleportOnOffMeshLink();
    }

    private void TeleportOnOffMeshLink()
    {
        if (agent.isOnOffMeshLink)
        {
            agent.CompleteOffMeshLink();
        }
    }

    private void NeedFulfilledByAnimal()
    {
        _currentNeed.OnNeedFulfiled -= OnNeedFulFilled;
        OnNeedFulFilled?.Invoke();
    }

    private void CheckIfDestinationReached()
    {
        if (IsGoingToDestination && agent.pathStatus == NavMeshPathStatus.PathComplete &&
            agent.remainingDistance < acceptableDistance)
        {
            _destination = null;
            OnDestinationReached?.Invoke();
        }
    }

    public bool GoToDestination(Transform destination)
    {
        if (IsGoingToDestination)
        {
            return false;
        }
        
        _destination = destination;
        agent.destination = destination.position;
        return true;
    }

    public void PlaySleepSound()
    {
        sleepSoundSource.Play();
    }

    public void PlayParticles()
    {
        particles.Play();
    }

    public void DisplayText(string textToDisplay)
    {
        tmp.color = Color.white;
        tmp.text = textToDisplay;
        tmp.DOColor(new Color(1f, 1f, 1f, 0f), .5f).SetDelay(2f);
    }
}
