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
    public sealed class HUD
    {
        //Fields
        private static Rectangle source;
        private static char inChar;
        private static Rectangle textureSquare;
        private static Rectangle backSquare;
        private static Rectangle editButton;
        private static Rectangle editButtonBack;
        private static int scroll;
        private static bool edit;

        //Properties
        public char Source
        {
            get { return inChar; }
        }
        public bool Edit { get { return edit; } }
        public Rectangle EditRect { get { return editButton; } }

        public void Initialize()
        {
            inChar = '1';
            edit = true;               
        }
        public void Update()
        {
            if (!MainMenu.Instance.StoryMode)
            {
                if (inChar != 'g')
                    source = MapManager.Instance.SourceRect(inChar);
                else
                    source = new Rectangle(288, 0, 32, 32);
                CurrentBlock();
                if (edit) { Scroll(); }
                Editor();
            }
            else
            {
                edit = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!MainMenu.Instance.StoryMode)
            {
                if (StateManager.Instance.CurrentGameState == StateManager.GameState.Pause)
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ContentManager.Instance.GrayScale);
                else
                    spriteBatch.Begin();
                if (edit)
                {
                    spriteBatch.Draw(
                        ContentManager.Instance.EditButton,
                        backSquare,
                        Color.Black
                        );
                    spriteBatch.Draw(
                        ContentManager.Instance.Map,
                        textureSquare,
                        source,
                        Color.White
                        );
                    spriteBatch.Draw(
                        ContentManager.Instance.EditButton,
                        editButtonBack,
                        Color.Black);
                    spriteBatch.Draw(
                        ContentManager.Instance.EditButton,
                        editButton,
                        Color.White);

                }
                else
                {
                    spriteBatch.Draw(
                        ContentManager.Instance.EditButton,
                        editButtonBack,
                        new Rectangle(0, 0, 80, 40),
                        new Color(0, 0, 0, 120),
                        0,
                        new Vector2(0),
                        SpriteEffects.FlipHorizontally,
                        0);
                    spriteBatch.Draw(
                        ContentManager.Instance.EditButton,
                        editButton,
                        new Rectangle(0, 0, 80, 40),
                        new Color(255, 255, 255, 120),
                        0,
                        new Vector2(0),
                        SpriteEffects.FlipHorizontally,
                        0);
                }
                spriteBatch.End();
            }
        }
        public void Scroll()
        {
            switch (scroll)
            {
                case 0:
                    inChar = '1';
                    break;
                case 1:
                    inChar = 'w';
                    break;
                case 2:
                    inChar = 'g';
                    break;
                case 3:
                    inChar = 'r';
                    break;
                case 4:
                    inChar = 'R';
                    break;
                case 5:
                    inChar = 'Q';
                    break;
                default:
                    break;
            }            
            if(InputManager.Instance.CurrentMouse.ScrollWheelValue > InputManager.Instance.PreviousMouse.ScrollWheelValue)
            {
                scroll++;
            }
            else if (InputManager.Instance.CurrentMouse.ScrollWheelValue < InputManager.Instance.PreviousMouse.ScrollWheelValue)
            {
                scroll++;
            }
            if (scroll > 5)
                scroll = 0;
            else if (scroll < 0)
                scroll = 5;
        }
        public void CurrentBlock()
        {
            textureSquare = new Rectangle(
                GamePlaying.Instance.Width - 50 - GamePlaying.Instance.WorldX,
                10 - GamePlaying.Instance.WorldY,
                40, 40);
            backSquare = new Rectangle(
                textureSquare.X - 3,
                textureSquare.Y - 3,
                46, 46);
        }
        public void Editor()
        {
            editButton = new Rectangle(
                GamePlaying.Instance.Width -100 - GamePlaying.Instance.WorldX,
                GamePlaying.Instance.Height -75 - GamePlaying.Instance.WorldY,
                60, 30);
            editButtonBack = new Rectangle(
                editButton.X-2,
                editButton.Y-2,
                64,34);
            if (edit)
            {
                if(editButton.Contains(InputManager.Instance.MousePosition) && InputManager.Instance.LeftClick)
                {
                    edit = false;
                    MapManager.Instance.Release = false;
                }
            }
            else
            {
                if (editButton.Contains(InputManager.Instance.MousePosition) && InputManager.Instance.LeftClick)
                {
                    edit = true;
                    MapManager.Instance.Release = false;
                }
            }
        }

        //Singleton
        private static HUD instance = null;

        private HUD() { }

        public static HUD Instance
        {
            get {
                if (instance == null)
                    instance = new HUD();
                    return instance;
                }
        }
    }
}
