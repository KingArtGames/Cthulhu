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
        private BaseCard.Factory _baseCardFactory;
        private DiContainer _container;
        private CoroutineService _coroutines;
        [Inject("CardVisualization")]
        public GameObject Visualization;

        public CardFactory(DiContainer container, CoroutineService coroutines, BaseCard.Factory baseCardFactory)
        {
            _container = container;
            _coroutines = coroutines;
            _baseCardFactory = baseCardFactory;
        }

        public BaseCard BuildCard(GameObject prefab)
        {
            BaseCard baseCard = _baseCardFactory.Create();
            InitializeCardPrefab(baseCard, prefab);

            return baseCard;
        }

        public void InitializeCardPrefab(BaseCard baseCard, GameObject prefab)
        {
            baseCard.Prefab = GameObject.Instantiate(prefab);
            GameObject vis = GameObject.Instantiate(Visualization);
            vis.transform.parent = baseCard.Prefab.transform;
            _container.InjectGameObject(baseCard.Prefab, true);
            foreach (AbstractCardBehaviour executor in baseCard.Prefab.GetComponentsInChildren<AbstractCardBehaviour>())
                executor.Initialize(baseCard);
            baseCard.Prefab.SetActive(false);
        }
    }
}
