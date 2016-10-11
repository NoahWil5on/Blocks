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
    class Enemy
    {
        //Fields
        Rectangle rect;
        Rectangle source;
        GameObject obj;
        Random rand;

        Color enemyColor;
        int value;
        bool right;
        bool wallHit;
        bool flip;
        int delay;
        int value2;
        int value3;
        int altDelay;
        int delay2;

        public enum WalkState
        {
            Forward,
            Left,
            Right
        }

        WalkState currentWalkState;

        //Properties
        public Rectangle Source { get { return source; } }
        public Rectangle Rectangle { get { return obj.Rectangle; } }
        public Color Color { get { return enemyColor; } }

        public Enemy()
        {
            rect = new Rectangle(
                InputManager.Instance.MousePosition.X,
                InputManager.Instance.MousePosition.Y,
                PlayerManager.Instance.Rectangle.Width,
                PlayerManager.Instance.Rectangle.Height);
            source = new Rectangle(0,0,32,48);

            obj = new GameObject(rect);
            right = true;
            delay = 0;
            rand = new Random();
            value = rand.Next(10, 60);
            value2 = rand.Next(10, 60);
            value3 = rand.Next(20, 1000);
            delay2 = 0;
            altDelay = 0;
            flip = false;
        }
        public void Update(GameTime gameTime)
        {
            enemyColor = Color.White;
            foreach(Rectangle w in MapManager.Instance.Water)
            {
                if (obj.Rectangle.Intersects(w))
                    enemyColor = Color.Aqua;
            }
            obj.Update();        
            MoveUpdate();
            JumpUpdate();
            SourceRect(gameTime);
        }
        public void MoveUpdate()
        {
            if (right)
            {
                if (obj.Right(PlayerManager.Instance.Speed))
                {
                    flip = true;
                    wallHit = true;
                    obj.Jump(-7);
                }
                else
                    wallHit = false;
                currentWalkState = WalkState.Right;
            }
            else
            {
                if (obj.Left(PlayerManager.Instance.Speed))
                {
                    flip = true;
                    wallHit = true;
                    obj.Jump(-7);
                }
                else
                    wallHit = false;
                currentWalkState = WalkState.Left;
            }
            if (flip)
                altDelay++;
            if (altDelay > value2)
            {
                altDelay = 0;
                value2 = rand.Next(10, 60);
                flip = false;
                if (wallHit)
                {
                    if (right)
                        right = false;
                    else
                        right = true;
                }
            }
            delay2++;
            if (delay2 > value3)
            {
                value3 = rand.Next(20, 1000);
                delay2 = 0;
                if (right)
                    right = false;
                else
                    right = true;
            }
        }
        public void JumpUpdate()
        {
            delay++;
            if(delay > value)
            {
                value = rand.Next(20,80);
                delay = 0;
                obj.Jump(-7);
            }
        }
        public void SourceRect(GameTime gameTime)
        {
            int tick = 32 * ((int)(gameTime.TotalGameTime.Milliseconds / 100) % 4);
            if (currentWalkState == WalkState.Forward)
            { source = new Rectangle(0, 0, 32, 48); }
            else if (currentWalkState == WalkState.Right)
            { source = new Rectangle(tick, 96, 32, 48); }
            else if (currentWalkState == WalkState.Left)
            { source = new Rectangle(tick, 48, 32, 48); }
        }
    }
}
