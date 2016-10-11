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
    public sealed class GamePlaying
    {
        //Fields
        private Viewport view;
        private Color color;
        private List<Enemy> enemies;
        private int worldX;
        private int worldY;
        private int width;
        private int height;
        private double worldXSpeed;
        private double worldYSpeed;

        //Properties
        public int WorldX
        {
            get { return worldX; }
            set { worldX = value; }
        }
        public int WorldY
        {
            get { return worldY; }
            set { worldY = value; }
        }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Viewport View
        {
            get { return view; }
            set { view = value; }
        }
        //Initialize
        public void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;
            enemies = new List<Enemy>();
            worldX = 0;
            worldY = 0;
            worldXSpeed = 0;
            worldYSpeed = 0;
            color = Color.White;
                   
        }

        //Update
        public void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {   
            //Pause Screen
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Escape) 
                && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Escape))
            {
                PauseScreen.Instance.Cheat = 0;
                StateManager.Instance.CurrentGameState = StateManager.GameState.Pause;               
            }

            PlayerManager.Instance.Update(gameTime);
            Scroll(graphics);
            MapManager.Instance.Update();

            EnemyUpdate(gameTime);

            //Water physics      
            WaterUpdate();

            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Space))
            {
                if(InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Space))
                    enemies.Add(new Enemy());
            }
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Delete))
            {
                if (InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Delete))
                    enemies.Clear();
            }
            HUD.Instance.Update();
        }
        public void EnemyUpdate(GameTime gameTime)
        {
            foreach(Enemy e in enemies)
            {
                e.Update(gameTime);
            }
        }

        //Water Update
        public void WaterUpdate()
        {
            foreach (Rectangle r in MapManager.Instance.Water)
            {
                if (PlayerManager.Instance.Rectangle.Intersects(r))
                {
                    color = Color.Aqua;
                    PlayerManager.Instance.UnderWater = true;
                    PlayerManager.Instance.Gravity = .1;
                    PlayerManager.Instance.Speed = 2;
                    if (PlayerManager.Instance.Grav > 2)
                        PlayerManager.Instance.Grav = 2;
                    if ((int)PlayerManager.Instance.Grav == 0)
                        PlayerManager.Instance.Grav = .9;
                    break;
                }
                else
                {
                    color = new Color(200, 200, 200);
                    PlayerManager.Instance.UnderWater = false;
                    PlayerManager.Instance.Gravity = .3;
                    PlayerManager.Instance.Speed = 3;
                }
            }
        }

        //Draw Stuff
        public void Draw(SpriteBatch spriteBatch)
        {
            MapManager.Instance.MapDraw(spriteBatch);
            if (StateManager.Instance.CurrentGameState == StateManager.GameState.Pause)
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ContentManager.Instance.GrayScale);
            else
                spriteBatch.Begin();     
            foreach(Enemy e in enemies)
            {
                spriteBatch.Draw(
                    ContentManager.Instance.Hunter,
                    e.Rectangle,
                    e.Source,
                    FilterManager.Instance.Calculate(e.Color));
            }                 
            spriteBatch.Draw(
                        ContentManager.Instance.CurrentCharacter,
                        PlayerManager.Instance.Rectangle,
                        PlayerManager.Instance.SourceRect,
                        FilterManager.Instance.Calculate(color));
            spriteBatch.DrawString(
                ContentManager.Instance.Lucida_12,
                MainMenu.Instance.ActiveFile.ToString(),
               new Vector2(20 - worldX, 20 - worldY),             
               Color.White);
            spriteBatch.End();
        }

        //Scrolling
        public void Scroll(GraphicsDeviceManager graphics)
        {
            if (PlayerManager.Instance.Rectangle.X + worldX < width/2 - 150)
            {
                worldXSpeed = 3;
            }
            else if (PlayerManager.Instance.Rectangle.X + worldX > width/2 + 150)
            {
                worldXSpeed = -3;
            }
            else
            {
                worldXSpeed *= .95;
            }
            if (PlayerManager.Instance.Rectangle.Y + worldY < height/2 -100)
            {
                worldYSpeed = 2;
            }
            else if (PlayerManager.Instance.Rectangle.Y + worldY > height/2 + 100)
            {
                worldYSpeed = -2;
            }
            else
            {
                worldYSpeed *= .95;
            }
            worldX += (int)worldXSpeed;
            worldY += (int)worldYSpeed;
            if(worldX > 0)
                worldX = 0;
            if (worldY > 0)
                worldY = 0;
            if (worldX < -MapManager.Instance.Tile.GetLength(0) * 32 + width)
            {
                worldX = -MapManager.Instance.Tile.GetLength(0) * 32 + width;
            }


            view = new Viewport(worldX, worldY, 20000, 20000);
            graphics.GraphicsDevice.Viewport = view;
        }

        //Singleton
        private static GamePlaying instance = null;

        public static GamePlaying Instance
        {
            get
            {
                if (instance == null)
                    instance = new GamePlaying();
                return instance;
            }
        }

        private GamePlaying() { }
    }
}
