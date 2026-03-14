using System;
using System.Diagnostics;
using System.Threading;

class DiningPhilosophersOrdered
{
    private readonly int n;
    private readonly int thinkingTime;
    private readonly int eatingTime;

    private long totalEatingTime = 0;
    private object eatLock = new object();

    private readonly object[] forks;
    private readonly Thread[] threads;

    private volatile bool running = true;
    private Stopwatch dinnerTimer = new Stopwatch();

    public DiningPhilosophersOrdered(int n, int thinkingTime, int eatingTime)
    {
        this.n = n;
        this.thinkingTime = thinkingTime;
        this.eatingTime = eatingTime;

        forks = new object[n];
        threads = new Thread[n];
    }

    public void Start()
    {
        dinnerTimer.Start();

        for (int i = 0; i < n; i++)
        {
            forks[i] = new object();
        }

        for (int i = 0; i < n; i++)
        {
            int index = i;
            threads[i] = new Thread(() => PhilosopherLoop(index));
            threads[i].Start();
        }
    }

    public void Stop()
    {
        running = false;
        Console.WriteLine("STOPPING Philosophers");

        foreach (var t in threads)
        {
            t.Join();
        }

        dinnerTimer.Stop();

        Console.WriteLine("All philosophers stopped");
        Console.WriteLine($"Total time spent eating {totalEatingTime} ms");
        Console.WriteLine($"Total dinner time: {dinnerTimer.ElapsedMilliseconds} ms");
    }

    private void PhilosopherLoop(int index)
    {
        int localEatingTime = 0;
        Random rand = new Random(index * 1000);

        int leftFork = index;
        int rightFork = (index + 1) % n;

        while (running)
        {
            int thinkTime = rand.Next(thinkingTime);
            Thread.Sleep(thinkTime);

            Console.WriteLine($"Philospoher Nr. {index} finished thinking");

            if (index % 2 == 0) // even philosophers - take right first
            {
                lock (forks[rightFork])
                {
                    Console.WriteLine($"Philospoher Nr. {index} took right fork {rightFork}");

                    lock (forks[leftFork])
                    {
                        Console.WriteLine($"Philospoher Nr. {index} took left fork {leftFork}");
                        localEatingTime += Eat(rand, index);
                    }
                }
            }
            else // odd philosophers - take left first
            {
                lock (forks[leftFork])
                {
                    Console.WriteLine($"Philospoher Nr. {index} took left fork {leftFork}");

                    lock (forks[rightFork])
                    {
                        Console.WriteLine($"Philospoher Nr. {index} took right fork {rightFork}");
                        localEatingTime += Eat(rand, index);
                    }
                }
            }
        }
        // Safely add local eating time to total time
        lock (eatLock)
        {
            totalEatingTime += localEatingTime;
        }
    }

    private int Eat(Random rand, int index)
    {
        int eatTime = rand.Next(eatingTime);

        Thread.Sleep(eatTime);

        Console.WriteLine($"Philosopher Nr. {index} finished eating");

        return eatTime;
    }
}