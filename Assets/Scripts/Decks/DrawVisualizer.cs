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
        public override void RefreshVisualization()
        {
            CardStack.transform.localScale = new Vector3(CardStack.transform.localScale.x, _deck.CurrentSize / 10, CardStack.transform.localScale.y);
        }
    }
}
