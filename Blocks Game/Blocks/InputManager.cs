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
    public sealed class InputManager
    {
        //Fields
        private static Point mousePosition, previousMousePosition;
        private static MouseState currentMouse, previousMouse;

        private static KeyboardState currentKeyboard, previousKeyboard;

        //Properties
        public Point MousePosition { get { return mousePosition; } }
        public Point PreviousMousePosition { get { return previousMousePosition; } }

        public MouseState CurrentMouse { get { return currentMouse; } }
        public MouseState PreviousMouse { get { return previousMouse; } }
        public KeyboardState CurrentKeyboard { get { return currentKeyboard; } }
        public KeyboardState PreviousKeyboard { get { return previousKeyboard; } }

        public bool LeftClick { get { return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released; } }
        public bool LeftHold { get { return currentMouse.LeftButton == ButtonState.Pressed; } }

        //Update
        public void Update()
        {
            previousMousePosition = mousePosition;
            previousMouse = currentMouse;
            previousKeyboard = currentKeyboard;

            UpdateKeyboard();
            UpdateMouse();
        }

        //Updates Mouse
        public void UpdateMouse()
        {
            currentMouse = Mouse.GetState();
            if(StateManager.Instance.CurrentGameState != StateManager.GameState.Menu)
            {
                mousePosition.X = currentMouse.Position.X - GamePlaying.Instance.WorldX;
                mousePosition.Y = currentMouse.Position.Y - GamePlaying.Instance.WorldY;
            }
            else
            {
                mousePosition.X = currentMouse.Position.X;
                mousePosition.Y = currentMouse.Position.Y;
            }        
        }

        //Updates Keyboard
        public void UpdateKeyboard()
        {
            currentKeyboard = Keyboard.GetState();
        }

        //Singleton
        private static InputManager instance = null;

        private InputManager() { }

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();
                return instance;
            }
        }

    }
}
