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
    public sealed class MapManager
    {
        //fields
        private static List<string> spot;
        private static Rectangle background;
        private static List<Rectangle> wall;
        private static List<Rectangle> water;
        private static List<Rectangle> grass;
        private static List<Rectangle> flag;
        private static string[] files;
        private static Rectangle[,] tileRect;
        private static List<List<Rectangle>> list;
        private static int animation;
        private int worldVolume;
        private int worldSFX;

        private static Dictionary<List<Rectangle>, char> dictionary;
        private static Color color;

        private static char[,] tile;       
        private static int size;
        private static int timer;
        private static int xPos;
        private static int yPos;
        private static bool release;

        //Properties
        public List<Rectangle> Wall
        {
            get { return wall; }
        }
        public List<Rectangle> Water
        {
            get { return water; }
        }
        public List<Rectangle> Flag
        {
            get { return flag; }
        }
        public char[,] Tile { get { return tile; } }
        public bool Release { get { return release; } set { release = value; } }
        public int XPos { get { return xPos; } }
        public int YPos { get { return yPos; } }
        public int WorldVolume { get { return worldVolume; } set { worldVolume = value; } }
        public int WorldSFX { get { return worldSFX; } set { worldSFX = value; } }
        public float Volume { get { return (float)(worldVolume) / 100; } }
        public float SFX { get { return (float)(worldSFX) / 100; } }
        public string[] Files { get { return files; } }

        //Initializer
        public void Initialize()
        {
            wall = new List<Rectangle>();
            water = new List<Rectangle>();
            spot = new List<string>();
            grass = new List<Rectangle>();
            flag = new List<Rectangle>();
            dictionary = new Dictionary<List<Rectangle>, char>();
            list = new List<List<Rectangle>>();
            background = new Rectangle(0, 0, GamePlaying.Instance.Width, GamePlaying.Instance.Height);

            list.Add(water);
            list.Add(wall);
            list.Add(flag);

            dictionary.Add(water, 'w');
            dictionary.Add(wall, '1');
            dictionary.Add(grass, 'g');
            dictionary.Add(flag, 'Q');

            size = 32;
            worldSFX = 30;
            worldVolume = 30;
            color = Color.White;
        }
        public void FilesReader()
        {
            files = Directory.GetFiles("MapFiles");
        }
        //Reads map
        public void MapReader(string file)
        {
            StreamReader input = null;

            spot.Clear();
            tile = null;
            tileRect = null;
            wall.Clear();
            water.Clear();
            grass.Clear();
            flag.Clear();

            //prevents errors
            try
            {
                input = new StreamReader(file);
                string line;
                string[] position;
                int x = 0;
                //Runs through each line
                while ((line = input.ReadLine()) != null)
                {
                    if (line[0] != 'P')
                    {
                        //Creates lines
                        spot.Add(line);
                    }
                    else
                    {                        
                        position = line.Split(':');
                        switch (x)
                        {
                            case 0:
                                xPos = int.Parse(position[1]);
                                break;
                            case 1:
                                yPos = int.Parse(position[1]);
                                break;
                            case 2:
                                GamePlaying.Instance.WorldX = int.Parse(position[1]);
                                break;
                            case 3:
                                GamePlaying.Instance.WorldY = int.Parse(position[1]);
                                break;
                            default:
                                break;
                        }
                        x++;
                    }
                }
                //closes file
                input.Close();

                //Make arrays
                tile = new char[spot[1].Length, spot.Count];
                tileRect = new Rectangle[spot[1].Length, spot.Count];

                for (int i = 0; i < spot.Count; i++)
                {
                    for(int b = 0; b < spot[1].Length; b++)
                    {
                        tile[b, i] = spot[i][b];
                        tileRect[b, i] = new Rectangle(b * size, i * size, size, size);

                        //Rectangle properties
                        switch (tile[b, i])
                        {
                            case 'x':
                            case '1':
                            case '2':
                            case 'r':
                            case 'R':
                                wall.Add(tileRect[b, i]);
                                break;
                            case 'g':
                                grass.Add(tileRect[b, i]);
                                break;
                            case 'w':
                                water.Add(tileRect[b, i]);
                                break;
                            case 'Q':
                                flag.Add(tileRect[b, i]);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        //Updates Map
        public void Update()
        {
            MapEditor();

            if (timer % 30 == 1)
            {
                WaterPhysics();
                GrassGrow();
                for (int b = 0; b < tileRect.GetLongLength(1); b++)
                {
                    for (int i = 0; i < tileRect.GetLongLength(0); i++)
                    {
                        if (tile[i, b] == 'W')
                        {
                            tile[i, b] = 'w';
                        }
                    }
                }
            }
            background = new Rectangle(
                -GamePlaying.Instance.WorldX,
                -GamePlaying.Instance.WorldY,
                GamePlaying.Instance.Width,
                GamePlaying.Instance.Height);
            timer++;
        }

        //Draws map
        public void MapDraw(SpriteBatch spritebatch)
        {
            animation++;
            int x = -100;
            int y = -100;
            if (StateManager.Instance.CurrentGameState == StateManager.GameState.Pause)
                spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ContentManager.Instance.GrayScale);
            else
                spritebatch.Begin();
            spritebatch.Draw(
                ContentManager.Instance.FallingLights,
                background,
                new Rectangle(0,0, 
                ContentManager.Instance.FallingLights.Width,
                ContentManager.Instance.FallingLights.Height),
                Color.White);
            color = Color.White;
            for (int b = 0; b < tileRect.GetLongLength(1); b++)
            {
                for (int i = 0; i < tileRect.GetLongLength(0); i++)
                {
                    if (HUD.Instance.Edit)
                    {
                        if (tileRect[i, b].Contains(InputManager.Instance.MousePosition))
                        {
                            x = tileRect[i, b].X;
                            y = tileRect[i, b].Y;                            
                        }
                    }
                    if(tile[i, b] == 'w' || tile[i, b] == 'W')
                    {
                        color.A = 80;
                    }
                    else
                    {
                        color.A = 255;
                    }
                    spritebatch.Draw(
                                ContentManager.Instance.Map,
                                tileRect[i,b],
                                SourceRect(tile[i,b]),
                                FilterManager.Instance.Calculate(color));
                }
            }
            color = new Color(255, 0, 0, 1);
            if (StateManager.Instance.CurrentGameState == StateManager.GameState.Playing)
            {
                spritebatch.Draw(
                  ContentManager.Instance.FlatColor,
                  new Rectangle(x, y, size, size),
                  color);
            }
            spritebatch.End();
        }
        public void CurrentLevel()
        {
            string input;
            try
            {
                LevelManager.Instance.Initialize();
            }
            catch
            {
                Directory.CreateDirectory(MainMenu.Instance.TypedFile);
                string[] dirFiles = Directory.GetFiles("StoryModeStarter");
                foreach (string s in dirFiles)
                {
                    string fileName = Path.GetFileName(s);
                    string destFile = Path.Combine(MainMenu.Instance.TypedFile, fileName);
                    File.Copy(s, destFile, false);
                }
               LevelManager.Instance.Initialize();
            }

            input = LevelManager.Instance.CurrentLevel;
            MainMenu.Instance.ActiveFile = MainMenu.Instance.TypedFile + input;
            MapReader(MainMenu.Instance.TypedFile + input);
            
        }
        //Makes Edits to map
        public void MapEditor()
        {
            char current;
            if (release)
            {
                if (HUD.Instance.Edit)
                {
                    for (int b = 0; b < tileRect.GetLongLength(1); b++)
                    {
                        for (int i = 0; i < tileRect.GetLongLength(0); i++)
                        {
                            if (tileRect[i, b].Contains(InputManager.Instance.MousePosition)
                            && InputManager.Instance.LeftHold
                            && tile[i, b] != 'x'
                            && !tileRect[i, b].Intersects(PlayerManager.Instance.Rectangle)
                            && !HUD.Instance.EditRect.Contains(InputManager.Instance.MousePosition))
                            {
                                current  = HUD.Instance.Source;
                                if(HUD.Instance.Source == 'r' || HUD.Instance.Source == 'R')
                                {
                                    current = '1';
                                }
                                tile[i, b] = HUD.Instance.Source;
                                foreach (List<Rectangle> l in list)
                                {
                                    l.Remove(tileRect[i, b]);
                                    if (current == dictionary[l])
                                    {
                                        l.Add(tileRect[i, b]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (InputManager.Instance.LeftClick)
                release = true;
        }

        //Source Rectangle
        public Rectangle SourceRect(char input)
        {
            
            Rectangle source;
            switch (input)
            {
                //Grass
                case 'g':
                    source = new Rectangle(0, 0, 32, 32);
                    break;
               
                //Water
                case 'w':
                    source = new Rectangle(96+(32*((animation/20)%3)), 0, 32, 32);
                    break;

                case '2':
                    source = new Rectangle(64, 0, 32, 32);
                    break;
                //Wall
                case '1':
                    source = new Rectangle(32, 0, 32, 32);
                    break;
                case 'x':
                    source = new Rectangle(192, 0, 32, 32);
                    break;
                case 'r':
                    source = new Rectangle(224, 0, 32, 32);
                    break;
                case 'R':
                    source = new Rectangle(256, 0, 32, 32);
                    break;
                case 'Q':
                    source = new Rectangle(0, 32, 32, 32);
                    break;
                default:
                    source = new Rectangle(32, 32, 32, 32);
                    break;
            }
            return source;
        }

        //Water Physics
        public void WaterPhysics()
        {
            for (int b = 0; b < tile.GetLongLength(1); b++)
            {
                for (int i = 0; i < tile.GetLongLength(0); i++)
                {
                    if (tile[i,b] == 'w')
                    {
                        if (tile[i, b + 1] == 'g')
                        {
                            tile[i, b + 1] = 'W';

                            foreach (List<Rectangle> l in list)
                            {
                                l.Remove(tileRect[i, b + 1]);
                                if ('w' == dictionary[l])
                                {
                                    l.Add(tileRect[i, b + 1]);
                                }
                            }
                        }
                        else if(tile[i, b + 1] != 'w')
                            {
                            if ((i + 1) < tile.GetLength(0))
                            {
                                if (tile[i + 1, b] == 'g')
                                {
                                    tile[i + 1, b] = 'W';

                                    foreach (List<Rectangle> l in list)
                                    {
                                        l.Remove(tileRect[i + 1, b]);
                                        if ('w' == dictionary[l])
                                        {
                                            l.Add(tileRect[i + 1, b]);
                                        }
                                    }
                                }
                            }
                            if (i != 0)
                            {
                                if (tile[i - 1, b] == 'g')
                                {
                                    tile[i - 1, b] = 'W';

                                    foreach (List<Rectangle> l in list)
                                    {
                                        l.Remove(tileRect[i - 1, b]);
                                        if ('w' == dictionary[l])
                                        {
                                            l.Add(tileRect[i - 1, b]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GrassGrow()
        {
            for (int b = 0; b < tile.GetLongLength(1); b++)
            {
                for (int i = 0; i < tile.GetLongLength(0); i++)
                {
                    if (tile[i, b] == '1')
                    {
                        if (b != 0)
                        {
                            if (tile[i, b - 1] == 'g')
                            {
                                tile[i, b] = '2';
                            }
                        }
                        
                    }
                    else if (tile[i, b] == '2')
                    {
                        if (tile[i, b - 1] == '1' || 
                            tile[i, b - 1] == 'w' ||
                            tile[i, b - 1] == '2' ||
                            tile[i, b - 1] == 'r' ||
                            tile[i, b - 1] == 'R')
                        {
                            tile[i, b] = '1';
                        }
                    }
                }
            }
        }

        //Instance
        private static MapManager instance = null;

        private MapManager() { }

        //Never need to open this up
        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MapManager();

                return instance;
            }
        }
    }
}
