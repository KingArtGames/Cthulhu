using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Services;
using Zenject;
using Assets.Scripts.CardBehaviours;

public enum CardLifecycleStep
{
    Create,
    Add,
    Remove,
    RoundBegin,
    RoundEnd,
    Use
}

public class BaseCard
{
    private Dictionary<CardLifecycleStep, List<Func<Field.DeckLocation, CardOperation>>> _executors = new Dictionary<CardLifecycleStep, List<Func<Field.DeckLocation, CardOperation>>>();

    public GameObject Prefab;
    private CardFactory _cardFactory;
    private CoroutineService _coroutines;
    private WaitForEndOfLifecycleStep.Factory _waitForEndOfLifecycleStepFactory;

    public string Title;
    public string Description;
    public Texture2D Image;

    public Field.DeckLocation CurrentLocation { get; set; }

    public BaseCard(CardFactory cardFactory, CoroutineService coroutines, WaitForEndOfLifecycleStep.Factory waitForEndOfLifecycleStepFactory)
    {
        _cardFactory = cardFactory;
        _coroutines = coroutines;
        _waitForEndOfLifecycleStepFactory = waitForEndOfLifecycleStepFactory;
    }

    public void RegisterLivecycleStepExecutor(CardLifecycleStep step, Func<Field.DeckLocation, CardOperation> func)
    {
        if (!_executors.ContainsKey(step))
        {
            _executors.Add(step, new List<Func<Field.DeckLocation, CardOperation>> ());
        }
        _executors[step].Add(func);
    }

    public IEnumerable<CardOperation> ExecuteLifecycleStep(CardLifecycleStep step, Field.DeckLocation deckLocation)
    {
        if (!_executors.ContainsKey(step))
            yield break;
        foreach (var executor in _executors[step])
            yield return executor.Invoke(deckLocation);
    }

    public void Replace(GameObject newPrefab)
    {
        _coroutines.RunAsync(ReplaceAsync(newPrefab));
    }

    private IEnumerator ReplaceAsync(GameObject newPrefab)
    {
        yield return _waitForEndOfLifecycleStepFactory.Create();
        _executors.Clear();
        GameObject.Destroy(Prefab);
        _cardFactory.InitializeCardPrefab(this, newPrefab);
    }

    public override string ToString()
    {
        return Title;
    }

    public class Factory : Factory<BaseCard> { }

    public string GetDescription()
    {
        string description = "";
        foreach (AbstractCardBehaviour executor in Prefab.GetComponentsInChildren<AbstractCardBehaviour>())
        {
            string executorDescription = executor.GetDescription();
            if (executorDescription != null)
            {
                description += executor.GetDescription();
                description += Environment.NewLine;
            }
        }
        description = description.TrimEnd(Environment.NewLine.ToCharArray());
        return description;
    }
}