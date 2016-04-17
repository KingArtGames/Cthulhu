using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Decks
{
    public class HandVisualizer : AbstractDeckVisualizer
    {
        

        public float radius = 0.1f;
        public float offset = -0.34f;
        public Vector2 center = new Vector2(0, 0);
 
        public float cardSpace = 10;

        public override void RefreshVisualization()
        {
            foreach(Transform trans in transform)
            {
                //Destroy(trans.gameObject);
            }

            _deck = field.GetDeck(DeckLocation);

            offset = -0.055f * _deck.CurrentSize;

            for (int i = 0; i < _deck.CurrentSize; i++)
            {
                GameObject cardGO = _deck.GetCardAtIndex(i).Prefab;
                cardGO.SetActive(true);
                cardGO.transform.parent = transform;
                float posX = center.x + radius * Mathf.Sin(offset + (i * cardSpace) * Mathf.Deg2Rad);
                float posY = center.y + radius * Mathf.Cos(offset + (i * cardSpace) * Mathf.Deg2Rad);

                cardGO.transform.localPosition = new Vector3(posX, (i * 0.1f), posY);

                Vector2 direction = new Vector2(center.x - posX, center.y - posY);

                cardGO.transform.Rotate(Vector3.up, -direction.x, Space.Self);
            }
        }
    }
}
