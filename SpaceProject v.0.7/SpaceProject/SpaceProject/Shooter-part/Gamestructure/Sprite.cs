using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Snodd fran forelasning
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public Nullable<Rectangle> SourceRectangle { get; private set; }

        public int Width
        {
            get { return SourceRectangle.Value.Width; }
        }

        public int Height
        {
            get { return SourceRectangle.Value.Height;  }
        }

        public Sprite()
        {
        Texture = null;
        SourceRectangle = null;
        }

        public Sprite(Texture2D tex, Nullable<Rectangle> rect = null)
        {
            Initialize(tex, rect);
        }

        public void Initialize(Texture2D tex, Nullable<Rectangle> rect = null)
        {
            Texture = tex;
            SourceRectangle = rect;
        }

        public Sprite GetSubSprite(Nullable<Rectangle> rect)
        {
            return new Sprite(Texture, rect);
        }
    }
}
