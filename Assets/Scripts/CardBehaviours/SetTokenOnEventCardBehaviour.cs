using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Assets.Scripts.Services;

namespace Assets.Scripts.CardBehaviours
{
    class SetTokenOnEventCardBehaviour : AbstractCardBehaviour
    {
        public TokenService.TokenType tokenType;
        public int numTokens = 0;
        public List<CardLifecycleStep> executeSteps = new List<CardLifecycleStep>();
        public List<Field.DeckLocation> executeLocations = new List<Field.DeckLocation>();

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public TokenService tokenService;
        [Inject]
        public CoroutineService Async;

        public bool hideExecuteLocation = false;

        public override void Initialize(BaseCard owner)
        {
            foreach (CardLifecycleStep step in executeSteps)
            {
                owner.RegisterLivecycleStepExecutor(step, OnEvent);
            }
        }

        private CardOperation OnEvent(Field.DeckLocation loc)
        {
            if (executeLocations.Contains(loc))
            {
                CardOperation result = new CardOperation();
                Async.RunAsync(SetTokens(result, loc));
                return result;
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator SetTokens(CardOperation op, Field.DeckLocation loc)
        {
            
            //play animation / SFX

            //
            tokenService.SetTokens(tokenType, numTokens);

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string descriptionString = "";
            for (int i = 0; i < executeSteps.Count; i++)
            {
                for (int j = 0; j < executeLocations.Count; j++)
                {
                    if (i != 0 || j != 0)
                        descriptionString += ",";
                    descriptionString += GetEventDescription(executeSteps[i], executeLocations[j], false);
                }
            }
            descriptionString += ": Set " + GetTokenName(tokenType, true) + " to " + numTokens;

            return descriptionString;
        }
    }
}
