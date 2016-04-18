using Assets.Scripts.CardBehaviours;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Decks
{
    public abstract class AbstractDeckVisualizer : MonoBehaviour
    {
        [Inject]
        public Field field;

        [Inject]
        public VisualizationService Visualization;

        protected BaseDeck Deck;
        public Field.DeckLocation DeckLocation;

        private IDisposable _countChangedSubscription;
        private IDisposable _cardRemovedSubscription;

        [PostInject]
        virtual public void Initialize()
        {
            Deck = field.GetDeck(DeckLocation);
            Visualization.RegisterDeckVisualizer(this);
            _countChangedSubscription = Deck.Cards.ObserveCountChanged(true).Subscribe(_ => ReArrangeCards());
            _cardRemovedSubscription = Deck.Cards.ObserveRemove().Subscribe(OnCardRemoved);
        }

        public void OnDestroy()
        {
            if (_countChangedSubscription != null)
                _countChangedSubscription.Dispose();
        }

        public abstract CardOperation RefreshVisualization();

        protected virtual void OnCardRemoved(CollectionRemoveEvent<BaseCard> removeEvent)
        {
            GameObject cardGO = removeEvent.Value.Prefab;
            cardGO.SetActive(false);
        }

        protected abstract void ReArrangeCards();
    }
}
