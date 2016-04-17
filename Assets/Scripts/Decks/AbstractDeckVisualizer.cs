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
        public VisualizationService visualization;

        protected BaseDeck _deck;
        public Field.DeckLocation DeckLocation;
        public GameObject CardPrefab;

        [PostInject]
        public void Initialize()
        {
            visualization.RegisterDeckVisualizer(this);

        }

        public abstract void RefreshVisualization();

    }
}
