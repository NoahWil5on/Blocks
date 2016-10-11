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
    public sealed class MainMenu
    {
        //Fields
        private static Rectangle menuRect;
        private static Button start;
        private static Button load;
        private static Button options;
        private static Button exit;
        Vector2 stringLength;
        private static Viewport view;
        private static string activeFile;
        private static string typedFile;
        private static string loadString;
        private static bool exitExtension;
        private static bool error;
        private static bool storyMode;
        private LoadFile loadFile;
        private CreateFile createFile;
        private OptionMenu optionMenu;
        private GraphicsDeviceManager graphics;      

        private static Color sColor;
        private static Color lColor;
        private static Color oColor;
        private static Color eColor;

        //Properties
        public bool StoryMode { get { return storyMode; } set { storyMode = value; } }
        public string ActiveFile { get { return activeFile; }set { activeFile = value; }}
        public string TypedFile { get { return typedFile; } }
        public Rectangle MenuRect { get { return menuRect; } }
        public bool Error { get { return error; } set { error = value; } }

        //Initialize
        public void Initialize(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            
            menuRect = new Rectangle(0,0,
                GamePlaying.Instance.Width,
                GamePlaying.Instance.Height);             
            loadFile = null;
            createFile = null;
            exitExtension = true;
            error = false;
            
        }
        public void Update(GraphicsDeviceManager graphics)
        {
            view = new Viewport(0, 0, 10000, 10000);
            graphics.GraphicsDevice.Viewport = view;
            CreateUpdate();
            LoadUpdate();
            OptionUpdate();
            ButtonUpdate();
            ButtonCreate();                             
        }
        public void ButtonCreate()
        {
            if(start == null)
            {
                int center = GamePlaying.Instance.Width / 2 - 208;
                start = new Button(center, GamePlaying.Instance.Height / 3, 50, "Create");
                load = new Button(center, start.PosY + 70, 50, "Load");
                options = new Button(center, load.PosY + 70, 50, "Options");
                exit = new Button(center, options.PosY + 70, 50, "Exit");
            }
        }
        public void OptionUpdate()
        {
            if(optionMenu != null)
            {
                optionMenu.Update();
                if(optionMenu.ExitClick || optionMenu.EnterClick)
                {
                    optionMenu.ExitClick = false;
                    optionMenu.EnterClick = false;
                    optionMenu = null;
                    exitExtension = false;
                }
            }
        }
        public void CreateUpdate()
        {
            if (createFile != null)
            {
                createFile.Update();
                if (InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Enter)
                    && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Enter)
                    || createFile.CClicked)
                {
                    createFile.CClicked = false;
                    error = false;
                    string[] exisitingDirectories = Directory.GetDirectories("MapFiles/");
                    string[] exisitingFiles = Directory.GetFiles("MapFiles/");
                    foreach (string s in exisitingDirectories)
                    {
                        if (s.ToUpper() == "MAPFILES/" + createFile.TypedString.ToUpper())
                            error = true;
                    }                    
                    foreach (string s in exisitingFiles)
                    {
                        if (s.ToUpper() == "MAPFILES/" + createFile.TypedString.ToUpper() + ".TXT")
                            error = true;
                    }
                    if (!error)
                    {
                        typedFile = "MapFiles/" + createFile.TypedString;
                        if (createFile.Story == true)
                        { Load("MapFiles/tile.txt", false, true); storyMode = true; }

                        else
                        { Load("MapFiles/tile.txt", false, false); storyMode = false; }
                    }
                    else
                    {
                        error = true;
                        createFile.TypedString = "";
                    }
                }
                else if (createFile.EClicked)
                {
                    createFile.EClicked = false;
                    createFile = null;
                    exitExtension = false;
                    error = false;
                }
            }
        }
        public void LoadUpdate()
        {
            if (loadFile != null)
            {
                loadFile.Update();
                if ((InputManager.Instance.CurrentKeyboard.IsKeyDown(Keys.Enter)
                    && InputManager.Instance.PreviousKeyboard.IsKeyUp(Keys.Enter)) 
                    || loadFile.LClicked)
                {
                    error = false;
                    loadFile.LClicked = false;
                    try
                    {
                        if (loadFile.TypedString.ToUpper() != "TILE")
                        {
                            typedFile = "MapFiles/" + loadFile.TypedString;
                            Load("MapFiles/" + loadFile.TypedString + ".txt", true, false);
                            storyMode = false;
                            
                        }
                        else
                        {
                            loadFile.TypedString = "";
                            loadString = "Cannot Load \"Tile\"";
                            error = true;
                        }
                    }
                    catch (Exception e)
                    {
                        error = true;
                        string[] exisitingFiles = Directory.GetDirectories("MapFiles/");
                        foreach(string s in exisitingFiles)
                        {
                            if (s.ToUpper() == "MAPFILES/" + loadFile.TypedString.ToUpper())
                                error = false;
                        }
                        if(!error)
                        {
                            typedFile = "MapFiles/" + loadFile.TypedString;
                            Load("MapFiles/" + loadFile.TypedString, true, true);
                            storyMode = true;
                        }
                        else
                        {
                            loadFile.TypedString = "";
                            error = true;
                            loadFile.LClicked = false;
                            e.ToString();
                            loadString = "File Does Not Exist";
                        }    
                    }
                }
                else if (loadFile.EClicked)
                {
                    loadFile.EClicked = false;
                    loadFile = null;
                    exitExtension = false;
                    error = false;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            graphics.GraphicsDevice.Viewport = new Viewport(0,0,10000,10000);
            spriteBatch.Begin();
            spriteBatch.Draw(
                ContentManager.Instance.MainMenu,
                menuRect,
                Color.White);
            if(start != null)
            {
                start.Draw(spriteBatch);
                options.Draw(spriteBatch);
                exit.Draw(spriteBatch);
                load.Draw(spriteBatch);
            }
            if (loadFile != null)
            {
                loadFile.Draw(spriteBatch);
                if (error)
                {
                    stringLength = ContentManager.Instance.Lucida_12.MeasureString("File Already Exists");
                    spriteBatch.DrawString(
                        ContentManager.Instance.Lucida_12,
                        loadString,
                        new Vector2(
                            loadFile.LoadRect.X + loadFile.LoadRect.Width / 2 - stringLength.X / 2,
                            loadFile.LoadRect.Y + (loadFile.LoadRect.Height / 4) * 3),
                        Color.Red);
                }
            }
            else if (createFile != null)
            {
                createFile.Draw(spriteBatch);
                if (error)
                {
                    stringLength = ContentManager.Instance.Lucida_12.MeasureString("File Already Exists");
                    spriteBatch.DrawString(
                        ContentManager.Instance.Lucida_12,
                        "File Already Exists",
                        new Vector2(
                            createFile.CreateRect.X + createFile.CreateRect.Width / 2 - stringLength.X / 2,
                            createFile.CreateRect.Y + (createFile.CreateRect.Height / 16) * 11),
                        Color.Red);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin();
            if (optionMenu != null)
            {
                optionMenu.Draw(spriteBatch, graphics);
            }
            spriteBatch.End();
                    
        }

        public void ButtonUpdate()
        {
            sColor = Color.White;
            lColor = Color.White;
            oColor = Color.White;
            eColor = Color.White;
            if (loadFile == null && createFile == null && exitExtension && optionMenu == null)
            {
                if (start != null)
                {
                    start.Update();
                    options.Update();
                    load.Update();
                    exit.Update();
                    if (start.Click)
                    {
                        MapManager.Instance.FilesReader();
                        loadFile = null;
                        createFile = new CreateFile();
                    }
                    if (load.Click)
                    {
                        createFile = null;
                        loadFile = new LoadFile();
                    }
                    if (options.Click)
                    {
                        optionMenu = new OptionMenu(false, graphics);
                    }
                    if (exit.Click)
                    {
                        PauseScreen.Instance.Exit = true;
                    }
                }
            }
            exitExtension = true;
        }
        public void Load(string file, bool load, bool story)
        {
            activeFile = file;   
            if (story)
            {               
                MapManager.Instance.CurrentLevel();
            }
            else
            {
                activeFile = typedFile+ ".txt";
                MapManager.Instance.MapReader(file);
            }
            PlayerManager.Instance.Rectangle = new Rectangle(
                MapManager.Instance.XPos,
                MapManager.Instance.YPos,
                22,
                PlayerManager.Instance.Size);
            MapManager.Instance.Release = false;
            StateManager.Instance.CurrentGameState = StateManager.GameState.Playing;
            if (load)
            {               
                loadFile.LClicked = false;
                loadFile = null;
            }
            else if(createFile != null)
            {                
                createFile.CClicked = false;
                createFile = null;                
            }

            exitExtension = false;
            error = false;
        }
        //Singleton
        private static MainMenu instance = null;

        private MainMenu() { }

        public static MainMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new MainMenu();
                return instance;
            }
        }
    }
}
