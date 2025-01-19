public sealed class MyThreadPool
{

    public Handle QueueUserWorkItem(Action action) => 
        Handle.StartWork(action);

    public sealed class Handle
    {
        public event EventHandler? Finished;

        public static Handle StartWork(Action action)
        {
            var handle = new Handle();          

            var thread = new Thread(_ =>
            {
                action.Invoke();
                handle.FinishedCallback();
            });

            thread.Start();

            return handle;
        }

        private void FinishedCallback()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}