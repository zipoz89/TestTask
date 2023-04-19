using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionName;

    public void DisplayNeedName(string name)
    {
        actionName.text = "Current Need: " + name;
    }
}
