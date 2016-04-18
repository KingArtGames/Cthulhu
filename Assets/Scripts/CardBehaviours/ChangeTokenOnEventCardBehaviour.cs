using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using Assets.Scripts.Services;

namespace Assets.Scripts.CardBehaviours
{
    [RequireComponent(typeof(AudioSource))]
    class ChangeTokenOnEventCardBehaviour : AbstractCardBehaviour
    {
        public TokenService.TokenType tokenType;
        public int numTokens = 0;
        public List<CardLifecycleStep> executeSteps = new List<CardLifecycleStep>();
        public List<Field.DeckLocation> executeLocations = new List<Field.DeckLocation>();
        public AudioClip AudioClip;

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

            //play animation / SFX
            if (AudioClip != null)
            {
                AudioSource source = GetComponent<AudioSource>();
                if (source != null)
                {
                    source.clip = AudioClip;
                    source.Play();
                    while (source.isPlaying)
                        yield return null;
                }
            }
            //
            tokenService.AddTokens(tokenType, numTokens);

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            string descriptionString = "[";
            for (int i = 0; i < executeSteps.Count; i++)
            {
                if(i != 0)
                    descriptionString += ",";
                descriptionString += executeSteps[i].ToString();
            }
            descriptionString += "] in [";
            for (int i = 0; i < executeLocations.Count; i++)
            {
                if (i != 0)
                    descriptionString += ",";
                descriptionString += executeLocations[i].ToString();
            }
            descriptionString += "]: " + tokenType.ToString() + numTokens.ToString("+#;-#;0");

            return descriptionString;
        }
    }
}
