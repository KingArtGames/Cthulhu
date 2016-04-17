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

    public ReactiveDictionary<TokenType, TokenStack> Tokens
    {
        get
        {
            return _tokens;
        }
    }

    private ReactiveDictionary<TokenType, TokenStack> _tokens = new ReactiveDictionary<TokenType, TokenStack>();
    
    public TokenStack GetTokenStack(TokenType type)
    {
        if (!_tokens.ContainsKey(type))
            _tokens.Add(type, new TokenStack());

        return _tokens[type];
    }

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