using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimalDestianation : MonoBehaviour
{
    [FormerlySerializedAs("animalNeedType")] [SerializeField] private DestinationType destinationType;

    public DestinationType DestinationType => destinationType;
}
public enum DestinationType
{
    Eet,
    Sleep,
    Wash,
    Exit,
}