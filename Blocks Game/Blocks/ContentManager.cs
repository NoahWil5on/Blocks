using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace TileGame
{
    public sealed class ContentManager
    {
        //Textures
        private static Texture2D mapTexture, pauseScreen, mainMenu, createMenu, loadMenu, 
            optionMenu, settings, character;
        private static Texture2D resumeButton, optionButton, saveButton, exitButton,
            loadButton, startButton, editButton, slider, blankButton, blankButtonLabel;
        private static Texture2D hunter, character2, character3, 
            character4, character5, character6, character7,
            character8, character9, character10, character11,
            character12;
        private static Texture2D flatColor;
        private static Texture2D currentCharacter;
        private static List<Texture2D> characterList;

        //Backgrounds
        private static Texture2D fallingLights;

        //Filter
        Effect grayScale;

        //Sounds
        private static SoundEffect pop, click, thump;
        private static SoundEffect song;       

        //Play Sounds
        private static SoundEffectInstance pSong;

        //Fonts
        private static SpriteFont lucida_12;
        private static SpriteFont arcade_20;
        private static SpriteFont induction_20;

        //Properties
        public Texture2D Map { get { return mapTexture; } }        

        public Texture2D PauseScreen { get { return pauseScreen; } }
        public Texture2D MainMenu { get { return mainMenu; } }
        public Texture2D LoadScreen { get { return loadMenu; } }
        public Texture2D CreateScreen { get { return createMenu; } }
        public Texture2D OptionScreen { get { return optionMenu; } }
        public Texture2D Settings { get { return settings; } }
        public Texture2D Character { get { return character; } }
        public Texture2D FlatColor { get { return flatColor; } }

        public Texture2D ResumeButton { get { return resumeButton; } }
        public Texture2D OptionButton { get { return optionButton; } }
        public Texture2D SaveButton { get { return saveButton; } }
        public Texture2D ExitButton { get { return exitButton; } }
        public Texture2D LoadButton { get { return loadButton; } }
        public Texture2D StartButton { get { return startButton; } }
        public Texture2D EditButton { get { return editButton; } }
        public Texture2D BlankButton { get { return blankButton; } }
        public Texture2D BlankButtonLabel { get { return blankButtonLabel; } }
        public Texture2D Slider { get { return slider; } }

        public List<Texture2D> CharacterList { get { return characterList; } }
        public Texture2D CurrentCharacter { get { return currentCharacter; } set { currentCharacter = value; } }
        public Texture2D Hunter { get { return hunter; } }


        public SpriteFont Lucida_12 { get { return lucida_12; } }
        public SpriteFont Arcade_20 { get { return arcade_20; } }
        public SpriteFont Induction_20 { get { return induction_20; } }

        //Sounds
        public SoundEffect Pop { get { return pop; } }
        public SoundEffect Click { get { return click; } }
        public SoundEffect Thump { get { return thump; } }
        public SoundEffect Song { get { return song; } }

        //Filters
        public Effect GrayScale { get { return grayScale; } }

        //Backgrounds
        public Texture2D FallingLights { get { return fallingLights; } }

        //Game Textures
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDevice graphics)
        {
            characterList = new List<Texture2D>();
            //Textures
            mapTexture = Content.Load<Texture2D>(@"Textures/TexturePack");
            flatColor = new Texture2D(graphics, 1, 1);
            flatColor.SetData<Color>(new Color[] { Color.White });

            //Menus
            pauseScreen = Content.Load<Texture2D>(@"Textures/Menus/PauseScreen");
            mainMenu = Content.Load<Texture2D>(@"Textures/Menus/MainMenu");
            loadMenu = Content.Load<Texture2D>(@"Textures/Menus/LoadScreen");
            createMenu = Content.Load<Texture2D>(@"Textures/Menus/CreateScreen");
            character = Content.Load<Texture2D>(@"Textures/Menus/Character");
            settings = Content.Load<Texture2D>(@"Textures/Menus/Settings");
            optionMenu = Content.Load<Texture2D>(@"Textures/Menus/OptionsScreen");

            //Buttons
            resumeButton = Content.Load<Texture2D>(@"Textures/Buttons/Resume");
            optionButton = Content.Load<Texture2D>(@"Textures/Buttons/Options");
            saveButton = Content.Load<Texture2D>(@"Textures/Buttons/SaveGame");
            exitButton = Content.Load<Texture2D>(@"Textures/Buttons/Exit");
            startButton = Content.Load<Texture2D>(@"Textures/Buttons/Create");
            loadButton = Content.Load<Texture2D>(@"Textures/Buttons/Load");
            editButton = Content.Load<Texture2D>(@"Textures/Buttons/editButton");
            slider = Content.Load<Texture2D>(@"Textures/Buttons/Slider");
            blankButton = Content.Load<Texture2D>(@"Textures/Buttons/BlankButton");
            blankButtonLabel = Content.Load<Texture2D>(@"Textures/Buttons/BlankButtonLabel");

            //Characters
            hunter = Content.Load<Texture2D>(@"Characters/Hunter");
            character2 = Content.Load<Texture2D>(@"Characters/Character2");
            character3 = Content.Load<Texture2D>(@"Characters/Character3");
            character4 = Content.Load<Texture2D>(@"Characters/Character4");
            character5 = Content.Load<Texture2D>(@"Characters/Character5");
            character6 = Content.Load<Texture2D>(@"Characters/Character6");
            character7 = Content.Load<Texture2D>(@"Characters/Character7");
            character8 = Content.Load<Texture2D>(@"Characters/Character8");
            character9 = Content.Load<Texture2D>(@"Characters/Character9");
            character10 = Content.Load<Texture2D>(@"Characters/Character10");
            character11 = Content.Load<Texture2D>(@"Characters/Character11");
            character12 = Content.Load<Texture2D>(@"Characters/Character12");

            //Simply adding texture to list adds it to character menu
            characterList.Add(hunter);
            characterList.Add(character2);
            characterList.Add(character3);
            characterList.Add(character4);
            characterList.Add(character5);
            characterList.Add(character6);
            characterList.Add(character7);
            characterList.Add(character8);
            characterList.Add(character9);
            characterList.Add(character10);
            characterList.Add(character11);
            characterList.Add(character12);
            

            currentCharacter = hunter;

            //Fonts
            lucida_12 = Content.Load<SpriteFont>("LucidaConsole_12");
            arcade_20 = Content.Load<SpriteFont>(@"Fonts/Arcade_20");
            induction_20 = Content.Load<SpriteFont>(@"Fonts/Induction_20");

            //Sounds
            pop = Content.Load<SoundEffect>(@"Sounds/Pop");
            click = Content.Load<SoundEffect>(@"Sounds/MouseClick");
            thump = Content.Load<SoundEffect>(@"Sounds/Thump");
            song = Content.Load<SoundEffect>(@"Sounds/Songs/Song");
            pSong = song.CreateInstance();

            //Filters
            grayScale = Content.Load<Effect>(@"SpriteEffects/GrayScale");

            //Backgrounds
            fallingLights = Content.Load<Texture2D>(@"Textures/Backgrounds/FallingLights");

        }
        public void PlaySound(SoundEffect sound, float volume)
        {
            SoundEffectInstance instance = sound.CreateInstance();
            instance.Volume = volume;
            instance.Play();
            if (instance.State != SoundState.Playing)
            {
                instance.Dispose();
            }
        }
        public void PlaySong(float volume)
        {
            pSong.IsLooped = true;
            pSong.Volume = volume;
            if (pSong.State != SoundState.Playing)
            { pSong.Play(); }
        }

        //Sington
        private static ContentManager instance = null;

        private ContentManager() { }

        public static ContentManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ContentManager();

                return instance;
            }
        }
    }
}
