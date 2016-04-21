using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardBehaviours
{
    public enum CompareType
    {
        Equal,
        NotEqual,
        Greater,
        Smaller,
        GreaterEqual,
        SmallerEqual
    }

    abstract public class AbstractCardBehaviour : MonoBehaviour
    {
        abstract public void Initialize(BaseCard owner);

        public virtual string GetDescription()
        {
            return null;
        }

        public static bool Check(int value1, CompareType compareType, int value2)
        {
            switch (compareType)
            {
                case CompareType.Equal:
                    return value1 == value2;
                case CompareType.NotEqual:
                    return value1 != value2;
                case CompareType.Greater:
                    return value1 > value2;
                case CompareType.GreaterEqual:
                    return value1 >= value2;
                case CompareType.Smaller:
                    return value1 < value2;
                case CompareType.SmallerEqual:
                    return value1 <= value2;
            }
            return false;
        }

        public static string ToString(CompareType compareType)
        {
            switch(compareType)
            {
                case CompareType.Equal:
                    return "==";
                case CompareType.NotEqual:
                    return "!=";
                case CompareType.Greater:
                    return ">";
                case CompareType.GreaterEqual:
                    return ">=";
                case CompareType.Smaller:
                    return "<";
                case CompareType.SmallerEqual:
                    return "<=";
            }
            return null;
        }

        public static string GetDeckName(Field.DeckLocation deck, bool differPlayerAndEnemy = false)
        {
            switch (deck)
            {
                case Field.DeckLocation.HandPlayer:
                    return "Hand";
                case Field.DeckLocation.FieldPlayer:
                    if(differPlayerAndEnemy)
                        return "Player Field";
                    else
                        return "Field";
                case Field.DeckLocation.FieldEnemy:
                    if (differPlayerAndEnemy)
                        return "Enemy Field";
                    else
                        return "Field";
                case Field.DeckLocation.DiscardPlayer:
                    if (differPlayerAndEnemy)
                        return "Player Discard Pile";
                    else
                        return "Discard Pile";
                case Field.DeckLocation.DiscardEnemy:
                    if (differPlayerAndEnemy)
                        return "Enemy Discard Pile";
                    else
                        return "Discard Pile";
                default:
                    return deck.ToString();
            }
        }

        public static string GetTokenName(TokenService.TokenType tokenType, bool plural = false)
        {
            switch (tokenType)
            {
                case TokenService.TokenType.blue:
                case TokenService.TokenType.green:
                case TokenService.TokenType.purple:
                    if (plural)
                        return tokenType.ToString() + " tokens";
                    else
                        return tokenType.ToString() + " token";
                case TokenService.TokenType.health:
                case TokenService.TokenType.sanity:
                case TokenService.TokenType.doom:
                    return tokenType.ToString();
            }
            return "";
        }

        public static string GetEventDescription(CardLifecycleStep step, Field.DeckLocation location, bool onlyOneTime)
        {
            if (step == CardLifecycleStep.Add && location == Field.DeckLocation.FieldPlayer)
                return "Play card";
            else if ((step == CardLifecycleStep.Remove && location == Field.DeckLocation.FieldPlayer)
                || (step == CardLifecycleStep.Remove && location == Field.DeckLocation.FieldEnemy)
                || (step == CardLifecycleStep.Add && location == Field.DeckLocation.DiscardEnemy)
                || (step == CardLifecycleStep.Add && location == Field.DeckLocation.DiscardPlayer))
                return "Card destroyed";
            else if ((step == CardLifecycleStep.RoundBegin && location == Field.DeckLocation.FieldPlayer)
                || (step == CardLifecycleStep.RoundBegin && location == Field.DeckLocation.FieldEnemy))
                if(onlyOneTime)
                    return "Next turn";
                else
                    return "Every turn";
            else if (step == CardLifecycleStep.RoundEnd &&
                (location == Field.DeckLocation.FieldPlayer
                || location == Field.DeckLocation.FieldEnemy
                || location == Field.DeckLocation.CharacterPlayer
                || location == Field.DeckLocation.CharacterEnemy))
                if(onlyOneTime)
                    return "At end of turn";
                else
                    return "After every turn";
            else if ((step == CardLifecycleStep.RoundBegin && location == Field.DeckLocation.HandPlayer)
                || (step == CardLifecycleStep.RoundEnd && location == Field.DeckLocation.HandPlayer))
                if(onlyOneTime)
                    return "Next turn in hand";
                else
                    return "Every round in hand";
            else
                return "[" + step.ToString() + " in " + location.ToString() + "]";
        }
    }
}