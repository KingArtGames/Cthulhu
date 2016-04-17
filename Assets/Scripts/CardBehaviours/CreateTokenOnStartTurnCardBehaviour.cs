using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;

namespace Assets.Scripts.CardBehaviours
{
    class CreateTokenOnStartTurnCardBehaviour : AbstractCardBehaviour
    {
        public TokenService.TokenType tokenType;
        public int numTokens = 0;
        public List<Field.DeckLocation> executeLocations = new List<Field.DeckLocation>();

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public TokenService tokenService;

        public override void Initialize(BaseCard owner)
        {
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundBegin, OnStartTurn);
        }

        private CardOperation OnStartTurn(Field.DeckLocation loc)
        {
            if (executeLocations.Contains(loc))
            {
                CardOperation result = new CardOperation();
                StartCoroutine(AddTokens(result, loc));
                return result;
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator AddTokens(CardOperation op, Field.DeckLocation loc)
        {
            
            //play animation / SFX

            //
            tokenService.AddTokens(tokenType, numTokens);

            op.Complete(CardOperation.Result.Success);
            yield break;
        }
    }
}
