using System;

public class PlayerInputHandler
{
    public bool HasControl
    {
        get
        {
            return _currentWaitForPlayerDoneOP != null;
        }
    }

    private WaitForPlayerInputDone _currentWaitForPlayerDoneOP;

    public WaitForPlayerInputDone HandOverControl()
    {
        _currentWaitForPlayerDoneOP = new WaitForPlayerInputDone();
        return _currentWaitForPlayerDoneOP;
    }

    public void PlayerInputDone()
    {
        if (_currentWaitForPlayerDoneOP != null)
            _currentWaitForPlayerDoneOP.SetTurnEnded();
        _currentWaitForPlayerDoneOP = null;

    }
}