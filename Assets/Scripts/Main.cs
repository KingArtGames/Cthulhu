using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Assets.Scripts.Services;
using Assets.Scripts;

public class Main : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TurnManager>().ToSingleGameObject<TurnManager>();
        Container.Bind<CoroutineService>().ToSingleGameObject<CoroutineService>();
        Container.Bind<VisualizationService>().ToSingle<VisualizationService>();
        Container.Bind<Field>().ToSingle<Field>();
        Container.Bind<DeckFactory>().ToSingle();
        Container.Bind<GameProcessor>().ToSingle();
        Container.Bind<PlayerInputHandler>().ToSingle();
        Container.Bind<TokenService>().ToSingle();
        Container.Bind<CardFactory>().ToSingle();
    }
}
