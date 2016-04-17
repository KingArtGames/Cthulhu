using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;
using UniRx;

namespace Assets.Scripts.Decks
{
    public class HandVisualizer : AbstractDeckVisualizer
    {
        public float cardSpace = 10;
        
        public override void Initialize()
        {
            base.Initialize();
            
        }

        public override CardOperation RefreshVisualization()
        {
            return CardOperation.DoneSuccess;
        }

        protected override void ReArrangeCards()
        {
            float offset = -cardSpace / 2f;
            if (Deck.CurrentSize % 2 == 0)
                offset += (cardSpace / Deck.CurrentSize) / 2;
            else
                offset += (cardSpace / Deck.CurrentSize) / 2;

            for (int i = 0; i < Deck.CurrentSize; i++)
            {
                GameObject cardGO = Deck.GetCardAtIndex(i).Prefab;
                cardGO.SetActive(true);
                cardGO.transform.parent = transform;
                cardGO.transform.localPosition = Vector3.zero;
                cardGO.transform.localRotation = Quaternion.identity;

                cardGO.transform.localPosition = new Vector3(offset + (cardSpace / Deck.CurrentSize) * i, 0, 0);
            }
        }
    }
}
