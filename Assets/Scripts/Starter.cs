using UnityEngine;
using System.Collections;
using Zenject;
using Assets.Scripts;

public class Starter : MonoBehaviour
{
    public int PlayerCardsPerRound = 2;
    public int EnemyCardsPerRound = 2;
    public int PlayerDeckSize = 10;
    public int EnemyDeckSize = 10;
    public int StartHP = 30;
    public int StartSanity = 30;

    public DeckSettings PlayerDeck;
    public DeckSettings EnemyDeck;

    [Inject]
    public GameProcessor GameProcessor;

    [PostInject]
    public void Initialize()
    {
        GameProcessor.PlayerCardsPerRound = PlayerCardsPerRound;
        GameProcessor.EnemyCardsPerRound = EnemyCardsPerRound;
        GameProcessor.PlayerDeckSize = PlayerDeckSize;
        GameProcessor.EnemyDeckSize = EnemyDeckSize;
        GameProcessor.StartHP = StartHP;
        GameProcessor.StartSanity = StartSanity;
        GameProcessor.StartNewGame(PlayerDeck, EnemyDeck);
    }
}
