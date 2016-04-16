using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Assets.Scripts;

public interface IDeck
{
    int CurrentSize { get; }
    IEnumerable<ICard> Cards { get; }

    IEnumerable<ICard> GetCards();

    CardOperation RemoveCard(ICard card);
    CardOperation AddCard(ICard card, int index);
    ICard GetCardAtIndex(int index);

}
