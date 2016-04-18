using Assets.Scripts.CardBehaviours;
using Assets.Scripts.Services;
using System;
using System.Collections;
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

        [Inject]
        public CoroutineService Async;

        public Field.DeckLocation DeckLocation;
        public AudioClip CardAddedSound;
        public AudioClip CardRemovedSound;

        protected BaseDeck Deck;

        private IDisposable _countChangedSubscription;
        private IDisposable _cardAddedSubscription;
        private IDisposable _cardRemovedSubscription;
        private AudioSource _audioSource;

        public AudioSource SoundSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = GetComponent<AudioSource>();
                return _audioSource;
            }
        }

        abstract protected bool ShowOnAdd { get; }

        [PostInject]
        virtual public void Initialize()
        {
            Deck = field.GetDeck(DeckLocation);
            Visualization.RegisterDeckVisualizer(this);
            _countChangedSubscription = Deck.Cards.ObserveCountChanged(true).Subscribe(_ => ReArrangeCards());
        }

        public void OnDestroy()
        {
            if (_countChangedSubscription != null)
                _countChangedSubscription.Dispose();
            if (_cardAddedSubscription != null)
                _cardAddedSubscription.Dispose();
            if (_cardRemovedSubscription != null)
                _cardRemovedSubscription.Dispose();
        }

        public IEnumerator AddCardAsync(CardOperation op, BaseCard card)
        {
            if (ShowOnAdd)
            {
                card.Prefab.SetActive(true);
                card.Prefab.transform.parent = transform;
            }
            else
                card.Prefab.transform.parent = null;
            yield return null;
            if (CardAddedSound == null || SoundSource == null)
            {
                op.Complete(CardOperation.Result.Success);
                yield break;
            }
            SoundSource.clip = CardAddedSound;
            SoundSource.Play();
            while (SoundSource.isPlaying)
                yield return null;

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public IEnumerator RemoveCardAsync(CardOperation op, BaseCard card)
        {
            if (CardRemovedSound == null ||SoundSource == null)
            {
                card.Prefab.SetActive(false);
                card.Prefab.transform.parent = null;
                op.Complete(CardOperation.Result.Success);
                yield break;
            }
            SoundSource.clip = CardRemovedSound;
            SoundSource.Play();
            while (SoundSource.isPlaying)
                yield return null;

            card.Prefab.SetActive(false);
            card.Prefab.transform.parent = null;
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public CardOperation AddCard(BaseCard card)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(AddCardAsync(result, card));
            return result;
        }

        public CardOperation RemoveCard(BaseCard card)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(RemoveCardAsync(result, card));
            return result;
        }

        public abstract CardOperation RefreshVisualization();

        protected abstract void ReArrangeCards();
    }
}
