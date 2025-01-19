var threadPool = new MyThreadPool();

var handle1 = threadPool.QueueUserWorkItem(ExecuteMethod1);

handle1.Finished += (o, a) => { Console.WriteLine($"Done 1"); };

var handle2 = threadPool.QueueUserWorkItem(ExecuteMethod2);
handle2.Finished += (o, a) => { Console.WriteLine($"Done 2"); };

Thread.Sleep(1000);

void ExecuteMethod1()
{
    Thread.Sleep(400);
}

void ExecuteMethod2()
{
    Thread.Sleep(200);
}