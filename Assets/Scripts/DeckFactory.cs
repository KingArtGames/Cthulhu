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

        public void FillDeck(ref BaseDeck deck, int numCards, DeckSettings settings)
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
                        BaseCard baseCard = new BaseCard();
                        GameObject prefab = GameObject.Instantiate(card.Prefab);
                        _container.InjectGameObject(prefab);
                        
                        foreach (AbstractCardBehaviour executor in prefab.GetComponents<AbstractCardBehaviour>())
                            executor.Initialize(baseCard);
                    }
                }
            }
        }

    }

    public class DeckSettings : ScriptableObject
    {
        public List<CardSettings> CardsInDeck;
    }

    public class CardSettings :ScriptableObject
    {
        public GameObject Prefab;
        public float Chance;
    }
}
