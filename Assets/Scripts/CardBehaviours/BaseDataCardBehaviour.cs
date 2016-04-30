using UnityEngine;
using System.Collections;
using System;
using Zenject;
using Assets.Scripts.Services;
using Assets.Scripts.Decks;
using Assets.Scripts.UI;

namespace Assets.Scripts.CardBehaviours
{
    class BaseDataCardBehaviour : AbstractCardBehaviour
    {
        public string Title;
        public string Description;
        
        public Texture2D Image;
        [Inject]
        public CoroutineService Async;
        [Inject]
        public VisualizationService Visualization;

        private BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            foreach (CardLifecycleStep step in Enum.GetValues(typeof(CardLifecycleStep)))
            {
                owner.RegisterLivecycleStepExecutor(step, OnEvent);
            }

            _card = owner;
            _card.Title = Title;
            _card.Description = String.IsNullOrEmpty(Description) ? _card.GetDescription() : Description;
            _card.Image = Image;
        }

        private CardOperation OnEvent(Field.DeckLocation loc)
        {
            _card.Title = Title;
            _card.Description = String.IsNullOrEmpty(Description) ? _card.GetDescription() : Description;
            _card.Image = Image;
            return CardOperation.DoneSuccess;
        }
    }
}
