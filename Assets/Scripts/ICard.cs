using UnityEngine;
using System.Collections;
using System;

public interface ICard
{
    void OnDrawn();
    void OnPlay();
    void OnDestroy();
}

public class BaseCard : ICard
{
    public string Name { get; set; }

    public void OnDestroy()
    {
        throw new NotImplementedException();
    }

    public void OnDrawn()
    {
        throw new NotImplementedException();
    }

    public void OnPlay()
    {
        throw new NotImplementedException();
    }

    public BaseCard(string name)
    {
        Name = name;
    }
}