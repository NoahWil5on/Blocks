using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TileGame
{
    class CreateFile
    {
        //Fields
        private Rectangle createRect;
        private Rectangle create;
        private Rectangle exit;
        private Rectangle storyRect;
        private Vector2 cursor;

        private Color sColor;
        private Color eColor;
        private Color cColor;

        private bool eClicked;
        private bool cClicked;
        private bool story;

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
        public bool Story { get { return story; } }
        public bool CClicked { get { return cClicked; } set { cClicked = value; } }
        public Rectangle CreateRect { get { return createRect; } }
        
        //Constructor
        public CreateFile()
        {
            eClicked = false;
            cClicked = false;
            typedString = "";
            int i = 0;
            dictionary = new Dictionary<Keys, string>();
            story = true;
            foreach (string s in characters)
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
        //Update Buttons
        public void UpdateButton()
        {
            //Colors
            eColor = Color.White;
            cColor = Color.White;
            sColor = Color.White;

            //Create Button
            if (create.Contains(InputManager.Instance.MousePosition))
            {
                if (!create.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                cColor = Color.LightBlue;
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    cClicked = true;
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
            //Story Mode Buttun
            if (storyRect.Contains(InputManager.Instance.MousePosition))
            {
                if (!storyRect.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                sColor = Color.LightBlue;
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    if (story)
                        story = false;
                    else
                        story = true;
                }
            }
        }
        //Draw
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(
                ContentManager.Instance.CreateScreen,
                createRect,
                Color.White);
            spritebatch.Draw(
                ContentManager.Instance.StartButton,
                create,
                cColor);
            spritebatch.Draw(
                ContentManager.Instance.ExitButton,
                exit,
                eColor);
            if (story)
            {
                Vector2 width = ContentManager.Instance.Arcade_20.MeasureString("Story Mode");
                spritebatch.Draw(
                    ContentManager.Instance.EditButton,
                    storyRect,
                    sColor);
                spritebatch.DrawString(
                    ContentManager.Instance.Arcade_20,
                    "Story Mode",
                    new Vector2(
                        storyRect.X + storyRect.Width/2 - width.X/2,
                        storyRect.Y-30),
                    new Color(30,30,30));
            }
            else
            {
                Vector2 width = ContentManager.Instance.Arcade_20.MeasureString("Sandbox Mode");
                spritebatch.Draw(
                    ContentManager.Instance.EditButton,
                    storyRect,
                    sColor);
                spritebatch.DrawString(
                    ContentManager.Instance.Arcade_20,
                    "Sandbox Mode",
                    new Vector2(
                        storyRect.X + storyRect.Width / 2 - width.X / 2,
                        storyRect.Y - 30),
                    new Color(30, 30, 30));
                spritebatch.Draw(
                    ContentManager.Instance.EditButton,
                    storyRect,
                    new Rectangle(0,0,
                    ContentManager.Instance.EditButton.Width,
                    ContentManager.Instance.EditButton.Height),
                    sColor,
                    0,
                    new Vector2(),
                    SpriteEffects.FlipHorizontally,
                    0);
            }
            spritebatch.DrawString(
                ContentManager.Instance.Arcade_20,
                typedString,
                new Vector2(cursor.X, cursor.Y + 3),
                Color.Black);
            spritebatch.DrawString(
                ContentManager.Instance.Arcade_20,
                typedString,
                cursor,
                Color.White);
        }
        //Update Rectangles
        public void UpdateRectangles()
        {
            createRect = new Rectangle(
                GamePlaying.Instance.Width / 2 - 150,
                GamePlaying.Instance.Height / 2 - 175,
                300, 350);
            create = new Rectangle(
                createRect.X + 8,
                createRect.Y + 110,
                284, 60);
            exit = new Rectangle(
                create.X,
                create.Y + 57,
                284, 60);
            storyRect = new Rectangle(
                create.X + create.Width / 2 - 40,
                create.Y + 170,
                80, 40);
            cursor = new Vector2(
                createRect.X + (createRect.Width / 2 -
                ContentManager.Instance.Arcade_20.MeasureString(typedString).X / 2),
                createRect.Y + 75);
        }
        //Update Keys
        public void UpdateKeys()
        {
            //New input
            foreach (Keys k in keys)
            {
                if (InputManager.Instance.CurrentKeyboard.IsKeyDown(k)
                    && InputManager.Instance.PreviousKeyboard.IsKeyUp(k)
                    && ContentManager.Instance.Arcade_20.MeasureString(typedString).X < 240)
                {                    
                    typedString += dictionary[k];                    
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    MainMenu.Instance.Error = false;
                }
            }
            //Back Click
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Back)
                && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Back)
                && typedString.Length > 0)
            {
                typedString = typedString.Remove(typedString.Length - 1, 1);
                ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                timer = 2;
                MainMenu.Instance.Error = false;
            }
            //Back Hold
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
