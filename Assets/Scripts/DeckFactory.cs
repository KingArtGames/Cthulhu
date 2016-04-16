using Assets.Scripts.CardBehaviours;
using System;
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

        public DeckFactory(DiContainer container)
        {
            _container = container;
        }

        public void FillDeck(BaseDeck deck, int numCards, DeckSettings settings)
        {
            float chanceSum = settings.CardsInDeck.Sum(c => c.Chance);
            for (int i = 0; i < numCards; i++)
            {
                float roll = UnityEngine.Random.Range(0, chanceSum);
                foreach (var card in settings.CardsInDeck)
                {
                    roll -= card.Chance;
                    if(roll <= 0)
                    {
                        deck.CreateCard(CardFactory.BuildCard(card.Prefab, _container));
                    }
                }
            }
        }

    }

    public class DeckSettings : ScriptableObject
    {
        public List<CardSettings> CardsInDeck;

        [Serializable]
        public class CardSettings
        {
            public GameObject Prefab;
            public float Chance;
        }
    }

}
