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
        public Text BlueTokensText;
        public Text GreenTokensText;
        public Text PurpleTokensText;

        private IDisposable _healthChangedSubscription;
        private IDisposable _sanityChangedSubscription;
        private IDisposable _doomChangedSubscription;
        private IDisposable _blueTokensChangedSubscription;
        private IDisposable _greenTokensChangedSubscription;
        private IDisposable _purpleTokensChangedSubscription;

        [PostInject]
        public void Initialize()
        {
            _healthChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.health).Count.Subscribe(_ => RefreshHealth());
            _sanityChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.sanity).Count.Subscribe(_ => RefreshSanity());
            _doomChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.doom).Count.Subscribe(_ => RefreshDoom());
            _blueTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.blue).Count.Subscribe(_ => RefrehsBlueTokens());
            _greenTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.green).Count.Subscribe(_ => RefreshGreenTokens());
            _purpleTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.purple).Count.Subscribe(_ => RefreshPurpleTokens());
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
        private void RefrehsBlueTokens()
        {
            BlueTokensText.text = TokenService.Tokens[TokenService.TokenType.blue].Count.ToString();
        }
        private void RefreshGreenTokens()
        {
            GreenTokensText.text = TokenService.Tokens[TokenService.TokenType.green].Count.ToString();
        }
        private void RefreshPurpleTokens()
        {
            PurpleTokensText.text = TokenService.Tokens[TokenService.TokenType.purple].Count.ToString();
        }


        [Inject]
        PlayerInputHandler PlayerInput;

        public void OnFinishedClicked()
        {
            PlayerInput.PlayerInputDone();
        }


    }
}
