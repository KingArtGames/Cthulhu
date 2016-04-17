using Assets.Scripts.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class VisualizationService
    {
        private List<AbstractDeckVisualizer> _visualizers = new List<AbstractDeckVisualizer>();

        public void RegisterDeckVisualizer(AbstractDeckVisualizer visualizer)
        {
            _visualizers.Add(visualizer);
        }

        public void HandleCardMovement(Field.DeckLocation loc)
        {
            foreach(AbstractDeckVisualizer visualizer in _visualizers)
            {
                if(visualizer.DeckLocation == loc)
                {
                    visualizer.RefreshVisualization();
                }
            }

        }
    }
}
