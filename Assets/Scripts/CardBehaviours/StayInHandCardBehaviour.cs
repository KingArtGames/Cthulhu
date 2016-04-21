using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class StayInHandCardBehaviour : AbstractCardBehaviour
    {
        public int minRounds;
        public int maxRounds;

        private int rounds;

        private bool _initialized = false;
        private BaseCard _card;

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public CoroutineService Async;

        public override void Initialize(BaseCard owner)
        {
            rounds = UnityEngine.Random.Range(minRounds, maxRounds);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundBegin, OnRoundBegin);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnRemove);
            _initialized = true;
            _card = owner;
        }

        private CardOperation OnRemove(Field.DeckLocation loc)
        {
            if (loc == Field.DeckLocation.HandPlayer && rounds > 0)
                return CardOperation.DoneFailure;
            else
                return CardOperation.DoneSuccess;
        }

        private CardOperation OnRoundBegin(Field.DeckLocation loc)
        {
            if (loc == Field.DeckLocation.HandPlayer)
            {
                rounds--;

                if (rounds <= 0)
                {
                    CardOperation result = new CardOperation();
                    Async.RunAsync(Destroy(result));
                    return result;
                }
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator Destroy(CardOperation op)
        {
            fieldOfPayne.MoveCard(_card, Field.DeckLocation.HandPlayer, Field.DeckLocation.DiscardPlayer);
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string description = "Stays in hand for ";
            if (!_initialized)
                description += minRounds + "-" + maxRounds;
            else
                description += rounds;
            description += " rounds";

            return description;
        }
    }
}
