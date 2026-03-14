using System;
using System.Threading;

class DiningPhilosophersOrdered
{
    private readonly int n;
    private readonly int thinkingTime;
    private readonly int eatingTime;

    private readonly object[] forks;
    private readonly Thread[] threads;

    private volatile bool running = true;

    public DiningPhilosophersOrdered(int n, int thinkingTime, int eatingTime)
    {
        this.n = n;
        this.thinkingTime = thinkingTime;
        this.eatingTime = eatingTime;

        forks = new object[n];
        threads = new Thread[n];

        for (int i = 0; i < n; i++)
        {
            forks[i] = new object();
        }
    }

    public void Start()
    {
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

        Console.WriteLine("All philosophers stopped");
    }

    private void PhilosopherLoop(int index)
    {
        Random rand = new Random(index * 1000);

        int leftFork = index;
        int rightFork = (index + 1) % n;

        while (running)
        {
            int thinkTime = rand.Next(thinkingTime);
            Thread.Sleep(thinkTime);

            Console.WriteLine($"Philosopher {index} finished thinking");

            // ORDERING STRATEGY
            if (index % 2 == 0) // even philosophers
            {
                lock (forks[rightFork])
                {
                    Console.WriteLine($"Philosopher {index} took right fork {rightFork}");

                    lock (forks[leftFork])
                    {
                        Console.WriteLine($"Philosopher {index} took left fork {leftFork}");
                        Eat(rand, index);
                    }
                }
            }
            else // odd philosophers
            {
                lock (forks[leftFork])
                {
                    Console.WriteLine($"Philosopher {index} took left fork {leftFork}");

                    lock (forks[rightFork])
                    {
                        Console.WriteLine($"Philosopher {index} took right fork {rightFork}");
                        Eat(rand, index);
                    }
                }
            }
        }
    }

    private void Eat(Random rand, int index)
    {
        int eatTime = rand.Next(eatingTime);
        Thread.Sleep(eatTime);

        Console.WriteLine($"Philosopher {index} finished eating");
    }
}