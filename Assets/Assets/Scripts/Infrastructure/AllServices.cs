using System;
using System.Collections;
using System.Collections.Generic;
using Script.Architecture;
using UnityEngine;

public class AllServices : Singleton<AllServices>
{
    public ISaveSystem SaveSystem;
    public PlayStateMachine PlayStateMachine;
    protected override void Awake()
    {
        base.Awake();
        SaveSystem = new PlayerPrefsSaveSystem();
        PlayStateMachine = new PlayStateMachine();
    }

    private void Update()
    {
        PlayStateMachine.Update();
    }
}
