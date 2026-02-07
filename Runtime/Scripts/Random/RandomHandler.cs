namespace LizardKit.Random
{
    public static class RandomHandler
    {
        public static int RandomSeed;
        public static System.Random Rand;
        public static void RollNewSeed()
        {
            RandomSeed = UnityEngine.Random.Range(10000,90000);
            Rand = new System.Random(RandomSeed);
        }

        public static void Reload()
        {
            Rand = new System.Random(RandomSeed);
        }

        public static int Range(int i, int i1)
        {
            return Rand.Next(i, i1);
        }

        public static void CheckSeed()
        {
            if (RandomSeed == 0) RollNewSeed();
        }
    }
}