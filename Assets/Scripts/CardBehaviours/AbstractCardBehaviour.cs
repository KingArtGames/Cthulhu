using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardBehaviours
{
    abstract public class AbstractCardBehaviour : MonoBehaviour
    {
        abstract public void Initialize(BaseCard owner);

        public virtual string GetDescription()
        {
            return null;
        }
    }
}