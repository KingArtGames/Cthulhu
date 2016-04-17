﻿using UnityEngine;
using System.Collections;
using Zenject;
using Assets.Scripts.Services;

namespace Assets.Scripts.CardBehaviours
{
    class SanityCheckCardBehaviour : AbstractCardBehaviour
    {
        public CompareType compareType;
        public int compareValue;
        public GameObject changeToCard;

        [Inject]
        public TokenService tokenService;
        [Inject]
        public CoroutineService Async;

        private BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            _card = owner;
            //owner.RegisterLivecycleStepExecutor(executeStep, OnEvent);
           
        }

        private CardOperation OnSanityChanged(Field.DeckLocation loc)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(SanityCheck(result));
            return result;
        }

        private IEnumerator SanityCheck(CardOperation op)
        {

            //play animation / SFX

            //
            if(Check(tokenService.GetTokenStack(TokenService.TokenType.sanity).Count.Value, compareType, compareValue))
            {
                _card.Replace(changeToCard);
            }
            else
            {
                op.Complete(CardOperation.Result.Failure);
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string descriptionString = "[Sanity" + ToString(compareType) + compareValue + "]: SanityCheck";

            return descriptionString;
        }
    }
}
