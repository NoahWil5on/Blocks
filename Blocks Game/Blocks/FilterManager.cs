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
    public sealed class FilterManager
    {

        private static Color mainFilter;

        public Color Filter { get { return mainFilter; } set { mainFilter = value; } }

        public void Initialize()
        {
            mainFilter = Color.White;
        }
        public Color Calculate(Color c)
        {
            c.R = (byte)((c.R * mainFilter.R) / 256);
            c.G = (byte)((c.G * mainFilter.G) / 256);
            c.B = (byte)((c.B * mainFilter.B) / 256);
            c.A = (byte)((c.A * mainFilter.A) / 256);
            return c;
        }
        public Color GrayScale(Color c)
        { 
            int gray = (c.R + c.B + c.A + c.G) / 4;
            return c;
        }


        //Singleton
        private static FilterManager instance = null;
       
        public static FilterManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new FilterManager();
                return instance;               
            }
        }

        private FilterManager() { }
    }
}
