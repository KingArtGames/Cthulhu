using UnityEngine;
using System.Collections;
using Assets.Scripts.CardBehaviours;
using System.Collections.Generic;
using Zenject;
using Assets.Scripts.Services;
using Assets.Scripts;

class MoveCardsFromDeckToDeckCardBehaviour : AbstractCardBehaviour
{
    public CardLifecycleStep step;
    public Field.DeckLocation deck;
    public Field.DeckLocation fromDeck;
    public Field.DeckLocation toDeck;

    public int numCards;
    public bool takeFromZero = true;

    [Inject]
    public Field fieldOfPayne;
    [Inject]
    public CoroutineService Async;

    public override void Initialize(BaseCard owner)
    {
        owner.RegisterLivecycleStepExecutor(step, OnEvent);
    }

    private CardOperation OnEvent(Field.DeckLocation loc)
    {
        if (loc == deck)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(MoveCards(result));
            return result;
        }

        return CardOperation.DoneSuccess;
    }

    private IEnumerator MoveCards(CardOperation op)
    {

        //play animation / SFX
        Debug.Log("MoveCards");
        //
        for (int i = 0; i < numCards; i++)
        {
            if (fieldOfPayne.GetDeck(fromDeck).CurrentSize == 0)
            {
                if (fromDeck == Field.DeckLocation.DrawPlayer)
                {
                    CardOperation reshuffleDeckOp = new CardOperation();
                    Async.RunAsync(ReshufflePlayerDeck(reshuffleDeckOp));
                    yield return reshuffleDeckOp;
                }
                else
                {
                    break;
                }
            }
            if (fieldOfPayne.GetDeck(fromDeck).CurrentSize > 0)
            {
                Debug.Log("Move");
                BaseCard cardToMove = takeFromZero ? fieldOfPayne.GetDeck(fromDeck).GetFirstCard() : fieldOfPayne.GetDeck(fromDeck).GetLastCard();
                CardOperation moveOp = fieldOfPayne.MoveCard(cardToMove, fromDeck, toDeck);
                yield return moveOp;
            }
        }
        op.Complete(CardOperation.Result.Success);
        yield break;
    }

    private IEnumerator ReshufflePlayerDeck(CardOperation op)
    {
        BaseDeck playerDraw = fieldOfPayne.GetDeck(Field.DeckLocation.DrawPlayer);
        if (playerDraw.Cards.Count > 0)
        {
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        BaseDeck playerDiscard = fieldOfPayne.GetDeck(Field.DeckLocation.DiscardPlayer);
        while (playerDiscard.Cards.Count > 0)
            yield return fieldOfPayne.MoveCard(playerDiscard.GetCardAtIndex(0), Field.DeckLocation.DiscardPlayer, Field.DeckLocation.DrawPlayer);
        playerDraw.Shuffle();
        op.Complete(CardOperation.Result.Success);
    }

    public override string GetDescription()
    {
        return "[" + step.ToString() + "] in [" + deck + "]: Move " + numCards + " cards from " + fromDeck.ToString() + " to " + toDeck;
    }
}