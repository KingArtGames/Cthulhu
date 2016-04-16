using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum CardLifecycleStep
{
    Add,
    Remove,
    RoundBegin,
    RoundEnd
}

public class BaseCard
{
    private Dictionary<CardLifecycleStep, List<Func<Field.DeckLocation, CardOperation>>> _executors = new Dictionary<CardLifecycleStep, List<Func<Field.DeckLocation, CardOperation>>>();

    public GameObject Prefab;

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
        if (_executors.ContainsKey(step))
            yield break;
        foreach (var executor in _executors[step])
            yield return executor.Invoke(deckLocation);
        
    }

    public BaseCard()
    {
    }
}