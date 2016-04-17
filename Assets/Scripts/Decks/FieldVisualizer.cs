using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public class FieldVisualizer : AbstractDeckVisualizer
    {

        public List<GameObject> FieldSlots;

        public override CardOperation RefreshVisualization()
        {
            return CardOperation.DoneSuccess;
        }

        protected override void ReArrangeCards()
        {
            Deck = field.GetDeck(DeckLocation);
            int cardIndex = 0;
            foreach(GameObject slot in FieldSlots)
            {
                if(cardIndex < Deck.CurrentSize)
                {
                    GameObject cardGO = Deck.GetCardAtIndex(cardIndex).Prefab;
                    cardGO.SetActive(true);
                    cardGO.transform.parent = slot.transform;
                    cardGO.transform.position = slot.transform.position;
                    cardIndex++;
                }
                
            }
        }
    }
}
