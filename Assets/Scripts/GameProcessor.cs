﻿using Assets.Scripts;
using Assets.Scripts.Services;
using System;
using System.Collections;
using Zenject;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public enum GamePhase
{
    Draw,
    Player,
    Enemy
}

public class GameProcessor
{

    public int PlayerCardsPerRound = 2;
    public int EnemyCardsPerRound = 2;
    public int PlayerDeckSize = 20;
    public int EnemyDeckSize = 10;
    public int StartHP = 30;
    public int StartSanity = 30;

    private Field _field;
    private DeckFactory _factory;
    private CoroutineService _async;
    private PlayerInputHandler _playerInput;


    private Coroutine _gameCoroutine;
    private TokenService _tokens;
    private List<Action> _lifecycleStepEndedListener = new List<Action>();
    private List<Action> _gamePhaseChangedListener = new List<Action>();

    private GamePhase _currentPhase;
    public GamePhase Phase { get { return _currentPhase; } }
    

    public GameProcessor(Field field, DeckFactory deckFactory, CoroutineService coroutines, PlayerInputHandler playerInput, TokenService tokens)
    {
        _field = field;
        _factory = deckFactory;
        _async = coroutines;
        _playerInput = playerInput;
        _tokens = tokens;
    }

    public void StartNewGame(DeckSettings playerDeck, DeckSettings enemyDeck, DeckSettings characterDeck, DeckSettings bossDeck)
    {
        _gameCoroutine = _async.StartCoroutine(StartGame(playerDeck, enemyDeck, characterDeck, bossDeck));
    }

    public void GameOver()
    {
        _async.StopCoroutine(_gameCoroutine);
        _gameCoroutine = null;
    }

    public CardOperation UseCard(BaseCard card)
    {
        card.Executing = true;
        CardOperation op = new CardOperation();
        _async.RunAsync(UseCardAsync(card, op));
        return op;
    }

    private IEnumerator UseCardAsync(BaseCard card, CardOperation op)
    {
        foreach (var item in card.ExecuteLifecycleStep(CardLifecycleStep.Use, card.CurrentLocation))
        {
            yield return item;
            if (item.OperationResult != CardOperation.Result.Success)
            {
                op.Complete(item.OperationResult);
                card.Executing = false;
                yield break;
            }
        }
        op.Complete(CardOperation.Result.Success);
        card.Executing = false;
        yield break;
    }

    public void AddLifecycleStepEndedListener(Action listener)
    {
        _lifecycleStepEndedListener.Add(listener);
    }

    public void AddGamePhaseChangedListener(Action listener)
    {
        _gamePhaseChangedListener.Add(listener);
    }

    

    private IEnumerator StartGame(DeckSettings playerDeck, DeckSettings enemyDeck, DeckSettings characterDeck, DeckSettings bossDeck)
    {
        yield return null;
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawPlayer), PlayerDeckSize, playerDeck);
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawEnemy), EnemyDeckSize, enemyDeck);
        
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.CharacterEnemy), 1, bossDeck);

        //CardPreview.ApplyCard(_field.GetDeck(Field.DeckLocation.CharacterEnemy).GetCardAtIndex(0));

        _tokens.AddTokens(TokenService.TokenType.health, StartHP);
        _tokens.AddTokens(TokenService.TokenType.sanity, StartSanity);

        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.CharacterPlayer), 1, characterDeck);

        while (true)
        {
            _currentPhase = GamePhase.Draw;
            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in _field.GetDeck(location).Cards.ToArray())
                {
                    foreach (CardOperation op in card.ExecuteLifecycleStep(CardLifecycleStep.RoundBegin, location))
                    {
                        yield return op;
                        if (op.OperationResult != CardOperation.Result.Success)
                            break;
                    }
                }
                
                TriggerLifecycleStepDone();
            }
            GamePhaseStarted();

            for (int i = 0; i < EnemyCardsPerRound; i++)
            {
                if (_field.GetDeck(Field.DeckLocation.DrawEnemy).Cards.Count > 0)
                {
                    CardOperation op = _field.MoveCard(_field.GetDeck(Field.DeckLocation.DrawEnemy).GetCardAtIndex(0), Field.DeckLocation.DrawEnemy, Field.DeckLocation.FieldEnemy);
                    yield return op;
                }
            }

            for (int i = 0; i < PlayerCardsPerRound; i++)
            {
                if (_field.GetDeck(Field.DeckLocation.DrawPlayer).Cards.Count == 0)
                {
                    CardOperation reshuffleDeckOp = new CardOperation();
                    _async.RunAsync(ReshufflePlayerDeck(reshuffleDeckOp));
                    yield return reshuffleDeckOp;
                }
                if (_field.GetDeck(Field.DeckLocation.DrawPlayer).Cards.Count > 0)
                {
                    CardOperation op = _field.MoveCard(_field.GetDeck(Field.DeckLocation.DrawPlayer).GetCardAtIndex(0), Field.DeckLocation.DrawPlayer, Field.DeckLocation.HandPlayer);
                    yield return op;
                }
            }

            _currentPhase = GamePhase.Player;
            GamePhaseStarted();
            yield return _playerInput.HandOverControl();

            _currentPhase = GamePhase.Enemy;
            GamePhaseStarted();

            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in _field.GetDeck(location).Cards.ToArray())
                {
                    foreach (CardOperation op in card.ExecuteLifecycleStep(CardLifecycleStep.RoundEnd, location))
                    {
                        yield return op;
                        if (op.OperationResult != CardOperation.Result.Success)
                            break;
                    }
                }
                TriggerLifecycleStepDone();
            }
        }
    }

    private IEnumerator ReshufflePlayerDeck(CardOperation op)
    {
        BaseDeck playerDraw = _field.GetDeck(Field.DeckLocation.DrawPlayer);
        if (playerDraw.Cards.Count > 0)
        {
            op.Complete(CardOperation.Result.Success);
            yield break;
        }

        BaseDeck playerDiscard = _field.GetDeck(Field.DeckLocation.DiscardPlayer);
        while (playerDiscard.Cards.Count > 0)
            yield return _field.MoveCard(playerDiscard.GetCardAtIndex(0), Field.DeckLocation.DiscardPlayer, Field.DeckLocation.DrawPlayer);
        playerDraw.Shuffle();
        op.Complete(CardOperation.Result.Success);
    }

    public void TriggerLifecycleStepDone()
    {
        foreach (Action listener in _lifecycleStepEndedListener)
            listener.Invoke();
        _lifecycleStepEndedListener.Clear();
    }

    public void GamePhaseStarted()
    {
        foreach (Action listener in _gamePhaseChangedListener)
            listener.Invoke();
    }
}