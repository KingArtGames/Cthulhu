using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TestRunner : MonoBehaviour
    {
        public DeckSettings PlayerDeck;

        [Inject]
        public Field field;

        [Inject]
        public DeckFactory factory;

        public void Start()
        {
            Debug.Log("Howdy");
        }

        [PostInject]
        public void Completed()
        {
            StartCoroutine(RunTestAsync());
        }

        private IEnumerator RunTestAsync()
        {
            yield return null;

            factory.FillDeck(field.GetDeck(Field.DeckLocation.DrawPlayer), 10, PlayerDeck);

            
            Debug.Log(field);
            
        }

    }
}
