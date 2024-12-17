using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject MainMenu;

    private void OnEnable()
    {
        UIEvents.OnStartBuilding += () => MainMenu.SetActive(false);
        UIEvents.OnEndBuilding += () => MainMenu.SetActive(true);
    }

    private void OnDisable()
    {
        UIEvents.OnStartBuilding -= () => MainMenu.SetActive(false);
        UIEvents.OnEndBuilding -= () => MainMenu.SetActive(true);
    }
}
