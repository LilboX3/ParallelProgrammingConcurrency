namespace PhilosophersConcurrency
{
    internal class Program
    {
        static void Main()
        {
            Console.Write("Number of philosophers: ");
            int n = int.Parse(Console.ReadLine());

            Console.Write("Max thinking time (ms): ");
            int thinkingTime = int.Parse(Console.ReadLine());

            Console.Write("Max eating time (ms): ");
            int eatingTime = int.Parse(Console.ReadLine());

            //var dinner = new DiningPhilosophersNaive(n, thinkingTime, eatingTime);
            var dinner = new DiningPhilosophersOrdered(n, thinkingTime, eatingTime);

            dinner.Start();

            Console.WriteLine("Press ENTER to stop...");
            Console.ReadLine();

            dinner.Stop();
        }
    }
}
