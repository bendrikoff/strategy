using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStateUI : MonoBehaviour
{
    public GameObject Panel;
    private void OnEnable()
    {
        UIEvents.OnStartBuilding += () => Panel.SetActive(true);
        UIEvents.OnEndBuilding += () => Panel.SetActive(false);
    }

    private void OnDisable()
    {
        UIEvents.OnStartBuilding -= () => Panel.SetActive(true);
        UIEvents.OnEndBuilding -= () => Panel.SetActive(false);
    }

    public void OnAgree()
    {
        UIEvents.OnEndBuilding?.Invoke();
    } 
}
