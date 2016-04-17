using Assets.Scripts.CardBehaviours;
using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class DeckFactory
    {
        private readonly DiContainer _container;
        private CoroutineService _coroutines;

        public DeckFactory(DiContainer container, CoroutineService coroutines)
        {
            _container = container;
            _coroutines = coroutines;
        }

        public CardOperation FillDeck(BaseDeck deck, int numCards, DeckSettings settings)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(FillDeckAsync(op, deck, numCards, settings));
            return op;
        }

        private IEnumerator FillDeckAsync(CardOperation op, BaseDeck deck, int numCards, DeckSettings settings)
        {
            float chanceSum = settings.CardsInDeck.Sum(c => c.Chance);
            for (int i = 0; i < numCards; i++)
            {
                float roll = UnityEngine.Random.Range(0, chanceSum);
                foreach (var card in settings.CardsInDeck)
                {
                    roll -= card.Chance;
                    if (roll <= 0)
                    {
                        CardOperation createOp = deck.CreateCard(CardFactory.BuildCard(card.Prefab, _container));
                        yield return createOp;
                        if (createOp.OperationResult != CardOperation.Result.Success)
                        {
                            op.Complete(createOp.OperationResult);
                            yield break;
                        }
                    }
                }
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }
    }

    

}
