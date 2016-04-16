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
    private Dictionary<CardLifecycleStep, List<ILifecycleStepExecutor>> _executors = new Dictionary<CardLifecycleStep, List<ILifecycleStepExecutor>>();

    public string Name { get; set; }

    public void AddLifecycleStepExecutor(CardLifecycleStep step, ILifecycleStepExecutor executor)
    {
        if (!_executors.ContainsKey(step))
        {
            _executors.Add(step, new List<ILifecycleStepExecutor>());
        }
        _executors[step].Add(executor);
    }

    public IEnumerable<CardOperation> ExecuteLifecycleStep(CardLifecycleStep step, Field.DeckLocation deckLocation)
    {
        if (_executors.ContainsKey(step))
            yield break;
        foreach (var executor in _executors[step])
            yield return executor.Execute(deckLocation);
        
    }

    public BaseCard(string name)
    {
        Name = name;
    }
}