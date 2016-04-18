using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Assets.Scripts.CardBehaviours
{
    class CardVisualizationBehaviour : AbstractCardBehaviour, IClickable
    {
        [Inject]
        public VisualizationService visualization;

        [Inject]
        public Field field;

        [Inject]
        public GameProcessor processor;

        [Inject]
        public PlayerInputHandler PlayerInput;

        public Animator animator;

        protected BaseCard _owner;

        public Text TitleLabel;
        public Text DescriptionLabel;

        public void Update()
        {
            UpdateTitle();
            UpdateDescription();
            UpdateImage();
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

            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Create, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Add, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Remove, OnCardRemoved);
        }

        private CardOperation OnCardAdded(Field.DeckLocation loc)
        {
            return visualization.HandleCardMovement(loc);
        }

        private CardOperation OnCardRemoved(Field.DeckLocation loc)
        {
            return visualization.HandleCardMovement(loc);
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
        }

        public void OnLeftClick()
        {
            if (!PlayerInput.HasControl) return;

            switch (_owner.CurrentLocation)
            {
                case Field.DeckLocation.HandPlayer:
                    if (!Selected)
                    {
                        animator.SetTrigger("Select");
                        _selected = true;
                        
                    }
                    else
                    {
                        animator.SetTrigger("Deselect");
                        _selected = false;
                    }
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

        public void OnAnimationFinished()
        {
            switch (_owner.CurrentLocation)
            {
                case Field.DeckLocation.HandPlayer:
                    CardOperation op = field.MoveCard(_owner, Field.DeckLocation.HandPlayer, Field.DeckLocation.FieldPlayer);
                    animator.SetTrigger("Deselect");
                    break;
            }
        }
    }
}
