using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public class DrawVisualizer : AbstractDeckVisualizer
    {
        public GameObject CardStack;

        public override CardOperation RefreshVisualization()
        {
            return CardOperation.DoneSuccess;
        }

        protected override void ReArrangeCards()
        {
            CardStack.transform.localScale = new Vector3(CardStack.transform.localScale.x,( Deck.CurrentSize / 10f) * 10f, CardStack.transform.localScale.z);
            Deck = field.GetDeck(DeckLocation);
            foreach (BaseCard card in Deck.GetCards())
            {
                GameObject cardGO = card.Prefab;
                cardGO.transform.rotation = this.transform.rotation;
                cardGO.transform.parent = this.transform;
                cardGO.transform.position = this.transform.position;
                cardGO.SetActive(false);
            }
        }
    }
}
