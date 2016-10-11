using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileGame
{
    public sealed class PauseScreen
    { 
        //Fields
        //Rectangle pause;
        Button resumeButton;
        Button optionButton;
        Button saveButton;
        Button exitButton;
        OptionMenu optionMenu;
        GraphicsDeviceManager graphics;

        Color oColor;
        Color rColor;
        Color sColor;
        Color eColor;

        int cheat;
        bool save;
        bool exit;
        bool exitExtension;

        //Properties
        public bool Exit {
            get { return exit; }
            set { exit = value; }
        }
        public int Cheat { set { cheat = value; } }

        //Initialize
        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            save = false;
            exit = false;
        }
        //Update
        public void Update()
        {
            OptionUpdate();
            if(optionMenu != null)
            {
                optionMenu.Update();
            }
            
            //Update Rectangles
            RectangleUpdate();
            ButtonUpdate();
        }
        public void ButtonUpdate()
        {
            rColor = Color.White;
            oColor = Color.White;
            sColor = Color.White;
            eColor = Color.White;           
            if ((InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Escape)
                && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Escape)))
            {
                ContentManager.Instance.PlaySound(ContentManager.Instance.Click, MapManager.Instance.SFX);
                MapManager.Instance.Release = false;
                resumeButton = null;
                optionMenu = null;
                StateManager.Instance.CurrentGameState = StateManager.GameState.Playing;
            }
            if (resumeButton != null)
            {
                if (exitExtension && optionMenu == null)
                {
                    resumeButton.Update();
                    optionButton.Update();
                    saveButton.Update();
                    exitButton.Update();
                    if(resumeButton.Click)
                    {
                        resumeButton = null;
                        MapManager.Instance.Release = false;
                        StateManager.Instance.CurrentGameState = StateManager.GameState.Playing;
                    }
                    if (optionButton.Click)
                    {
                        optionMenu = new OptionMenu(true, graphics);
                    }
                    if (saveButton.Click)
                    {
                        Save();
                        save = true;
                    }
                    if (exitButton.Click)
                    {
                        resumeButton = null;
                        StateManager.Instance.CurrentGameState = StateManager.GameState.Menu;
                    }
                }
            }
            exitExtension = true;
        }
        private void OptionUpdate()
        {
            if (optionMenu != null)
            {
                optionMenu.Update();
                if (optionMenu.ExitClick || optionMenu.EnterClick)
                {
                    optionMenu.ExitClick = false;
                    optionMenu.EnterClick = false;
                    optionMenu = null;
                    exitExtension = false;
                }
            }
        }
        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 stringLength = ContentManager.Instance.Induction_20.MeasureString("saved");
            if (cheat != 0)
            {
                spriteBatch.Begin();
                if (resumeButton != null)
                {
                    resumeButton.Draw(spriteBatch);
                    optionButton.Draw(spriteBatch);
                    saveButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                }
                if (save)
                {
                    spriteBatch.DrawString(
                        ContentManager.Instance.Induction_20,
                        "saved",
                        new Vector2(exitButton.PosX + exitButton.Width/2 -stringLength.X/2, exitButton.PosY + 70),
                        Color.White);

                }
                if (optionMenu != null)
                    optionMenu.Draw(spriteBatch, graphics);
                spriteBatch.End();
            }
            else
            {
                cheat = 1;
                save = false;
            }     
        }
        //Update Button Positions
        public void RectangleUpdate()
        {
            if (resumeButton == null)
            {
                resumeButton = new Button(
                  GamePlaying.Instance.Width / 2 - 208 - GamePlaying.Instance.WorldX,
                  GamePlaying.Instance.Height / 2 - 175 - GamePlaying.Instance.WorldY,
                  50, "Resume");
            }
            optionButton = new Button(
                resumeButton.PosX,
                resumeButton.PosY + 57,
                50, "Option");
            saveButton = new Button(
                optionButton.PosX,
                optionButton.PosY + 57,
                50, "save");
            exitButton = new Button(
                saveButton.PosX,
                saveButton.PosY + 57,
                50, "Exit");
        }
        //Saves Game
        public void Save()
        {
            StreamWriter output = null;
            StreamReader input = null;
            if (MainMenu.Instance.StoryMode)
            {
                input = new StreamReader(MainMenu.Instance.TypedFile + "/currentLevel.txt");
                string line;
                List<string> files = new List<string>();
                while((line = input.ReadLine()) != null)
                {
                    files.Add(line);
                }
                files[0] = "?:" + LevelManager.Instance.CurrentLevel;
                input.Close();
                output = new StreamWriter(MainMenu.Instance.TypedFile + "/currentLevel.txt");
                output.Flush();
                foreach(string s in files)
                {
                    output.WriteLine(s);
                }
                output.Close();
            }
            output = new StreamWriter(MainMenu.Instance.ActiveFile);
            output.Flush();
            for (int b = 0; b < MapManager.Instance.Tile.GetLongLength(1); b++)
            {
                for (int i = 0; i < MapManager.Instance.Tile.GetLongLength(0); i++)
                {
                    output.Write(MapManager.Instance.Tile[i,b]);
                }
                output.WriteLine();
            }
            output.WriteLine("P:" + PlayerManager.Instance.Rectangle.X.ToString() + ": PlayerX");
            output.WriteLine("P:" + PlayerManager.Instance.Rectangle.Y.ToString() + ": PlayerY");
            output.WriteLine("P:" + GamePlaying.Instance.WorldX.ToString() + ": CameraX");
            output.WriteLine("P:" + GamePlaying.Instance.WorldY.ToString() + ": CameraY");
            output.Close();
        }

        //Singleton
        private static PauseScreen instance = null;

        private PauseScreen() { }

        public static PauseScreen Instance
        {
            get
            {
                if (instance == null)
                    instance = new PauseScreen();
                return instance;
            }
        }
    }
}
