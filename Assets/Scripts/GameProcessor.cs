using Assets.Scripts;
using Assets.Scripts.Services;
using System;
using System.Collections;
using Zenject;
using UnityEngine;

public class GameProcessor
{
    [Inject]
    public Field field;

    [Inject]
    public DeckFactory factory;

    [Inject]
    public CoroutineService async;

    [Inject]
    public PlayerInputHandler playerInput;

    public int playerCardsPerRound = 2;
    public int enemyCardsPerRound = 2;
    private Coroutine _gameCoroutine;

    public void StartNewGame(DeckSettings playerDeck, DeckSettings enemyDeck)
    {
        factory.FillDeck(field.GetDeck(Field.DeckLocation.DrawPlayer), 10, playerDeck);
        factory.FillDeck(field.GetDeck(Field.DeckLocation.DrawEnemy), 10, enemyDeck);

        _gameCoroutine = async.StartCoroutine(StartGame());
    }

    public void GameOver()
    {
        async.StopCoroutine(_gameCoroutine);
        _gameCoroutine = null;
    }

    private IEnumerator StartGame()
    {
        while (true)
        {
            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in field.GetDeck(location).Cards)
                { 
                    foreach (CardOperation op in card.ExecuteLifecycleStep(CardLifecycleStep.RoundBegin, location))
                    {
                        yield return op;
                        if (op.OperationResult != CardOperation.Result.Success)
                            break;
                    }
                }
            }
            for (int i = 0; i < enemyCardsPerRound; i++)
            {
                CardOperation op = field.MoveCard(field.GetDeck(Field.DeckLocation.DrawEnemy).GetCardAtIndex(0), Field.DeckLocation.DrawEnemy, Field.DeckLocation.FieldEnemy);
                yield return op;
            }

            for (int i = 0; i < playerCardsPerRound; i++)
            {
                CardOperation op = field.MoveCard(field.GetDeck(Field.DeckLocation.DrawPlayer).GetCardAtIndex(0), Field.DeckLocation.DrawPlayer, Field.DeckLocation.HandPlayer);
                yield return op;
            }

            yield return playerInput.HandOverControl();

            foreach (Field.DeckLocation location in Enum.GetValues(typeof(Field.DeckLocation)))
            {
                foreach (BaseCard card in field.GetDeck(location).Cards)
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