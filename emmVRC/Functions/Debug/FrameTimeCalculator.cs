#if (DEBUG == true)
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace emmVRC.Functions.Debug
{
    public class FrameTimeCalculator
    {
        public static int[] frameTimes = new int[30];
        public static int frameTimeAvg = 0;
        public static int iterator = 0;
        private static Stopwatch watch = new Stopwatch();
        public static void Update()
        {
            watch.Start();
            MelonLoader.MelonCoroutines.Start(EndOfFrame());
        }
        public static IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            frameTimes[iterator] = (int)watch.ElapsedMilliseconds;
            if (iterator < frameTimes.Length - 1)
                iterator++;
            else
                iterator = 0;
            watch.Reset();
            int sum = 0;
            foreach (int frm in frameTimes)
            {
                sum += frm;
            }
            frameTimeAvg = Mathf.CeilToInt(sum / frameTimes.Length);
        }
    }
}
#endif