using Assets.Scripts;
using Assets.Scripts.Services;
using System;
using System.Collections;
using Zenject;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    private List<Action> _lifecycleStepEndedListener = new List<Action>();

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
                yield break;
            }
        }
        op.Complete(CardOperation.Result.Success);
        yield break;
    }

    public void AddLifecycleStepEndedListener(Action listener)
    {
        _lifecycleStepEndedListener.Add(listener);
    }

    private IEnumerator StartGame(DeckSettings playerDeck, DeckSettings enemyDeck, DeckSettings characterDeck, DeckSettings bossDeck)
    {
        yield return null;
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawPlayer), 10, playerDeck);
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.DrawEnemy), 10, enemyDeck);
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.CharacterPlayer), 1, characterDeck);
        yield return _factory.FillDeck(_field.GetDeck(Field.DeckLocation.CharacterEnemy), 1, bossDeck);

        CardPreview.ApplyCard(_field.GetDeck(Field.DeckLocation.CharacterEnemy).GetCardAtIndex(0));

        _tokens.AddTokens(TokenService.TokenType.health, StartHP);
        _tokens.AddTokens(TokenService.TokenType.sanity, StartSanity);

        while (true)
        {
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

    public void TriggerLifecycleStepDone()
    {
        foreach (Action listener in _lifecycleStepEndedListener)
            listener.Invoke();
        _lifecycleStepEndedListener.Clear();
    }
}