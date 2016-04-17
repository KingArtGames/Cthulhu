using Assets.Scripts.CardBehaviours;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public GameObject CardPrefab;

        [PostInject]
        virtual public void Initialize()
        {
            Deck = field.GetDeck(DeckLocation);
            Visualization.RegisterDeckVisualizer(this);
        }

        public abstract CardOperation RefreshVisualization();
    }
}
