using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.CardBehaviours
{
    class BaseDataCardBehaviour : AbstractCardBehaviour
    {
        public string Title;
        public Texture2D Image;

        private BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            foreach (CardLifecycleStep step in Enum.GetValues(typeof(CardLifecycleStep)))
            {
                owner.RegisterLivecycleStepExecutor(step, OnEvent);
            }
            _card = owner;
            _card.Title = Title;
            _card.Description = _card.GetDescription();
            _card.Image = Image;
        }

        private CardOperation OnEvent(Field.DeckLocation loc)
        {
            CardOperation result = new CardOperation();
            _card.Title = Title;
            _card.Description = _card.GetDescription();
            _card.Image = Image;
            result.Complete(CardOperation.Result.Success);
            return result;
        }
    }
}
