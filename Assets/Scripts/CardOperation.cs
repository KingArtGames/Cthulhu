using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CardOperation : CustomYieldInstruction
    {
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
}
