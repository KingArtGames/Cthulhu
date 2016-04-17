using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class CardVisualizationBehaviour : AbstractCardBehaviour
    {
        [Inject]
        public VisualizationService visualization;

        public override void Initialize(BaseCard owner)
        {
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Create, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Add, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnCardRemoved);
        }

        private CardOperation OnCardAdded(Field.DeckLocation loc)
        {
            return visualization.HandleCardMovement(loc);
        }

        private CardOperation OnCardRemoved(Field.DeckLocation loc)
        {
            return visualization.HandleCardMovement(loc);
        }


    }
}
