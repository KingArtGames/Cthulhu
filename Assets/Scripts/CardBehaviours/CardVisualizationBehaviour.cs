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
        public PlayerInputHandler PlayerInput;

        public Animator animator;

        protected BaseCard _owner;

        public Text TitleLabel;
        public Text DescriptionLabel;

        public void UpdateTitle()
        {
            TitleLabel.text = _owner.ToString();
        }

        public void SetDescription(string description)
        {
            DescriptionLabel.text = _owner.GetDescription();
        }

        public void SetImage(Texture2D texture)
        {
            GetComponent<MeshRenderer>().material.mainTexture = _owner.Image;
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

        public void OnClick()
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

            }
            
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

        public void UpdateAll()
        {
            UpdateImage();
            UpdateTitle();
            UpdateDescription();
        }

        public void UpdateImage()
        {

        }

        public void UpdateTitle()
        {

        }

        public void UpdateDescription()
        {

        }
    }
}
