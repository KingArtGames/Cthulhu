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

        public float radius = 0.1f;
        public float offset = -0.34f;
        public Vector2 center = new Vector2(0, 0);

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
            offset = -cardSpace / 2f;// * (cardSpace / Deck.CurrentSize);
            if (Deck.CurrentSize % 2 == 0)
                offset += (cardSpace / Deck.CurrentSize) / 2;

            for (int i = 0; i < Deck.CurrentSize; i++)
            {
                GameObject cardGO = Deck.GetCardAtIndex(i).Prefab;
                cardGO.SetActive(true);
                cardGO.transform.parent = transform;
                cardGO.transform.localPosition = Vector3.zero;
                cardGO.transform.localRotation = Quaternion.identity;
                //float posX = center.x + radius * Mathf.Sin(offset + (i * cardSpace) * Mathf.Deg2Rad);
                //float posY = center.y + radius * Mathf.Cos(offset + (i * cardSpace) * Mathf.Deg2Rad);

                cardGO.transform.localPosition = new Vector3(offset + (cardSpace / Deck.CurrentSize) * i, 0, 0);

                //Vector2 direction = new Vector2(center.x - posX, center.y - posY);

                //cardGO.transform.Rotate(Vector3.up, direction.x, Space.Self);
            }
        }
    }
}
