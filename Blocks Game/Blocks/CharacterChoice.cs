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
    class CharacterChoice
    {
        Dictionary<Rectangle, Texture2D> characterOption;

        public Dictionary<Rectangle, Texture2D> Dict { get { return characterOption; } }

        public CharacterChoice()
        {
            characterOption = new Dictionary<Rectangle, Texture2D>();
        }

        public void AddCharacters(int num)
        {
            //distributes character textures from content
            for(int i = 0; i < ContentManager.Instance.CharacterList.Count; i++)
            {
                characterOption.Add(                  
                    new Rectangle(
                        (((i%4) * 100) ),
                        (((i/4) * 90) ),
                        32, 48),
                        ContentManager.Instance.CharacterList[i]);
            }
        }
    }
}
