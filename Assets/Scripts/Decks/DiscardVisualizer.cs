using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public class DiscardVisualizer : AbstractDeckVisualizer
    {
        public GameObject CardStack;

        public override CardOperation RefreshVisualization()
        {
            return CardOperation.DoneSuccess;
        }

        protected override void ReArrangeCards()
        {
            CardStack.transform.localScale = new Vector3(CardStack.transform.localScale.x,( Deck.CurrentSize / 10f) * 20f, CardStack.transform.localScale.z);
            CardStack.transform.localPosition = new Vector3(0f, 0.5f * CardStack.transform.localScale.y, 0f);

            for (int i = 0; i < Deck.CurrentSize; i++)
            {
                GameObject cardGO = Deck.GetCardAtIndex(i).Prefab;
                cardGO.SetActive(false);
            }
            if (Deck.CurrentSize > 0)
            {
                GameObject upperCardGO = Deck.GetFirstCard().Prefab;
                upperCardGO.SetActive(true);
                upperCardGO.transform.rotation = transform.rotation;
                upperCardGO.transform.parent = transform;
                upperCardGO.transform.position = transform.position + new Vector3(0, CardStack.transform.localScale.y*CardStack.transform.parent.localScale.y, 0);
            }
        }
    }
}
