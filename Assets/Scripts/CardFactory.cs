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
            GameObject instance = GameObject.Instantiate(prefab);
            container.InjectGameObject(instance);
            foreach (AbstractCardBehaviour executor in instance.GetComponents<AbstractCardBehaviour>())
                executor.Initialize(baseCard);
            instance.SetActive(false);

            return baseCard;
        }
    }
}
