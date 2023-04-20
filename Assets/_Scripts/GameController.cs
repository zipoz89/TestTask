using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/*
 *  DISCLAIMER
 *
 * Due to very short amount of time I got to the this project I had to use some bad practices to make it on time.
 * Also I did not had enough time to implement proper game structure i.e. State Machine, Object Pooling etc.
 * All of those concept might be found on my github page: https://github.com/zipoz89?tab=repositories
 * All shaders are also made in 100% by me
 */
public class GameController : MonoBehaviour
{
    [SerializeField] private AnimalNeed[] _animalNeeds;
    [FormerlySerializedAs("animalNeedDestinationReference")] [SerializeField] private AnimalDestinationReference animalDestinationReference;
    [SerializeField] private Animal animalPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private HudController hudController;
    
    LiftGammaGain liftGammaGain;
    private Animal _animal;

    private void Start()
    {
        if (!globalVolume.profile.TryGet(out liftGammaGain)) throw new System.NullReferenceException(nameof(liftGammaGain));
        SpawnNewAnimal();
    }

    private void Update()
    {
        _animal.CustomUpdate();
    }
    
    private void SpawnNewAnimal()
    {
        _animal = Instantiate(animalPrefab, spawnPoint.position, Quaternion.identity,this.transform);
        _animal.OnNeedFulFilled += DestroyAnimalAndSpawnNew;

        var randomNeed = _animalNeeds[Random.Range(0, _animalNeeds.Length)];
        hudController.DisplayNeedName(randomNeed.Name);
        _animal.StartNeed(randomNeed);
    }

    private void DestroyAnimalAndSpawnNew()
    {
        _animal.OnNeedFulFilled -= DestroyAnimalAndSpawnNew;
        Destroy(_animal.gameObject);
        _animal = null;
        SpawnNewAnimal();
    }

    private void AnimalReachedDestination()
    {
        _animal.OnDestinationReached -= AnimalReachedDestination;
        _animal.ProgressInCurrentNeed();
    }

    #region Actions

    public void DisplayTextThenProgress(string text)
    {
        _animal.DisplayText(text);
        _animal.ProgressInCurrentNeed();
    }

    public void PlaySleepSoundThenProgress()
    {
        _animal.PlaySleepSound();
        _animal.ProgressInCurrentNeed();
    }

    public void PlayParticlesThenProgress()
    {
        _animal.PlayParticles();
        _animal.ProgressInCurrentNeed();
    }

    public void ChangeToNightThenProgress(bool mode)
    {
        StartCoroutine(ChangeToNightCoroutine(mode));
        _animal.ProgressInCurrentNeed();
    }

    private IEnumerator ChangeToNightCoroutine(bool mode)
    {
        var elapsedTime = 0f;

        var start = mode ? 1 : 1.2f;
        var end = mode ? 1.2f : 1;
        
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            
            liftGammaGain.gamma.Override(new Vector4(1f, 1f, math.lerp(start,end,elapsedTime/1), 0));
            liftGammaGain.gain.Override(new Vector4(1f, 1f, math.lerp(start,end,elapsedTime/1), 0));
            
            yield return null;
        }
    }

    public void GetToDestinationThenProgress(int needType)
    {
        GetToDestinationThenProgress((DestinationType)needType);
    }

    public void GetToDestinationThenProgress(DestinationType needType)
    {
        Transform destination = animalDestinationReference.GetFirstOfType(needType).transform;
        _animal.GoToDestination(destination);
        _animal.OnDestinationReached += AnimalReachedDestination;
    }
    
    public void WaitSecondsThenProgress(float seconds)
    {
        StartCoroutine(WaitSecondsThenProgressCoroutine(seconds));
    }

    private IEnumerator WaitSecondsThenProgressCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _animal.ProgressInCurrentNeed();
    }
    #endregion
    

}
