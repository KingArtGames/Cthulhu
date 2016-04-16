using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class ExplodesCardBehaviour : AbstractCardBehaviour
    {
        public int minRounds;
        public int maxRounds;

        private int rounds;

        [Inject]
        public Field fieldOfPayne;

        public override void Initialize(BaseCard owner)
        {
            rounds = UnityEngine.Random.Range(minRounds, maxRounds);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundBegin, OnRoundBegin);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnRemove);
        }

        private CardOperation OnRemove(Field.DeckLocation loc)
        {
            if (loc == Field.DeckLocation.HandPlayer)
                return CardOperation.DoneFailure;
            else
                return CardOperation.DoneSuccess;
        }

        private CardOperation OnRoundBegin(Field.DeckLocation loc)
        {
            if(loc == Field.DeckLocation.HandPlayer)
            {
                rounds--;

                if(rounds <= 0)
                {
                    CardOperation result = new CardOperation();
                    StartCoroutine(Explosion(result));
                    return result;
                }
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator Explosion(CardOperation op)
        {
            //play animation / SFX

            //Deal DMG

            op.Complete(CardOperation.Result.Failure);
            yield break;
        }
    }
}
