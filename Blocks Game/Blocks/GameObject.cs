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
    class GameObject
    {
        //Fields
        private bool hitGround;
        private bool jump;
        private bool press;

        private double grav;
        private double gravity;

        private List<Rectangle> wall;
        private Rectangle rect;

        public Rectangle Rectangle { get { return rect; } set { rect = value; } }
        public double Grav { get { return gravity; } set { gravity = value; } }
        public double G { get { return grav; } set { grav = value; } }

        //Constructor
        public GameObject(Rectangle rect)
        {
            wall = MapManager.Instance.Wall;
            this.rect = rect;
            gravity = .2;
            grav = 0;
            hitGround = false;
            jump = false;
            press = false;
        }
        //Update
        public void Update()
        {
            //Constrain
            if (rect.Y <= 0)
            {
                grav = 0;
                rect = new Rectangle(rect.X, 1, rect.Width, rect.Height);
            }
            if (rect.X < 0)
            {
                rect = new Rectangle(0, rect.Y, rect.Width, rect.Height);
            }
            if (rect.X > MapManager.Instance.Tile.GetLength(0) * 32 - rect.Width)
            {
                rect = new Rectangle(
                    MapManager.Instance.Tile.GetLength(0) * 32 - rect.Width,
                    rect.Y, rect.Width, rect.Height);
            }
            Gravity();
        }
        //Gravity
        public void Gravity()
        {
            double dist = Math.Pow((rect.X - PlayerManager.Instance.Rectangle.X),2) + Math.Pow((rect.Y - PlayerManager.Instance.Rectangle.Y),2);
            //gravity
            rect = new Rectangle(rect.X, rect.Y + (int)grav, rect.Width, rect.Height);
            hitGround = jump;

            foreach (Rectangle r in wall)
            {
                if (rect.Intersects(r))
                {
                    if (grav > 0)
                    {
                        float volume = (MapManager.Instance.SFX / 20) * (float)grav;
                        if (volume > 1)
                            volume = 1;
                        rect = new Rectangle(rect.X, r.Y - rect.Height, rect.Width, rect.Height);
                        hitGround = false;
                        foreach (Rectangle w in MapManager.Instance.Water)
                        {
                            if (rect.Intersects(w) && grav > 1)
                            {
                                if(dist < 250000)
                                { ContentManager.Instance.PlaySound(ContentManager.Instance.Thump, volume);
                                    hitGround = true;
                                }
                            }
                        }
                        if (hitGround == false && grav > 2)
                        {    if (dist < 250000)
                            {
                                ContentManager.Instance.PlaySound(ContentManager.Instance.Thump, volume);
                            }
                        }
                        grav = 0;
                        jump = true;
                    }
                    else
                    {
                        jump = false;
                        rect = new Rectangle(rect.X, rect.Y - (int)grav, rect.Width, rect.Height);
                        grav = 0;
                    }
                }
            }

            //Acceleration due to gravity
            grav += gravity;
        }
        public void Jump(int speed)
        {
            if (press == false || jump == true)
            {
                if (PlayerManager.Instance.UnderWater)
                {
                    grav = speed+1;
                }
                else
                {
                    grav = speed;
                }
                jump = false;
                press = true;
            }
        }
        public bool Left(int speed)
        {
            rect = new Rectangle(rect.X - speed, rect.Y, rect.Width, rect.Height);
            foreach (Rectangle r in wall)
            {
                if (rect.Intersects(r))
                {
                    rect = new Rectangle(rect.X + speed, rect.Y, rect.Width, rect.Height);
                    return true;
                }

            }
            return false;
        }
        public bool Right(int speed)
        {
    
            rect = new Rectangle(rect.X + speed, rect.Y, rect.Width, rect.Height);
            foreach (Rectangle r in wall)
            {
                if (rect.Intersects(r))
                {
                    rect = new Rectangle(rect.X - speed, rect.Y, rect.Width, rect.Height);
                    return true;
                }
            }
            return false;
        }
    }
}
