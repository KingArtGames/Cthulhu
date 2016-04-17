using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class DestroyOnEndTurnCardBehaviour : AbstractCardBehaviour
    {
        public int afterXRounds = 0;

        private int rounds;

        private BaseCard card = null;

        [Inject]
        public Field fieldOfPayne;

        public override void Initialize(BaseCard owner)
        {
            rounds = afterXRounds;
            card = owner;
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundEnd, OnRoundEnd);
        }

        private CardOperation OnRoundEnd(Field.DeckLocation loc)
        {
            if(loc == Field.DeckLocation.FieldEnemy || loc == Field.DeckLocation.FieldPlayer)
            {
                rounds--;

                if(rounds < 0)
                {
                    CardOperation result = new CardOperation();
                    StartCoroutine(Destroy(result, loc));
                    return result;
                }
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator Destroy(CardOperation op, Field.DeckLocation loc)
        {
            //play animation / SFX

            //
            if (loc == Field.DeckLocation.FieldEnemy)
                fieldOfPayne.MoveCard(card, loc, Field.DeckLocation.DiscardEnemy);
            else if (loc == Field.DeckLocation.FieldPlayer)
                fieldOfPayne.MoveCard(card, loc, Field.DeckLocation.DiscardPlayer);
            else
                op.Complete(CardOperation.Result.Failure);

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string description = "[" + CardLifecycleStep.RoundEnd.ToString() + ":";
            if (card == null)
                description += afterXRounds;
            else
                description += rounds;
            description += "]: DestroyCard";

            return description;
        }
    }
}
