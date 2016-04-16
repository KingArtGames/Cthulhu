using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.CardBehaviours
{
    class ExplodesCardBehaviour : AbstractCardBehaviour
    {
        public int minRounds;
        public int maxRounds;

        private int rounds;

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
                    //EXPLODE!!!!!
                }
            }

            return CardOperation.DoneSuccess;
        }
    }
}
