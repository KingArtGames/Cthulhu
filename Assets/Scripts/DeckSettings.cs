using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
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
