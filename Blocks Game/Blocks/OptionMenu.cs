using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileGame
{
    class OptionMenu
    {

        //Fields
        private bool inGame;
        private bool exitClick;
        private bool enterClick;
        private bool settingActive;
        private bool volumeClick;
        private bool sfxClick;

        private int volume;
        private int sfx;
        private int volumeX;
        private int sfxX;
        private int scroll;       

        private Rectangle menu;
        private Rectangle settings;        
        private Rectangle character;
        private Rectangle settingRect;
        private Rectangle characterRect;
        private Rectangle exit;
        private Rectangle enter;
        private Rectangle volumeSlide;
        private Rectangle sfxSlide;
        private Rectangle active;
        private GraphicsDeviceManager graphics;
        private CharacterChoice characterChoice;

        private Color exitColor;
        private Color enterColor;
        private Color characterColor;

        //Properties
        public bool ExitClick { get { return exitClick; } set { exitClick = value; } }
        public bool EnterClick { get { return enterClick; } set { enterClick = value; } }
        public int Volume { get { return volume; } set { volume = value; } }
        public int SFX { get { return sfx; } set { sfx = value; } }

        //Constructor
        public OptionMenu(bool inGame, GraphicsDeviceManager graphics)
        {
            scroll = 0;
            this.inGame = inGame;
            this.graphics = graphics;
            settingActive = true;
            sfxClick = false;
            volumeClick = false;
            //If in menu initialize rectangles
            if (!inGame)
            {
                menu = new Rectangle(
                    GamePlaying.Instance.Width / 2 - 217,
                    GamePlaying.Instance.Height / 2 - 175,
                    435, 350);
                character = new Rectangle(
                    menu.X + 5,
                    menu.Y + 52,
                    419, 290);
                settings = new Rectangle(
                    menu.X + 12,
                    menu.Y + 52,
                    412, 290);
                settingRect = new Rectangle(
                    settings.X,
                    settings.Y - 5,
                    80, 25);
                characterRect = new Rectangle(
                    settings.X + 80,
                    settings.Y - 5,
                    80, 25);
                exit = new Rectangle(
                    settings.X + 1,
                    settings.Y + settings.Height - 40,
                    206, 40);
                enter = new Rectangle(
                    settings.X + 206,
                    settings.Y + settings.Height - 40,
                    206, 40);                
                volumeX = 62 + menu.X;
                sfxX = 62 + menu.X;
            }
            UpdateRectangles();
            if (MapManager.Instance.WorldVolume < 0)
                volume = 30;
            else
                volume = MapManager.Instance.WorldVolume;
            if (MapManager.Instance.WorldSFX < 0)
                sfx = 30;
            else
                sfx = MapManager.Instance.WorldSFX;
            sfxX += (int)(2.88 * sfx);
            volumeX += (int)(2.88 * volume);
        }
        //Update
        public void Update()
        {           
            if (settingActive)
                UpdateSettings();
            //else
             //   UpdateCharacter();
            UpdateButtons();
        }
        //Update Setting Tab
        public void UpdateSettings()
        {
            UpdateSlide();

            volumeSlide = new Rectangle(
                volumeX, 
                menu.Y+148,
                22,22);
            sfxSlide = new Rectangle(
                sfxX,
                menu.Y + 222,
                22, 22);
        }
        private void UpdateSlide()
        {
            if (volumeSlide.Contains(InputManager.Instance.MousePosition))
            {
                if (InputManager.Instance.LeftClick)
                {
                    volumeClick = true;
                }
            }
            if (volumeClick && InputManager.Instance.LeftHold)
            {
                if (InputManager.Instance.MousePosition.X > 62 + menu.X)
                    volumeX = InputManager.Instance.MousePosition.X - 11;
                if (volumeX < 62 + menu.X)
                    volumeX = 62 + menu.X;
                else if (volumeX > 350 + menu.X)
                    volumeX = 350 + menu.X;
            }
            else
                volumeClick = false;

            if (sfxSlide.Contains(InputManager.Instance.MousePosition))
            {
                if (InputManager.Instance.LeftClick)
                {
                    sfxClick = true;
                }
            }
            if (sfxClick && InputManager.Instance.LeftHold)
            {
                if (InputManager.Instance.MousePosition.X > 62 + menu.X)
                    sfxX = InputManager.Instance.MousePosition.X - 11;
                if (sfxX < 62 + menu.X)
                    sfxX = 62 + menu.X;
                else if (sfxX > 350 + menu.X)
                    sfxX = 350 + menu.X;
            }
            else
                sfxClick = false;
            MapManager.Instance.WorldSFX = (int)Math.Round((sfxX - menu.X - 62) / 2.88);
            MapManager.Instance.WorldVolume = (int)Math.Round((volumeX - menu.X - 62) / 2.88);
        }
        //Update Character Tab
        public void UpdateCharacter(Rectangle rect)
        {
        Rectangle view = new Rectangle(
            graphics.GraphicsDevice.Viewport.X,
            graphics.GraphicsDevice.Viewport.Y,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);

            Rectangle temp = new Rectangle(rect.X + graphics.GraphicsDevice.Viewport.X,
            rect.Y + graphics.GraphicsDevice.Viewport.Y + scroll,
            rect.Width, rect.Height);

            if (temp.Contains(InputManager.Instance.MousePosition)
            && view.Contains(InputManager.Instance.MousePosition))
            {
                characterColor = Color.LightBlue;
                if (!temp.Contains(InputManager.Instance.PreviousMousePosition)
                        || !view.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.Volume);
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.Volume);
                    ContentManager.Instance.CurrentCharacter = characterChoice.Dict[rect];
                }
                if(rect == active)
                    characterColor = Color.Gold;
            }
        }
                   
        //Draw Everything
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            //Menu
            spriteBatch.Draw(
                ContentManager.Instance.OptionScreen,
                menu,
                Color.White);
            //Setting Tab
            if (settingActive)
            {
                if (!inGame)
                {
                    spriteBatch.Draw(
                        ContentManager.Instance.Character,
                        character,
                        Color.White);
                }
                spriteBatch.Draw(
                    ContentManager.Instance.Settings,
                    settings,
                    Color.White);
                spriteBatch.Draw(
                    ContentManager.Instance.Slider,
                    volumeSlide,
                    Color.White);
                spriteBatch.Draw(
                    ContentManager.Instance.Slider,
                    sfxSlide,
                    Color.White);
            }
            
            //Character Tab
            else
            {                
                spriteBatch.Draw(
                    ContentManager.Instance.Settings,
                    settings,
                    Color.White);
                spriteBatch.Draw(
                    ContentManager.Instance.Slider,
                    volumeSlide,
                    Color.White);
                spriteBatch.Draw(
                    ContentManager.Instance.Slider,
                    sfxSlide,
                    Color.White);
                if (!inGame)
                {                   
                    spriteBatch.Draw(
                        ContentManager.Instance.Character,
                        character,
                        Color.White);              
                }
            }
            //Buttons
            spriteBatch.Draw(
                ContentManager.Instance.ExitButton,
                exit,
                exitColor);
            spriteBatch.Draw(
                ContentManager.Instance.ResumeButton,
                enter,
                enterColor);
            if (!settingActive)
            {
                CharacterDraw(spriteBatch);          
            }
            
        }
        //Responsible for some cool stuff
        public void CharacterDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin();
            graphics.GraphicsDevice.Viewport = new Viewport(
                character.X + 40,
                character.Y + 40,
                character.Width - 80,
                character.Height - 100);
                       
            foreach (Rectangle r in characterChoice.Dict.Keys)
            {
                if (ContentManager.Instance.CurrentCharacter == characterChoice.Dict[r])
                    active = r;
                if (active == r)
                    characterColor = Color.Gold;
                else
                    characterColor = Color.White;                  
                RectangleDrawCorrect(r, characterChoice.Dict[r], spriteBatch);
            }
            //UpdateCharacter();
            Scroll();           
            /*spriteBatch.Draw(
                    ContentManager.Instance.MainMenu,
                    men,
                    Color.White);*/
            /*spriteBatch.Draw(
                    ContentManager.Instance.Hunter,
                    new Rectangle(0, 0, 32, 48),
                    new Rectangle(0, 0, 32, 48),
                    Color.White);*/
        }
        public void RectangleDrawCorrect(Rectangle rect, Texture2D text, SpriteBatch spriteBatch)
        {
            UpdateCharacter(rect);
            spriteBatch.Draw(
                text,
                new Rectangle(rect.X, rect.Y+scroll,rect.Width,rect.Height),
                new Rectangle(0, 0, 32, 48),
                characterColor);
            rect = default(Rectangle);
        }
        private void Scroll()
        {
            if(InputManager.Instance.CurrentMouse.ScrollWheelValue 
                < InputManager.Instance.PreviousMouse.ScrollWheelValue)
            {
                scroll -= 15;
            }
            else if (InputManager.Instance.CurrentMouse.ScrollWheelValue
                > InputManager.Instance.PreviousMouse.ScrollWheelValue)
            {
                scroll += 15;
            }
            if(scroll > 0)
            {
                scroll = 0;
            }
            if (scroll < -characterChoice.Dict.Count / 5 * 90)
            {
                scroll = -characterChoice.Dict.Count/5*90;
            }
        }
        //Update Rectangles
        private void UpdateRectangles()
        {
            //If in game deal with world coordinates
            if (inGame)
            {
                menu = new Rectangle(
                GamePlaying.Instance.Width / 2 - 217 - GamePlaying.Instance.WorldX,
                GamePlaying.Instance.Height / 2 - 175 - GamePlaying.Instance.WorldY,
                435, 350);
                character = new Rectangle(
                    menu.X + 5,
                    menu.Y + 52,
                    419, 290);
                settings = new Rectangle(
                    menu.X + 12,
                    menu.Y + 52,
                    412, 290);
                settingRect = new Rectangle(
                    settings.X,
                    settings.Y - 5,
                    80, 25);
                characterRect = new Rectangle(
                    settings.X + 80,
                    settings.Y - 5,
                    80, 25);
                exit = new Rectangle(
                    settings.X + 1,
                    settings.Y + settings.Height - 40,
                    206, 40);
                enter = new Rectangle(
                    settings.X + 206,
                    settings.Y + settings.Height - 40,
                    206, 40);
                volumeX = 62 + menu.X;
                sfxX = 62 + menu.X;
            }
        }
        //Update Buttons
        private void UpdateButtons()
        {
            exitColor = Color.White;
            enterColor = Color.White;
            //Exit Button
            if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Escape))
            {
                ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                exitClick = true;
            }
            if (exit.Contains(InputManager.Instance.MousePosition))
            {
                exitColor = Color.LightBlue;
                if(!exit.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                if (InputManager.Instance.LeftClick)
                {
                    exitClick = true;
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                }

            }
            //Enter Button
            else if (enter.Contains(InputManager.Instance.MousePosition))
            {
                enterColor = Color.LightBlue;
                if (!enter.Contains(InputManager.Instance.PreviousMousePosition))
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Pop, MapManager.Instance.SFX);
                if (InputManager.Instance.LeftClick)
                {
                    enterClick = true;
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                }
            }
            //Setting Tab
            if (settingRect.Contains(InputManager.Instance.MousePosition))
            {
                if (InputManager.Instance.LeftClick)
                {
                    ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                    characterChoice = null;
                    settingActive = true;
                }
            }
            //Character Tab
            else if (!inGame)
            { 
            if (characterRect.Contains(InputManager.Instance.MousePosition))
                {
                    if (InputManager.Instance.LeftClick)
                    {
                        ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                        characterChoice = new CharacterChoice();
                        characterChoice.AddCharacters(1);
                        settingActive = false;
                    }
                }
            }
        }
    }
}
