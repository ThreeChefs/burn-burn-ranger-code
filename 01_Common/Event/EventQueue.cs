using System;
using System.Collections.Generic;

public sealed class EventQueue
{
    private readonly Queue<Action<Action>> _queue = new();
    private bool _running;

    public void Enqueue(Action<Action> action)
    {
        _queue.Enqueue(action);
        TryRunNext();
    }

    private void TryRunNext()
    {
        if (_running) return;
        if (_queue.Count == 0) return;

        _running = true;
        var nextAction = _queue.Dequeue();

        nextAction.Invoke(() =>
        {
            _running = false;
            TryRunNext();
        });
    }

    public void ClearAll()
    {
        _queue.Clear();
        _running = false;
    }
}