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

        public CoroutineService _coroutines;

        private ReactiveCollection<BaseCard> _cards;
        private GameProcessor _processor;

        public BaseDeck(Field.DeckLocation locations, CoroutineService coroutines, GameProcessor processor)
        {
            _location = locations;
            _coroutines = coroutines;
            _processor = processor;
            _cards = new ReactiveCollection<BaseCard>();
        }

        public int CurrentSize
        {
            get
            {
                return _cards.Count;
            }
        }

        public ReactiveCollection<BaseCard> Cards
        {
            get
            {
                return _cards;
            }
        }

        public CardOperation CreateCard(BaseCard card)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(CreateCardAsync(card, op));
            return op;
        }

        private IEnumerator CreateCardAsync(BaseCard card, CardOperation op)
        {
            foreach (var item in card.ExecuteLifecycleStep(CardLifecycleStep.Create, _location))
            {
                yield return item;
                if (item.OperationResult != CardOperation.Result.Success)
                {
                    op.Complete(item.OperationResult);
                    yield break;
                }
            }

            card.CurrentLocation = _location;
            _cards.Add(card);
            op.Complete(CardOperation.Result.Success);
            yield break;
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

            card.CurrentLocation = _location;
            _cards.Insert(index, card);
            op.Complete(CardOperation.Result.Success);
            _processor.TriggerLifecycleStepDone();
        }

        public BaseCard GetCardAtIndex(int index)
        {
            return _cards[index];
        }

        public BaseCard GetFirstCard()
        {
            return _cards[0];
        }

        public BaseCard GetLastCard()
        {
            return _cards[_cards.Count - 1];
        }

        public IEnumerable<BaseCard> GetCards()
        {
            return _cards;
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(item => UnityEngine.Random.value).ToReactiveCollection();
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

            _processor.TriggerLifecycleStepDone();
            yield break;
        }

        public class Factory : Factory<Field.DeckLocation, BaseDeck> { }
    }
}
