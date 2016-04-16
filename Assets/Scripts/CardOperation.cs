using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardOperation : CustomYieldInstruction
{
    public static CardOperation DoneSuccess
    {
        get
        {
            return new CardOperation() { Done = true, OperationResult = Result.Success };
        }
    }

    public static CardOperation DoneFailure
    {
        get
        {
            return new CardOperation() { Done = true, OperationResult = Result.Failure };
        }
    }

    public enum Result
    {
        Success, Failure
    }

    public override bool keepWaiting
    {
        get
        {
            return !Done;
        }
    }

    public bool Done { get; private set; }
    public Result OperationResult { get; private set; }

    public CardOperation()
    {
        Done = false;
    }

    public void Complete(Result result)
    {
        OperationResult = result;
        Done = true;
    }
}
