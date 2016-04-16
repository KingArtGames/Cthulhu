using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public interface IDeck
{
    int GetMaxSize();
    int GetCurrentSize();
    IEnumerable<ICard> GetCards();

    void RemoveCard(ICard card);
    void AddCard(ICard card, int index);
    ICard GetCardAtIndex(int index);

    void CardAdded(ICard card);
    void CardRemoved(ICard card);

    IObservable<ICard> GetRemovedCardStream();
}