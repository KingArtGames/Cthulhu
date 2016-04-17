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


        public override void RefreshVisualization()
        {
            _deck = field.GetDeck(DeckLocation);
            int cardIndex = 0;
            foreach(GameObject slot in FieldSlots)
            {
                if(cardIndex < _deck.CurrentSize)
                {
                    GameObject cardGO = _deck.GetCardAtIndex(cardIndex).Prefab;
                    cardGO.SetActive(true);
                    cardGO.transform.parent = slot.transform;
                    cardGO.transform.position = slot.transform.position;
                    cardIndex++;
                }
                
            }
        }
    }
}
