using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Assets.Scripts.Services;
using Assets.Scripts;

public class Main : MonoInstaller
{
    public GameObject CardVisualization;

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
        Container.Bind<WaitForEndOfLifecycleStep.Factory>().ToSingle();
        Container.Bind<BaseCard.Factory>().ToSingle();
        Container.Bind<BaseDeck.Factory>().ToSingle();
        Container.Bind<GameObject>("CardVisualization").ToInstance<GameObject>(CardVisualization);
    }
}
