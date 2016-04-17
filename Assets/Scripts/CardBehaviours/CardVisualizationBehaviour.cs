using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class CardVisualizationBehaviour : AbstractCardBehaviour
    {
        [Inject]
        public VisualizationService visualization;

        [Inject]
        public Field field;

        public Animator animator;

        protected BaseCard _owner;

        public override void Initialize(BaseCard owner)
        {
            _owner = owner;

            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Create, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Added, OnCardAdded);
            owner.RegisterLivecycleStepExecutor(CardLifecycleStep.Removed, OnCardRemoved);
        }

        private CardOperation OnCardAdded(Field.DeckLocation loc)
        {
            visualization.HandleCardMovement(loc);
            return CardOperation.DoneSuccess;
        }

        private CardOperation OnCardRemoved(Field.DeckLocation loc)
        {
            visualization.HandleCardMovement(loc);
            return CardOperation.DoneSuccess;
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
        }

        public void OnClick()
        {

            if (!Selected)
            {
                animator.SetTrigger("Select");
                _selected = true;
                CardOperation op = field.MoveCard(_owner, Field.DeckLocation.HandPlayer, Field.DeckLocation.FieldPlayer);
            }
            else
            {
                animator.SetTrigger("Deselect");
                _selected = false;
            }
        }

    }
}
