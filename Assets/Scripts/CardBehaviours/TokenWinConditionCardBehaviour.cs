using UnityEngine;
using System.Collections;
using Zenject;
using Assets.Scripts.Services;
using UnityEngine.SceneManagement;
using System;

namespace Assets.Scripts.CardBehaviours
{
    class TokenWinConditionCardBehaviour : AbstractCardBehaviour
    {
        public TokenService.TokenType tokenType;
        public CompareType compareType;
        public int compareValue;

        [Inject]
        public TokenService tokenService;
        [Inject]
        public CoroutineService Async;

        public bool win = true;

        public Texture2D Image;
        public string Title;
        public string Description;

        public string nextScene;

        private BaseCard _card;

        public override void Initialize(BaseCard owner)
        {
            _card = owner;
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.RoundEnd, OnEndTurn);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Use, OnUse);
        }

        private CardOperation OnEndTurn(Field.DeckLocation loc)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(WinCheck(result));
            return result;
        }

        private CardOperation OnUse(Field.DeckLocation loc)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(NextSceneCheck(result));
            return result;
        }

        private bool ConditionFulfilled()
        {
            return Check(tokenService.GetTokenStack(tokenType).Count.Value, compareType, compareValue);
        }

        private IEnumerator WinCheck(CardOperation op)
        {
            if (ConditionFulfilled())
            {
                _card.Image = Image;
                _card.Title = Title;
                _card.Description = Description;
                foreach (AbstractCardBehaviour behaviour in _card.Prefab.GetComponents<AbstractCardBehaviour>())
                {
                    if (behaviour == this)
                        continue;
                    else if (behaviour is BaseDataCardBehaviour)
                    {
                        BaseDataCardBehaviour baseData = behaviour as BaseDataCardBehaviour;
                        baseData.Image = Image;
                        baseData.Title = Title;
                    }
                    else
                        Destroy(behaviour);
                }

                _card.Prefab.transform.parent.position = Camera.main.transform.position + 0.65f * Camera.main.transform.forward;
                _card.Prefab.transform.parent.rotation = Quaternion.LookRotation(Camera.main.transform.up);
            }
            else
            {
                op.Complete(CardOperation.Result.Failure);
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        private IEnumerator NextSceneCheck(CardOperation op)
        {
            if (ConditionFulfilled())
            {
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                op.Complete(CardOperation.Result.Failure);
            }

            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public override string GetDescription()
        {
            if (_card != null && ConditionFulfilled())
            {
                _card.Image = Image;
                _card.Title = Title;
                return Description;
            }
            else
            {
                string descriptionString = "";
                if (win)
                    descriptionString += "WinCondition: ";
                else
                    descriptionString += "LooseCondition: ";
                descriptionString += tokenType.ToString() + ToString(compareType) + compareValue;

                return descriptionString;
            }
        }
    }
}
