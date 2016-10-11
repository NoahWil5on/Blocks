using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileGame
{
    class Button
    {
        //Fields
        int width;
        int height;
        int posX;
        int posY;
        Vector2 center;
        int centerX;
        bool click;
        string text;
        Color buttonColor;
        Rectangle buttonRect;
        Rectangle label;

        //Properties
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int PosY { get { return posY; } }
        public int PosX { get { return posX; } }
        public bool Click { get { return click; } }
        public Rectangle Rectangle { get { return buttonRect; } }
        public string Text { get { return text; } }

        //Constructor
        public Button(int posX, int posY, int height, string text)
        {  
            width = height/3 * 25;
            this.height = height;
            this.posX = posX;
            this.posY = posY;
            this.text = text;

            buttonColor = new Color(0, 0, 0, 200);
            center = new Vector2();
            center = ContentManager.Instance.Induction_20.MeasureString(text);
            centerX = posX + width / 2 - (int)center.X / 2;
            click = false;
            buttonRect = new Rectangle(posX, posY, width, height);
            label = new Rectangle(posX + width/8, posY, width- width/4, height);
        }

        public void Update()
        {
            click = false;
            buttonColor = Color.Black;
            if (buttonRect.Contains(InputManager.Instance.MousePosition))
            {
                if (!buttonRect.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                buttonColor = new Color(50,50,255);
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    click = true;
                }
            }
            buttonColor.A = 200;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                ContentManager.Instance.BlankButton,
                buttonRect,
                buttonColor);
            spriteBatch.Draw(
                ContentManager.Instance.BlankButtonLabel,
                label,
                new Color(0,0,0,200));
            spriteBatch.DrawString(
                ContentManager.Instance.Induction_20,
                text,
                new Vector2(centerX, buttonRect.Y + 10),
                Color.White);
        }
    }
}
