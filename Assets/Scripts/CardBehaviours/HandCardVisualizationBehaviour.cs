using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.CardBehaviours
{
    class HandCardVisualizationBehaviour : CardVisualizationBehaviour, IClickable
    {
        

        

        public override void Initialize(BaseCard owner)
        {
            base.Initialize(owner);
        }        

        
    }
}
