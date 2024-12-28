using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOpener : MonoBehaviour
{
    public SelectedBuildingWindow SelectedBuildingWindow;
    public void OnEnable()
    {
        UIEvents.OnSelectedBuilding += building => SelectedBuildingWindow.Open(building);
    }

    private void OnDisable()
    {
        UIEvents.OnSelectedBuilding -= building => SelectedBuildingWindow.Open(building);
    }
}
