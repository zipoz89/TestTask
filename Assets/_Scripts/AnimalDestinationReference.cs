using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimalDestinationReference : MonoBehaviour
{
    [FormerlySerializedAs("_animalNeedDestianations")] [SerializeField] private List<AnimalDestianation> animalDestianations = new ();

    public List<AnimalDestianation> AnimalDestianations => animalDestianations;

    public AnimalDestianation GetFirstOfType(DestinationType type)
    {
        foreach (var destination in AnimalDestianations)
        {
            if (destination.DestinationType == type)
            {
                return destination;
            }
        }

        return null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimalDestinationReference))]
public class AnimalNeedDestinationReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetScript = (AnimalDestinationReference)target;

        if (GUILayout.Button("Find Animal Destinations on Scene"))
        {
            var destinationsFound = FindObjectsOfType<AnimalDestianation>();
            foreach (var destination in destinationsFound)
            {
                if (!targetScript.AnimalDestianations.Contains(destination))
                {
                    targetScript.AnimalDestianations.Add(destination);
                }
            }
        }
    }
}
#endif