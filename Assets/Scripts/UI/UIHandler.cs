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

        [Inject]
        public GameProcessor Processor;

        public Text HealthText;
        public Text SanityText;
        public Text DoomText;
        public Text RedTokensText;
        public Text GreenTokensText;
        public Text PurpleTokensText;

        public Text PhaseText;

        private IDisposable _healthChangedSubscription;
        private IDisposable _sanityChangedSubscription;
        private IDisposable _doomChangedSubscription;
        private IDisposable _redTokensChangedSubscription;
        private IDisposable _greenTokensChangedSubscription;
        private IDisposable _purpleTokensChangedSubscription;

        [PostInject]
        public void Initialize()
        {
            _healthChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.health).Count.Subscribe(_ => RefreshHealth());
            _sanityChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.sanity).Count.Subscribe(_ => RefreshSanity());
            _doomChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.doom).Count.Subscribe(_ => RefreshDoom());
            _redTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.red).Count.Subscribe(_ => RefreshRedTokens());
            _greenTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.green).Count.Subscribe(_ => RefreshGreenTokens());
            _purpleTokensChangedSubscription = TokenService.GetTokenStack(TokenService.TokenType.purple).Count.Subscribe(_ => RefreshPurpleTokens());

            Processor.AddGamePhaseChangedListener(PhaseChanged);
        }

        private void PhaseChanged()
        {
            switch (Processor.Phase)
            {
                case GamePhase.Draw:
                    PhaseText.text = "Draw Phase";
                    break;
                case GamePhase.Player:
                    PhaseText.text = "Player Turn";
                    break;
                case GamePhase.Enemy:
                    PhaseText.text = "Enemy Phase";
                    break;
            }
            
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
        private void RefreshRedTokens()
        {
            RedTokensText.text = "Red: " + TokenService.Tokens[TokenService.TokenType.red].Count;
        }
        private void RefreshGreenTokens()
        {
            GreenTokensText.text = "Green: " + TokenService.Tokens[TokenService.TokenType.green].Count;
        }
        private void RefreshPurpleTokens()
        {
            PurpleTokensText.text = "Purple: " + TokenService.Tokens[TokenService.TokenType.purple].Count;
        }


        [Inject]
        PlayerInputHandler PlayerInput;

        public void OnFinishedClicked()
        {
            PlayerInput.PlayerInputDone();
        }


    }
}
