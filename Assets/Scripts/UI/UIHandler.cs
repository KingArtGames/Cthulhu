using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace Assets.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {

        [Inject]
        public TokenService TokenService;

        public Text HealthText;
        public Text SanityText;
        public Text DoomText;

        private IDisposable _healthChangedSubscription;
        private IDisposable _sanityChangedSubscription;
        private IDisposable _doomChangedSubscription;

        [PostInject]
        public void Initialize()
        {
            _healthChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.health).Count.Subscribe(_ => RefreshHealth());
            _sanityChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.sanity).Count.Subscribe(_ => RefreshSanity());
            _doomChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.doom).Count.Subscribe(_ => RefreshDoom());

        }

        private void RefreshHealth()
        {
            HealthText.text = "Health: " + TokenService.Tokens[TokenService.TokenType.health].Count;
        }
        private void RefreshSanity()
        {
            SanityText.text = "Sanity: " + TokenService.Tokens[TokenService.TokenType.sanity].Count;
        }
        private void RefreshDoom()
        {
            DoomText.text = "Doom: " + TokenService.Tokens[TokenService.TokenType.doom].Count;
        }


        [Inject]
        PlayerInputHandler PlayerInput;

        public void OnFinishedClicked()
        {
            PlayerInput.PlayerInputDone();
        }


    }
}
