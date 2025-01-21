var threadPool = new MyThreadPool();

var handle1 = threadPool.QueueUserWorkItem(ExecuteMethod1);

handle1.Finished += (o, a) => { Console.WriteLine($"Done 1"); };

Thread.Sleep(1000);

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



Console.WriteLine("===== checking in loop =====");

var random = new Random();

var done = 0;

var N = 100;

for (int i = 0; i < N; i++)
{
    var j = i;

    _= Task.Run(() =>
    {

        var h = threadPool.QueueUserWorkItem(() =>
        {
            Thread.Sleep(random.Next(50, 100));
            Console.WriteLine($"{j,4}");
        });

        h.Finished += (o, a) => Interlocked.Increment(ref done);
    });
}

while (done < N)
{
    Thread.Sleep(2000);
    Console.WriteLine($"=====  {done} done ====");
};