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
    }
}