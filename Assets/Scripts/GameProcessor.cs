using Assets.Scripts;
using Assets.Scripts.Services;
using System;
using System.Collections;
using Zenject;
using UnityEngine;

public class GameProcessor
{

    public int PlayerCardsPerRound = 2;
    public int EnemyCardsPerRound = 2;
    public int StartHP = 30;
    public int StartSanity = 30;

    private Field _field;
    private DeckFactory _factory;
    private CoroutineService _async;
    private PlayerInputHandler _playerInput;
    private Coroutine _gameCoroutine;
    private TokenService _tokens;

    public GameProcessor(Field field, DeckFactory deckFactory, CoroutineService coroutines, PlayerInputHandler playerInput, TokenService tokens)
    {
        _field = field;
        _factory = deckFactory;
        _async = coroutines;
        _playerInput = playerInput;
        _tokens = tokens;
    }

    public void StartNewGame(DeckSettings playerDeck, DeckSettings enemyDeck)
    {
        _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawPlayer), 10, playerDeck);
        _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawEnemy), 10, enemyDeck);

        _tokens.AddTokens(TokenService.TokenType.health, StartHP);
        _tokens.AddTokens(TokenService.TokenType.sanity, StartSanity);

        _gameCoroutine = _async.StartCoroutine(StartGame());
    }

    public void GameOver()
    {
        _async.StopCoroutine(_gameCoroutine);
        _gameCoroutine = null;
    }

    private IEnumerator StartGame()
    {
        while (true)
        {
            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in _field.GetDeck(location).Cards)
                { 
                    foreach (CardOperation op in card.ExecuteLifecycleStep(CardLifecycleStep.RoundBegin, location))
                    {
                        yield return op;
                        if (op.OperationResult != CardOperation.Result.Success)
                            break;
                    }
                }
            }
            for (int i = 0; i < EnemyCardsPerRound; i++)
            {
                CardOperation op = _field.MoveCard(_field.GetDeck(Field.DeckLocation.DrawEnemy).GetCardAtIndex(0), Field.DeckLocation.DrawEnemy, Field.DeckLocation.FieldEnemy);
                yield return op;
            }

            for (int i = 0; i < PlayerCardsPerRound; i++)
            {
                CardOperation op = _field.MoveCard(_field.GetDeck(Field.DeckLocation.DrawPlayer).GetCardAtIndex(0), Field.DeckLocation.DrawPlayer, Field.DeckLocation.HandPlayer);
                yield return op;
            }

            yield return _playerInput.HandOverControl();

            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in _field.GetDeck(location).Cards)
                {
                    foreach (CardOperation op in card.ExecuteLifecycleStep(CardLifecycleStep.RoundEnd, location))
                    {
                        yield return op;
                        if (op.OperationResult != CardOperation.Result.Success)
                            break;
                    }
                }
            }
        }
    }
}