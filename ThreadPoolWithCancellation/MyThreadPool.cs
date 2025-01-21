using System.Collections.Concurrent;
using System.Runtime.InteropServices;

public sealed class MyThreadPool
{
    public MyThreadPool()
    {
        _thread = new Thread(Work);
        _thread.Start();
    }

    public Handle QueueUserWorkItem(Action action)
    {
        var handle = new Handle();

        _queue.Enqueue((action, handle));

        if (_status == 0)
        {
            var b = resetEvent.Set();

            if (!b) throw new InvalidOperationException("ResetEvent failed to set");

            Interlocked.Increment(ref _status);
        }

        return handle;
    }

    public sealed class Handle
    {
        public event EventHandler? Finished;

        public void FinishedCallback()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Work()
    {
        while (true)
        {
            resetEvent.WaitOne();

            var dequeued = _queue.TryDequeue(out var entry);

            if (!dequeued)
            {
                var oldStatus = Interlocked.Exchange(ref _status, 0);

                if (oldStatus <= 1)
                {
                    var b = resetEvent.Reset();

                    if (!b) throw new InvalidOperationException("ResetEvent failed to reset");
                }

                continue;
            };

            entry.Action();
            entry.Handle.FinishedCallback();
        }
    }

    private int _status = 0;
    private readonly Thread _thread;
    private readonly ManualResetEvent resetEvent = new(false);
    private readonly ConcurrentQueue<(Action Action, Handle Handle)> _queue = new();
}