using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;5
using Microsoft.Xna.Framework.Audio;

namespace TileGame
{
    class LoadFile
    {
        //Fields
        private Rectangle loadRect;
        private Rectangle load;
        private Rectangle exit;
        private Vector2 cursor;

        private Color eColor;
        private Color lColor;

        private bool eClicked;
        private bool lClicked;
        private string typedString;
        private int timer;
        Dictionary<Keys, string> dictionary;
        //All available key presses
        Keys[] keys =
        {
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
            Keys.Z,

            Keys.D0, Keys.D1, Keys.D2, Keys.D3,
            Keys.D4, Keys.D5, Keys.D6, Keys.D7,
            Keys.D8, Keys.D9,

            Keys.Space,
        };
        string[] characters =
        {
            "a", "b", "c", "d", "e",
            "f", "g", "h", "i", "j",
            "k", "l", "m", "n", "o",
            "p", "q", "r", "s", "t",
            "u", "v", "w", "x", "y",
            "z",

            "0", "1", "2", "3",
            "4", "5", "6", "7",
            "8", "9",

            " ",
        };

        //Properties
        public string TypedString { get { return typedString; } set { typedString = value; } }
        public bool EClicked { get { return eClicked; } set { eClicked = value; } }
        public bool LClicked { get { return lClicked; } set { lClicked = value; } }
        public Rectangle LoadRect { get { return loadRect; } }
       
        //Constructor
        public LoadFile()
        {
            eClicked = false;
            lClicked = false;
            typedString = "";
            int i = 0;
            dictionary = new Dictionary<Keys, string>();
            foreach(string s in characters)
            {
                dictionary.Add(keys[i], s);
                i++;
            }
        }
        //Update
        public void Update()
        {
            UpdateRectangles();
            UpdateKeys();
            UpdateButton();        
        }
        //Update the buttons
        public void UpdateButton()
        {
            //Colors
            eColor = Color.White;
            lColor = Color.White;

            //Load Button
            if (load.Contains(InputManager.Instance.MousePosition))
            {
                if (!load.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                lColor = Color.LightBlue;
                if(InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    lClicked = true;
                }
            }
            //Exit Button
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Escape))
            {
                ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                eClicked = true;
            }
            if (exit.Contains(InputManager.Instance.MousePosition))
            {
                if (!exit.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                eColor = Color.LightBlue;
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    eClicked = true;
                }
            }
        }
        //Draw
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(
                ContentManager.Instance.LoadScreen,
                loadRect,
                Color.White);
            spritebatch.Draw(
                ContentManager.Instance.LoadButton,
                load,
                lColor);
            spritebatch.Draw(
                ContentManager.Instance.ExitButton,
                exit,
                eColor);
            spritebatch.DrawString(
                ContentManager.Instance.Arcade_20,
                typedString,
                new Vector2(cursor.X, cursor.Y+3),
                Color.Black);
            spritebatch.DrawString(
                ContentManager.Instance.Arcade_20,
                typedString,
                cursor,
                Color.White);
        }
        //Update the Rectangles
        public void UpdateRectangles()
        {
            loadRect = new Rectangle(
                GamePlaying.Instance.Width / 2 - 150,
                GamePlaying.Instance.Height/2-175,
                300, 350);
            load = new Rectangle(
                loadRect.X + 8,
                loadRect.Y + 110,
                284, 60);
            exit = new Rectangle(
                load.X,
                load.Y + 57,
                284, 60);
            cursor = new Vector2(
                loadRect.X + (loadRect.Width/2 - 
                ContentManager.Instance.Arcade_20.MeasureString(typedString).X/2),
                loadRect.Y + 75);
        }
        public void UpdateKeys()
        {
            //Type new characters
            foreach(Keys k in keys)
            {
                if (InputManager.Instance.CurrentKeyboard.IsKeyDown(k)
                    && InputManager.Instance.PreviousKeyboard.IsKeyUp(k)
                    && ContentManager.Instance.Arcade_20.MeasureString(typedString).X < 240)
                {
                    typedString += dictionary[k];
                    MainMenu.Instance.Error = false;
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                }
            }
            //Hit backspace
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Back)
                && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Back)
                && typedString.Length > 0)
            {
                typedString = typedString.Remove(typedString.Length - 1, 1);
                ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                MainMenu.Instance.Error = false;
                timer = 2;
            }
            //Hold backspace
            else if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Back)
                && InputManager.Instance.PreviousKeyboard.IsKeyDown(Keys.Back)
                && typedString.Length > 0)
            {
                if (timer % 8 == 1)
                {
                    typedString = typedString.Remove(typedString.Length - 1, 1);
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                }
                
                timer++;
            }
        }
    }
}
