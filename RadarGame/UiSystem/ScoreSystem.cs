using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarGame.UiSystem
{
    public static class ScoreSystem
    {
        private static int currentscore;
        private static int[] highscore = new int[5];
        private static int whichscore = 0;

        public static int getCurrentScore()
        {
            return currentscore;
        }

        public static void setCurrentScore(int newscore)
        {
            currentscore = newscore;
        }

        public static void addHighscore(int newscore)
        {
            for(int i = 0; i < highscore.Length; i++)
            {
                if(highscore[i] <= newscore)
                {
                    highscore[i] = newscore;
                }
            }
        }

        public static int[] getHighscore()
        {
            return highscore;
        }

    }
}
