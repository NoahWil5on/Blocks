using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame
{
    public sealed class StateManager
    {
        //States
        public enum GameState
        {
            Menu,
            Playing,
            Pause,
            EndScene
        }
        public enum WalkState
        {
            Forward,
            Left,
            Right
        }

        //Fields
        GameState currentGameState;
        WalkState currentWalkState;

        //Properties
        public GameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }
        public WalkState CurrentWalkState
        {
            get { return currentWalkState; }
            set { currentWalkState = value; }
        }

        //Singleton
        private static StateManager instance = null;

        private StateManager() { }

        public static StateManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new StateManager();
                return instance;
            }
        }
    }
}
