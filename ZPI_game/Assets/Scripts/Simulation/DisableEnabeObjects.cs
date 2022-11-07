using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisableEnabeObjects : MonoBehaviour
{
    [SerializeField]
    public List<Selectable> objects;
    public bool areDisabled = false;

    public void DisableEnable()
    {
        foreach (var o in objects)
        {
            o.enabled = areDisabled;
        }
        areDisabled = !areDisabled;
    }
}
