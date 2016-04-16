using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class Main : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().ToSingleGameObject<TurnManager>();
    }
}
