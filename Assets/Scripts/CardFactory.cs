using Assets.Scripts.CardBehaviours;
using Assets.Scripts.Services;
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
        private DiContainer _container;
        private CoroutineService _coroutines;

        public CardFactory(DiContainer container, CoroutineService coroutines)
        {
            _container = container;
            _coroutines = coroutines;
        }

        public BaseCard BuildCard(GameObject prefab)
        {
            BaseCard baseCard = new BaseCard(this);
            InitializeCardPrefab(baseCard, prefab);

            return baseCard;
        }

        public void InitializeCardPrefab(BaseCard baseCard, GameObject prefab)
        {
            baseCard.Prefab = GameObject.Instantiate(prefab);
            _container.InjectGameObject(baseCard.Prefab);
            foreach (AbstractCardBehaviour executor in baseCard.Prefab.GetComponents<AbstractCardBehaviour>())
                executor.Initialize(baseCard);
            baseCard.Prefab.SetActive(false);
        }
    }
}
