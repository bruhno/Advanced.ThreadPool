using System.Threading;

var threadPool = new MyThreadPool();

using var cts = new CancellationTokenSource();

var handle1 = threadPool.QueueUserWorkItem(() => ExecuteMethod1(cts.Token));
handle1.Finished += (o, a) => { Console.WriteLine($"Done 1"); };

var handle2 = threadPool.QueueUserWorkItem(() => ExecuteMethod2(cts.Token));
handle2.Finished += (o, a) => { Console.WriteLine($"Done 2"); };

Thread.Sleep(600);

cts.Cancel();

Thread.Sleep(1000);

void ExecuteMethod1(CancellationToken cancellationToken)
{
    for (var i = 0; i < 5; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Thread.Sleep(100);        
    }
}

void ExecuteMethod2(CancellationToken cancellationToken)
{
    for (var i = 0; i < 10; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Thread.Sleep(100);
    }
}