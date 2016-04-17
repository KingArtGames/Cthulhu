﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts;
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

    public Texture2D Image;

    public Field.DeckLocation CurrentLocation { get; set; }

    public BaseCard(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;   
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
        _executors.Clear();
        GameObject.Destroy(Prefab);
        _cardFactory.InitializeCardPrefab(this, newPrefab);
    }
    public override string ToString()
    {
        return Prefab.name;
    }
    public string GetDescription()
    {
        string description = "";
        foreach (AbstractCardBehaviour executor in Prefab.GetComponentsInChildren<AbstractCardBehaviour>())
        {
            description += executor.GetDescription();
            description += Environment.NewLine;
        }
        description = description.TrimEnd(Environment.NewLine.ToCharArray());
        return description;
    }
}