using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Assets.Scripts.Services;
using System;

namespace Assets.Scripts.CardBehaviours
{
    class ChangeTokenOnEventCardBehaviour : AbstractCardBehaviour
    {
        public TokenService.TokenType tokenType;
        public int numTokens = 0;
        public List<CardLifecycleStep> executeSteps = new List<CardLifecycleStep>();
        public List<Field.DeckLocation> executeLocations = new List<Field.DeckLocation>();
        public bool onlyOneTime;
        private bool _alreadyDone;
        public AudioClip AudioClip;
        public string Animation;

        [Inject]
        public Field fieldOfPayne;
        [Inject]
        public TokenService tokenService;
        [Inject]
        public CoroutineService Async;

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
                Async.RunAsync(AddTokens(result, loc));
                return result;
            }

            return CardOperation.DoneSuccess;
        }

        private IEnumerator AddTokens(CardOperation op, Field.DeckLocation loc)
        {
            AudioSource source = GetComponentInChildren<AudioSource>();
            Animator animator = GetComponentInChildren<Animator>();

            if (source != null && AudioClip != null)
            {
                source.clip = AudioClip;
                source.Play();
            }

            //if (animator != null && !string.IsNullOrEmpty(Animation))
            //    animator.Play(Animation);

            while ((source != null && source.isPlaying))// || (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
                yield return null;
            //
            if (!_alreadyDone || !onlyOneTime)
            {
                tokenService.AddTokens(tokenType, numTokens);
                _alreadyDone = true;
            }

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
                    if(i != 0 || j != 0)
                        descriptionString += ",";
                    descriptionString += GetEventDescription(executeSteps[i], executeLocations[j], onlyOneTime);
                }
            }
            if (tokenType == TokenService.TokenType.health && numTokens < 0)
            {
                descriptionString += ": Player gets " + Mathf.Abs(numTokens) + " damage";
            }
            else
            {
                if (numTokens > 0)
                {
                    descriptionString += ": Get " + numTokens;
                }
                else if (numTokens < 0)
                {
                    descriptionString += ": Lose " + Mathf.Abs(numTokens);
                }
                descriptionString += " " + GetTokenName(tokenType, Mathf.Abs(numTokens) != 1);
            }

            return descriptionString;
        }
    }
}
