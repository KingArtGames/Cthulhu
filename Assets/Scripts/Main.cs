using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Assets.Scripts.Services;

public class Main : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().ToSingleGameObject<TurnManager>();
        Container.Bind<ICoroutineService>().ToSingleGameObject<CoroutineService>();
        Container.Bind<Field>().ToSingle<Field>();
    }
}
