﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UniRx;
using Assets.Scripts.Services;
using Zenject;
using System;
using System.Text;

public class Field
{
    public enum DeckLocation
    {
        DrawPlayer,
        DrawEnemy, 
        HandPlayer,
        FieldPlayer,
        FieldEnemy,
        DiscardPlayer
    }

    private Dictionary<DeckLocation, BaseDeck> _decks = new Dictionary<DeckLocation, BaseDeck>();


    public CoroutineService Coroutines;

    public Field(CoroutineService coroutines)
    {
        Coroutines = coroutines;

        foreach (DeckLocation item in Enum.GetValues(typeof(DeckLocation)))
            CreateLocation(item);
        
    }
    
    public BaseDeck GetDeck(DeckLocation location)
    {
        return _decks[location];
    }

    private void CreateLocation(DeckLocation location)
    {
        _decks.Add(location, new BaseDeck(location, Coroutines));
    }
    

    internal CardOperation AddCard(BaseCard card, DeckLocation to)
    {
        return _decks[to].AddCard(card, 0);
    }

    public CardOperation MoveCard(BaseCard card, DeckLocation from, DeckLocation to)
    {
        CardOperation op = new CardOperation();
        Coroutines.RunAsync(MoveCardAsync(card, from, to, op));
        return op;
    }

    private IEnumerator MoveCardAsync(BaseCard card, DeckLocation from, DeckLocation to, CardOperation op)
    {
        yield return null;

        CardOperation removeOp = _decks[from].RemoveCard(card);
        yield return removeOp;
            
        if(removeOp.OperationResult == CardOperation.Result.Success)
        {
            CardOperation addOp = _decks[to].AddCard(card, 0);
            yield return addOp;

            op.Complete(addOp.OperationResult);
        }
        else
        {
            op.Complete(removeOp.OperationResult);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var field in _decks)
        {
            sb.Append(field.Key + ": ");
            foreach (var item in field.Value.Cards)
            {
                sb.Append(item.ToString() + " ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

}
