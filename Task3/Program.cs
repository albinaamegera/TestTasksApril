using System;
using System.Threading;

namespace Task3;
public class Program
{
    // helpers to demonstrate Server class performance

    static int _numThreads = 20;
    static bool _running = true;
    public static void Main()
    {
        // start a series of threads to randomly read from and
        // write to the shared resource.
        Thread[] t = new Thread[_numThreads];
        for (int i = 0; i < _numThreads; i++)
        {
            t[i] = new Thread(new ThreadStart(ThreadProc));
            t[i].Name = new String((char)(i + 65), 1);
            t[i].Start();
            if (i > 10)
                Thread.Sleep(300);
        }

        // tell the threads to shut down and wait until they all finish.
        _running = false;
        for (int i = 0; i < _numThreads; i++)
            t[i].Join();

        // Display statistics.
        Console.WriteLine(Server.Statistics());
        Console.WriteLine($"count value: {Server.GetCount()}");
        Console.Write("Press ENTER to exit... ");
        Console.ReadLine();
    }
    static void ThreadProc()
    {
        Random rnd = new Random();

        // randomly select a way for the thread to read and write from the shared
        // resource.
        while (_running)
        {
            double action = rnd.NextDouble();
            if (action < .8)
                Server.GetCount();
            else
                Server.AddToCount(rnd.Next(10));
        }
    }
}
// static solution class for task 3
public static class Server
{
    static ReaderWriterLock _rwl = new();
    static TimeSpan _timeout = TimeSpan.FromSeconds(10);
    static int _count = 0;

    // statistics
    static int _reads = 0;
    static int _writes = 0;
    static int _readsTimeouts = 0;
    static int _writesTimeouts = 0;

    public static int GetCount()
    {
        try
        {
            _rwl.AcquireReaderLock(_timeout);
            try
            {
                Interlocked.Increment(ref _reads);
                return _count;
            }
            finally
            {
                _rwl.ReleaseReaderLock();
            }
        }
        catch (ApplicationException)
        {
            Interlocked.Increment(ref _readsTimeouts);
            return 0;
        }
    }
    public static void AddToCount(int value)
    {
        try
        {
            _rwl.AcquireWriterLock(_timeout);
            try
            {
                _count += value;
                Interlocked.Increment(ref _writes);
            }
            finally
            {
                _rwl.ReleaseWriterLock();
            }
        }
        catch (ApplicationException)
        {
            Interlocked.Increment(ref _writesTimeouts);
        }
    }
    // helper method to show thread actions
    public static string Statistics()
    {
        return $"reads: {_reads};\nwrites: {_writes};\nread timeouts: {_readsTimeouts};\nwrite timeouts: {_writesTimeouts};";
    }
}
