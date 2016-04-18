using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace Assets.Scripts.Decks
{
    public class DrawVisualizer : AbstractDeckVisualizer
    {
        public GameObject CardStack;
        public Transform UpperCard;

        protected override bool ShowOnAdd
        {
            get
            {
                return false;
            }
        }

        public override CardOperation RefreshVisualization()
        {
            return CardOperation.DoneSuccess;
        }

        protected override void ReArrangeCards()
        {
            CardStack.transform.localScale = new Vector3(CardStack.transform.localScale.x,( Deck.CurrentSize / 10f) * 10f, CardStack.transform.localScale.z);
            CardStack.transform.localPosition = new Vector3(0f, 0.5f * CardStack.transform.localScale.y, 0f);
            if (UpperCard != null)
            {
                UpperCard.transform.localPosition = new Vector3(0f, CardStack.transform.localScale.y, 0f);
            }
        }
    }
}
