using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Decks;
using Assets.Scripts.UI;

namespace Assets.Scripts.CardBehaviours
{
    class CardVisualizationBehaviour : AbstractCardBehaviour, IClickable
    {
        [Inject]
        public VisualizationService Visualization;
        [Inject]
        public CoroutineService Async;

        [Inject]
        public Field field;

        [Inject]
        public GameProcessor processor;

        [Inject]
        public PlayerInputHandler PlayerInput;

        public Animation animation;

        protected BaseCard _owner;

        public Text TitleLabel;
        public Text DescriptionLabel;
        public CardTokenVisualgroup CardTokens;

        public TokenComponent HealthModifier;
        public TokenComponent SanityModiifier;

        public void Update()
        {
            UpdateTitle();
            UpdateDescription();
            UpdateImage();
            
        }

        public void UpdateTokens()
        {
            CardTokens.Card = _owner;
            HealthModifier.Card = _owner;
            SanityModiifier.Card = _owner;
        }

        public void UpdateTitle()
        {
            if(TitleLabel != null)
                TitleLabel.text = _owner.Title;
        }

        public void UpdateDescription()
        {
            if (DescriptionLabel != null)
                DescriptionLabel.text = _owner.Description;
        }

        public void UpdateImage()
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = _owner.Image;
        }

        public override void Initialize(BaseCard owner)
        {
            _owner = owner;
            UpdateTokens();
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Create, OnAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Add, OnAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnRemoved);
        }

        private CardOperation OnAdded(Field.DeckLocation loc)
        {
            CardOperation result = new CardOperation();
            Async.RunAsync(OnAddedAsync(loc, result));
            return result;
        }

        private IEnumerator OnAddedAsync(Field.DeckLocation loc, CardOperation op)
        {
            CardOperation result = Visualization.HandleCardAdded(loc, _owner);
            yield return result;
            if (result.OperationResult != CardOperation.Result.Success)
            {
                op.Complete(result.OperationResult);
                yield break;
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        private CardOperation OnRemoved(Field.DeckLocation loc)
        {   
            CardOperation result = new CardOperation();
            Async.RunAsync(OnRemovedAsync(loc, result));
            return result;
        }

        private IEnumerator OnRemovedAsync(Field.DeckLocation loc, CardOperation op)
        {
            CardOperation result = Visualization.HandleCardRemoved(loc, _owner);
            yield return result;
            if (result.OperationResult != CardOperation.Result.Success)
            {
                op.Complete(result.OperationResult);
                yield break;
            }
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        public void OnLeftClick()
        {
            if (!PlayerInput.HasControl || _owner.Executing) return;

            switch (_owner.CurrentLocation)
            {
                case Field.DeckLocation.HandPlayer:
                    field.MoveCard(_owner, Field.DeckLocation.HandPlayer, Field.DeckLocation.FieldPlayer);
                    break;
                case Field.DeckLocation.FieldPlayer:
                case Field.DeckLocation.FieldEnemy:
                case Field.DeckLocation.CharacterEnemy:
                case Field.DeckLocation.CharacterPlayer:
                    processor.UseCard(_owner);
                    break;
            }
        }

        public void OnRightClick()
        {
            CardPreview.ApplyCard(_owner);
        }
    }
}
