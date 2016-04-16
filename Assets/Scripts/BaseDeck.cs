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
    public class BaseDeck : IDeck
    {
        private ICoroutineService _coroutines;

        private List<ICard> _cards;

        public BaseDeck(ICoroutineService coroutines)
        {
            _coroutines = coroutines;
            _cards = new List<ICard>();
        }

        public int CurrentSize
        {
            get
            {
                return _cards.Count;
            }
        }

        public IEnumerable<ICard> Cards
        {
            get
            {
                return _cards;
            }
        }

        public CardOperation AddCard(ICard card, int index)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(AddCardAsync(card, index, op));
            return op;
        }

        private IEnumerator AddCardAsync(ICard card, int index, CardOperation op)
        {
            yield return null;

            _cards.Insert(index, card);
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public ICard GetCardAtIndex(int index)
        {
            return _cards[index];
        }

        public IEnumerable<ICard> GetCards()
        {
            return _cards;
        }

        public CardOperation RemoveCard(ICard card)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(RemoveCardAsync(card, op));
            return op;
        }

        private IEnumerator RemoveCardAsync(ICard card, CardOperation op)
        {
            yield return null;

            _cards.Remove(card);
            op.Complete(CardOperation.Result.Success);
            yield break;
        }
    }
}
