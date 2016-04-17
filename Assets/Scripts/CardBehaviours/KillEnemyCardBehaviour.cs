using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Assets.Scripts.Services;

namespace Assets.Scripts.CardBehaviours
{
    class KillEnemyCardBehaviour : AbstractCardBehaviour
    {
        public List<TokenService.TokenType> neededTokens;

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public TokenService tokenService;
        [Inject]
        public CoroutineService Async;

        BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Use, OnUse);
            _card = owner;
        }

        private CardOperation OnUse(Field.DeckLocation loc)
        {
            if (loc == Field.DeckLocation.FieldEnemy)
            {
                CardOperation result = new CardOperation();
                Async.RunAsync(TryKillEnemy(result));
                return result;
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator TryKillEnemy(CardOperation op)
        {

            //play animation / SFX

            //
            Dictionary<TokenService.TokenType, int> neededTokensCount = new Dictionary<TokenService.TokenType, int>();
            foreach (TokenService.TokenType token in neededTokens)
            {
                if (!neededTokensCount.ContainsKey(token))
                    neededTokensCount.Add(token, 0);
                neededTokensCount[token] = neededTokensCount[token] + 1;
            }
            foreach (KeyValuePair<TokenService.TokenType, int> keyval in neededTokensCount)
            {
                if (tokenService.GetTokenStack(keyval.Key).Count.Value < keyval.Value)
                {
                    op.Complete(CardOperation.Result.Failure);
                    yield break;
                }
            }
            CardOperation moveOp = fieldOfPayne.MoveCard(_card, Field.DeckLocation.FieldEnemy, Field.DeckLocation.DiscardEnemy);
            if(moveOp.OperationResult == CardOperation.Result.Failure)
            {
                op.Complete(CardOperation.Result.Failure);
                yield break;
            }
            foreach (KeyValuePair<TokenService.TokenType, int> keyval in neededTokensCount)
            {
                tokenService.AddTokens(keyval.Key, -keyval.Value);
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string descriptionString = "[Use:";
            for (int i = 0; i < neededTokens.Count; i++)
            {
                if (i != 0)
                    descriptionString += ",";
                descriptionString += neededTokens[i].ToString();
            }
            descriptionString += "]: Destroy";

            return descriptionString;
        }
    }
}
