using System;
using UnityEngine;

public class WaitForPlayerInputDone : CustomYieldInstruction
{
    private bool _turnEnded = false;

    public void SetTurnEnded()
    {
        _turnEnded = true;
    }

    public override bool keepWaiting
    {
        get
        {
            return !_turnEnded;
        }
    }
}