using UnityEngine;
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

    private Dictionary<DeckLocation, IDeck> _decks = new Dictionary<DeckLocation, IDeck>();

    

    [Inject]
    public ICoroutineService coroutines;

    public Field(ICoroutineService coroutines)
    {
        _decks.Add(DeckLocation.DrawPlayer, new BaseDeck(coroutines));
        _decks.Add(DeckLocation.DrawEnemy, new BaseDeck(coroutines));
        _decks.Add(DeckLocation.HandPlayer, new BaseDeck(coroutines));
        _decks.Add(DeckLocation.FieldPlayer, new BaseDeck(coroutines));
        _decks.Add(DeckLocation.FieldEnemy, new BaseDeck(coroutines));
        _decks.Add(DeckLocation.DiscardPlayer, new BaseDeck(coroutines));
    }

    

    internal CardOperation AddCard(ICard card, DeckLocation to)
    {
        return _decks[to].AddCard(card, 0);
    }

    public CardOperation MoveCard(ICard card, DeckLocation from, DeckLocation to)
    {
        CardOperation op = new CardOperation();
        coroutines.RunAsync(MoveCardAsync(card, from, to, op));
        return op;
    }

    private IEnumerator MoveCardAsync(ICard card, DeckLocation from, DeckLocation to, CardOperation op)
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
