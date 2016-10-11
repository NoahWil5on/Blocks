using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileGame
{
    //Deals with what level you're on in storymode
    public sealed class LevelManager
    {
        //Fields
        private static StreamReader input;
        private static string currentLevel;
        private static string previousLevel;
        private static Stack<string> futureLevels;
        private static Stack<string> pastLevels;

        //Properties
        public string CurrentLevel { get { return currentLevel; } }
        public string PreviousLevel { get { return previousLevel; } }

        //Initializer
        public void Initialize()
        {
            futureLevels = new Stack<string>();
            pastLevels = new Stack<string>();
            input = new StreamReader(MainMenu.Instance.TypedFile + "/currentLevel.txt");
            string line;
            while ((line = input.ReadLine()) != null)
            {
                if (line[0] != '?')
                    futureLevels.Push(line);
                else
                {
                    string[] level = line.Split(':');
                    currentLevel = level[1];
                }
            }
            input.Close();  
            while(currentLevel != futureLevels.Peek())
            {
                previousLevel = currentLevel;
                NextLevel(false);
            }
            NextLevel(false);
        }
        public string NextLevel(bool inGame)
        {
            if (futureLevels.Count > 0)
            {
                if (!inGame)
                    previousLevel = currentLevel;
                else
                    currentLevel = futureLevels.Peek();
                pastLevels.Push(currentLevel);
                futureLevels.Pop();
                return currentLevel;
            }
            else
            {
                return "GameOver";
            }
        }



        //Singleton
        private static LevelManager instance = null;
        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelManager();
                return instance;
            }
        }

        private LevelManager() { }

    }
}
