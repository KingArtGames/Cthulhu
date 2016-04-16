using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using Zenject;

namespace Assets.Scripts
{
    public class BaseDeck
    {
        private readonly Field.DeckLocation _location;
        private CoroutineService _coroutines;

        private List<BaseCard> _cards;

        public BaseDeck(Field.DeckLocation locations, CoroutineService coroutines)
        {
            _location = locations;
            _coroutines = coroutines;
            _cards = new List<BaseCard>();
        }

        public int CurrentSize
        {
            get
            {
                return _cards.Count;
            }
        }

        public IEnumerable<BaseCard> Cards
        {
            get
            {
                return _cards;
            }
        }

        public void CreateCard(BaseCard card)
        {
            _cards.Add(card);
        }

        public CardOperation AddCard(BaseCard card, int index)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(AddCardAsync(card, index, op));
            return op;
        }

        private IEnumerator AddCardAsync(BaseCard card, int index, CardOperation op)
        {
            foreach (var item in card.ExecuteLifecycleStep(CardLifecycleStep.Add, _location))
            {
                yield return item;
                if (item.OperationResult != CardOperation.Result.Success)
                {
                    op.Complete(item.OperationResult);
                    yield break;
                }
            }

            _cards.Insert(index, card);
            op.Complete(CardOperation.Result.Success);
        }

        public BaseCard GetCardAtIndex(int index)
        {
            return _cards[index];
        }

        public IEnumerable<BaseCard> GetCards()
        {
            return _cards;
        }

        public CardOperation RemoveCard(BaseCard card)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(RemoveCardAsync(card, op));
            return op;
        }

        private IEnumerator RemoveCardAsync(BaseCard card, CardOperation op)
        {
            foreach (var item in card.ExecuteLifecycleStep(CardLifecycleStep.Remove, _location))
            {
                yield return item;
                if (item.OperationResult != CardOperation.Result.Success)
                {
                    op.Complete(item.OperationResult);
                    yield break;
                }
            }

            _cards.Remove(card);
            op.Complete(CardOperation.Result.Success);
            yield break;
        }
    }
}
