using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;
using UnityEngine;

namespace Assets.Scripts.CardBehaviours
{
    [RequireComponent(typeof(AudioSource))]
    class ExplodesCardBehaviour : AbstractCardBehaviour
    {
        public int minRounds;
        public int maxRounds;
        public int Damage = 5;
        public AudioClip ExplosionSound;

        private int rounds;

        private bool _initialized = false;

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public CoroutineService Async;
        [Inject]
        public TokenService Tokens;
        private BaseCard _owner;

        public override void Initialize(BaseCard owner)
        {
            rounds = UnityEngine.Random.Range(minRounds, maxRounds);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundBegin, OnRoundBegin);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnRemove);
            _initialized = true;
            _owner = owner;
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
            if(loc == Field.DeckLocation.HandPlayer)
            {
                rounds--;

                if(rounds <= 0)
                {
                    CardOperation result = new CardOperation();
                    Async.RunAsync(Explosion(result));
                    return result;
                }
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator Explosion(CardOperation op)
        {
            //play animation / SFX

            if (ExplosionSound != null)
                GetComponent<AudioSource>().PlayOneShot(ExplosionSound);
            Tokens.GetTokenStack(TokenService.TokenType.health).Remove(Damage);

            //Deal DMG

            CardOperation result = fieldOfPayne.MoveCard(_owner, Field.DeckLocation.HandPlayer, Field.DeckLocation.DiscardPlayer);
            yield return result;
            if (result.OperationResult != CardOperation.Result.Success)
            {
                op.Complete(result.OperationResult);
                yield break;
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string description = "After ";
            if (!_initialized)
                description += minRounds + "-" + maxRounds;
            else
                description += rounds;
            description += " turns in Hand: Player takes " + Damage + " damage";
            //description += Environment.NewLine;
            //description += "Cannot be played";

            return description;
        }
    }
}
