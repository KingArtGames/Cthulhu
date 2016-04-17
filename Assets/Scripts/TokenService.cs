using System;
using System.Collections.Generic;
using UniRx;

public class TokenService
{
    public enum TokenType
    {
        health,
        sanity,
        red,
        green,
        doom
    }

    private ReactiveDictionary<TokenType, TokenStack> _tokens = new ReactiveDictionary<TokenType, TokenStack>();
    
    public void AddTokens(TokenType type, int count)
    {
        if (!_tokens.ContainsKey(type))
            _tokens.Add(type, new TokenStack());
        _tokens[type].Add(count);
    }
}

public class TokenStack
{
    public ReactiveProperty<int> Count = new ReactiveProperty<int>(0);

    public void Add(int count)
    {
        Count.Value += count;
    }

    public void Remove(int count)
    {
        Count.Value -= count;
    }
}