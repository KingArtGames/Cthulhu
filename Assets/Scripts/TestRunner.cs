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
        [Inject]
        public Field field;

        [PostInject]
        public void Completed()
        {
            StartCoroutine(RunTestAsync());
        }

        private IEnumerator RunTestAsync()
        {
            yield return null;

            BaseCard card = new BaseCard();
            yield return field.AddCard(card, Field.DeckLocation.HandPlayer);

            /*Debug.Log(field);

            yield return field.MoveCard(card, Field.DeckLocation.HandPlayer, Field.DeckLocation.FieldPlayer);

            Debug.Log(field);*/
        }

    }
}
