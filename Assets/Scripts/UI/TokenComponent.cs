using Assets.Scripts.CardBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TokenComponent : MonoBehaviour
    {
        public TokenService.TokenType TokenType;
        public Text TokenValueText;

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
            int tokenModifier = 0;
            foreach (AbstractCardBehaviour executor in _card.Prefab.GetComponentsInChildren<AbstractCardBehaviour>())
            {
                Dictionary<TokenService.TokenType, int> tokens = executor.GetCardTokens();
                foreach (KeyValuePair<TokenService.TokenType, int> keyval in tokens)
                {
                    if(keyval.Key == TokenType)
                    {
                        tokenModifier += keyval.Value;
                    }
                }
            }

            TokenValueText.text = tokenModifier.ToString();

            if(tokenModifier == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
