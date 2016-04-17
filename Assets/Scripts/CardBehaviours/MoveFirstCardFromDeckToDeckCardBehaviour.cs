using UnityEngine;
using System.Collections;
using Assets.Scripts.CardBehaviours;
using System.Collections.Generic;
using Zenject;

class MoveFirstCardFromDeckToDeckCardBehaviour : AbstractCardBehaviour
{
    public CardLifecycleStep step;
    public Field.DeckLocation deck;
    public Field.DeckLocation fromDeck;
    public Field.DeckLocation toDeck;

    [Inject]
    public Field fieldOfPayne;

    public override void Initialize(BaseCard owner)
    {
        owner.RegisterLivecycleStepExecutor(step, OnEvent);
    }

    private CardOperation OnEvent(Field.DeckLocation loc)
    {
        if (loc == deck)
        {
            CardOperation result = new CardOperation();
            StartCoroutine(MoveFirstCard(result));
            return result;
        }

        return CardOperation.DoneSuccess;
    }

    private IEnumerator MoveFirstCard(CardOperation op)
    {

        //play animation / SFX

        //
        if (fieldOfPayne.GetDeck(fromDeck).CurrentSize == 0)
        {
            op.Complete(CardOperation.Result.Failure);
        }
        else
        {
            CardOperation moveOperation = fieldOfPayne.MoveCard(fieldOfPayne.GetDeck(fromDeck).GetCardAtIndex(0), fromDeck, toDeck);
            op.Complete(moveOperation.OperationResult);
        }
        yield break;
    }
}