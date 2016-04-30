using Assets.Scripts.CardBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CardTokenVisualgroup : MonoBehaviour
    {
        public GameObject PurpleToken;
        public GameObject BlueToken;
        public GameObject GreenToken;

        private BaseCard _card;
        public BaseCard Card
        {
            get
            {
                return _card;
            }
            set
            {
                _card = value;
                Refresh();
            }
        }

        public void Refresh()
        {
            foreach(Transform trans in transform)
            {
                Destroy(trans.gameObject);
            }
            foreach (AbstractCardBehaviour executor in _card.Prefab.GetComponentsInChildren<AbstractCardBehaviour>())
            {
                Dictionary<TokenService.TokenType, int> tokens = executor.GetCardTokens();
                foreach (KeyValuePair<TokenService.TokenType, int> keyval in tokens)
                {
                    GameObject prefab = null;
                    switch (keyval.Key)
                    {
                        case TokenService.TokenType.blue:
                            prefab = BlueToken;
                            break;
                        case TokenService.TokenType.green:
                            prefab = GreenToken;
                            break;
                        case TokenService.TokenType.purple:
                            prefab = PurpleToken;
                            break;
                        default:
                            continue;
                    }

                    for (int i = 0; i < keyval.Value; i++) {
                        GameObject go = Instantiate(prefab);
                        go.transform.SetParent(transform);
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }
    }
}
