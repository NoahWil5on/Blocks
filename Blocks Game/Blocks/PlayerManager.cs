using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TileGame
{
    class PlayerManager
    {
        //Fields
        private static Rectangle rect;
        private static Rectangle source;
        private static GameObject obj;
        private static int size;
        private static int sizeX;

        private static int speed;

        private static bool underWater;

        //Properties
        public Rectangle Rectangle
        {
            get { return obj.Rectangle; }
            set { obj.Rectangle = value; }
        }
        public Rectangle SourceRect { get { return source; } }
        public double Gravity
        {
            get { return obj.Grav; }
            set { obj.Grav = value; }
        }
        public int Size { get { return size; } }
        public double Grav
        {
            get { return obj.G; }
            set { obj.G = value; }
        }
        public bool UnderWater
        {
            get { return underWater; }
            set { underWater = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //Initializer
        public void Initialize()
        {
            rect = new Rectangle(80, 80, sizeX, size);
            obj = new GameObject(rect);
            speed = 3;
            size = 28;
            sizeX = 22;
            underWater = false;            
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Source Rectangle
            Source(gameTime);
            //Falling
            obj.Update();
            //Moving
            Movement(); 
        }
        //Movement
        public void Movement()
        {
            KeyboardState keyboard = Keyboard.GetState();
            //left
            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                obj.Left(speed);
                StateManager.Instance.CurrentWalkState = StateManager.WalkState.Left;
            }
            //right
            else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                obj.Right(speed);
                StateManager.Instance.CurrentWalkState = StateManager.WalkState.Right;
            }
            else
            {
                StateManager.Instance.CurrentWalkState = StateManager.WalkState.Forward;
            }
            //jump
            if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)))
            {
                obj.Jump(-7);
            }
        
            foreach(Rectangle r in MapManager.Instance.Flag)
            {
                if(r.Intersects(obj.Rectangle))
                {
                    string gameOver = LevelManager.Instance.NextLevel(true);
                    if(gameOver != "GameOver")
                    {
                        MainMenu.Instance.Load(
                            MainMenu.Instance.TypedFile + gameOver,
                            false, false);
                    }
                    MainMenu.Instance.ActiveFile = MainMenu.Instance.TypedFile + gameOver;
                    break;
                }
            }
        }

        //Source Rectangle
        public void Source(GameTime gameTime)
        {
            int tick = 32 * ((int)(gameTime.TotalGameTime.Milliseconds / 100)%4);
            if (StateManager.Instance.CurrentWalkState == StateManager.WalkState.Forward)
            { source = new Rectangle(0,0,32,48); }
            else if (StateManager.Instance.CurrentWalkState == StateManager.WalkState.Right)
            { source = new Rectangle(tick, 96, 32, 48); }
            else if (StateManager.Instance.CurrentWalkState == StateManager.WalkState.Left)
            { source = new Rectangle(tick, 48, 32, 48); }
        }
        //Singleton
        private static PlayerManager instance = null;

        private PlayerManager()
        :base(){ }

        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerManager();

                return instance;
            }
        }
    }
}
