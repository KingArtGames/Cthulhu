using Assets.Scripts.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Services
{
    public class VisualizationService
    {
        private List<AbstractDeckVisualizer> _visualizers = new List<AbstractDeckVisualizer>();
        private CoroutineService _coroutines;

        public VisualizationService(CoroutineService coroutines)
        {
            _coroutines = coroutines;
        }

        public void RegisterDeckVisualizer(AbstractDeckVisualizer visualizer)
        {
            _visualizers.Add(visualizer);
        }

        public CardOperation HandleCardMovement(Field.DeckLocation loc)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(HandleCardMovementAsync(loc, op));
            return op;
        }

        private IEnumerator HandleCardMovementAsync(Field.DeckLocation loc, CardOperation op)
        {
            foreach (AbstractDeckVisualizer visualizer in _visualizers)
            {
                if (visualizer.DeckLocation == loc)
                {
                    CardOperation  visOp = visualizer.RefreshVisualization();
                    yield return visOp;
                    if (visOp.OperationResult != CardOperation.Result.Success)
                    {
                        op.Complete(visOp.OperationResult);
                        yield break;
                    }
                }
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public AbstractDeckVisualizer GetDeckVisualizer(Field.DeckLocation loc)
        {
            return _visualizers.FirstOrDefault(vis => vis.DeckLocation == loc);
        }

        public CardOperation HandleCardAdded(Field.DeckLocation loc, BaseCard card)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(HandleCardAddedAsync(loc, card, op));
            return op;
        }

        private IEnumerator HandleCardAddedAsync(Field.DeckLocation loc, BaseCard card, CardOperation op)
        {
            foreach (AbstractDeckVisualizer visualizer in _visualizers)
            {
                if (visualizer.DeckLocation == loc)
                {
                    CardOperation visOp = visualizer.AddCard(card);
                    yield return visOp;
                    if (visOp.OperationResult != CardOperation.Result.Success)
                    {
                        op.Complete(visOp.OperationResult);
                        yield break;
                    }
                }
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public CardOperation HandleCardRemoved(Field.DeckLocation loc, BaseCard card)
        {
            CardOperation op = new CardOperation();
            _coroutines.RunAsync(HandleCardRemovedAsync(loc, card, op));
            return op;
        }

        private IEnumerator HandleCardRemovedAsync(Field.DeckLocation loc, BaseCard card, CardOperation op)
        {
            foreach (AbstractDeckVisualizer visualizer in _visualizers)
            {
                if (visualizer.DeckLocation == loc)
                {
                    CardOperation visOp = visualizer.RemoveCard(card);
                    yield return visOp;
                    if (visOp.OperationResult != CardOperation.Result.Success)
                    {
                        op.Complete(visOp.OperationResult);
                        yield break;
                    }
                }
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }
    }
}
