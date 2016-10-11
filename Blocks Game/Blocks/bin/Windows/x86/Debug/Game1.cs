using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileGame
{

    public class Game1 : Game
    {
        //Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int height;
        int width;

        //Game1 Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Initializer
        protected override void Initialize()
        {
            //Makes mouse visible
            IsMouseVisible = true;

            //Sets screen dimensions
            width = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            height = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();           

            //Initializes States
            StateManager.Instance.CurrentGameState = StateManager.GameState.Menu;

            //Initializes all singltons     
            PauseScreen.Instance.Initialize(graphics);
            GamePlaying.Instance.Initialize(width, height);
            MainMenu.Instance.Initialize(graphics);
            HUD.Instance.Initialize();
            MapManager.Instance.Initialize();
            ContentManager.Instance.LoadContent(Content, GraphicsDevice);                      
            PlayerManager.Instance.Initialize();
            FilterManager.Instance.Initialize();
            base.Initialize();          
        }

        //Load Stuff
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);    
        }

        //Unload Stuff
        protected override void UnloadContent()
        {

        }

        //Update
        protected override void Update(GameTime gameTime)
        {
            //Update volume
            ContentManager.Instance.PlaySong(MapManager.Instance.Volume);

            //Update mouse
            InputManager.Instance.Update();

            //Exit
            if (PauseScreen.Instance.Exit)
                Exit();

            //Checks GameState
            switch (StateManager.Instance.CurrentGameState)
            {
                case StateManager.GameState.Playing:
                    GamePlaying.Instance.Update(graphics, gameTime);
                    break;
                case StateManager.GameState.Pause:
                    PauseScreen.Instance.Update();
                    break;
                case StateManager.GameState.Menu:
                    MainMenu.Instance.Update(graphics);
                    break;
                case StateManager.GameState.EndScene:
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        //Draw Stuff
        protected override void Draw(GameTime gameTime)
        {
            //Blue Color
            //GraphicsDevice.Clear(new Color(72,223,255));
            GraphicsDevice.Clear(new Color(0, 0, 0));
            //Checks GameState
            switch (StateManager.Instance.CurrentGameState)
            {
                case StateManager.GameState.Playing:
                    GamePlaying.Instance.Draw(spriteBatch);
                    HUD.Instance.Draw(spriteBatch);
                    break;
                case StateManager.GameState.Pause:
                    GamePlaying.Instance.Draw(spriteBatch);
                    HUD.Instance.Draw(spriteBatch);
                    PauseScreen.Instance.Draw(spriteBatch);
                    break;
                case StateManager.GameState.Menu:
                    MainMenu.Instance.Draw(spriteBatch);
                    break;
                case StateManager.GameState.EndScene:
                    break;
                default:
                    break;
            }
            

            base.Draw(gameTime);
        }
    }
}
