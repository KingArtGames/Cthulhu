using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Assets.Scripts.Services;

namespace Assets.Scripts.CardBehaviours
{
    class ChangeCardOnEventCardBehaviour : AbstractCardBehaviour
    {
        public GameObject changeToCard;
        public CardLifecycleStep executeStep;
        public Field.DeckLocation executeLocation;
        public bool onlyOneTime = false;
        private bool _alreadyDone;

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public CoroutineService Async;

        private BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            owner.RegisterLivecycleStepExecutor(executeStep, OnEvent);
            _card = owner;
        }

        private CardOperation OnEvent(Field.DeckLocation loc)
        {
            if (executeLocation == loc)
            {
                CardOperation result = new CardOperation();
                Async.RunAsync(ChangeCard(result));
                return result;
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator ChangeCard(CardOperation op)
        {

            //play animation / SFX

            //
            if (!_alreadyDone || !onlyOneTime)
            {
                _card.Replace(changeToCard);
                fieldOfPayne.MoveCard(_card, _card.CurrentLocation, _card.CurrentLocation);
                _alreadyDone = true;
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string descriptionString = GetEventDescription(executeStep, executeLocation, onlyOneTime) + ": Change to " + changeToCard.name;

            return descriptionString;
        }
    }
}
