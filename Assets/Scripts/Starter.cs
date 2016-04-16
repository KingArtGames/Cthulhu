﻿using UnityEngine;
using System.Collections;
using Zenject;
using Assets.Scripts;

public class Starter : MonoBehaviour
{
    public DeckSettings PlayerDeck;
    public DeckSettings EnemyDeck;

    [Inject]
    public GameProcessor GameProcessor;

    [PostInject]
    public void Initialize()
    {
        GameProcessor.StartNewGame(PlayerDeck, EnemyDeck);
    }
}
