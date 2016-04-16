using Assets.Scripts.CardBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CardFactory
    {
        public static BaseCard BuildCard(GameObject prefab, DiContainer container)
        {
            BaseCard baseCard = new BaseCard();
            baseCard.Prefab = GameObject.Instantiate(prefab);
            container.InjectGameObject(baseCard.Prefab);
            foreach (AbstractCardBehaviour executor in baseCard.Prefab.GetComponents<AbstractCardBehaviour>())
                executor.Initialize(baseCard);
            baseCard.Prefab.SetActive(false);

            return baseCard;
        }
    }
}
