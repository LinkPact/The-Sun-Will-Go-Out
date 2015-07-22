using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public enum FontSize
    {
        Small,
        Medium,
        Big
    }

    public class FontManager
    {
        private Game1 Game;

        public static readonly Color FontSelectColor1 = Color.OrangeRed;
        public static readonly Color FontSelectColor2 = Color.LightYellow;

        private static Color fontColor;
        public Color FontColor { get { return fontColor; } set { fontColor = value; } }
        public static Color FontColorStatic { get { return fontColor; } set { fontColor = value; } }

        private static Vector2 fontOffset;
        public Vector2 FontOffset { get { return fontOffset; } set { fontOffset = value; } }
        public static Vector2 FontOffsetStatic { get { return fontOffset; } set { fontOffset = value; } }

        private static FontSize gameFontSize;
        public FontSize GameFontSize { get { return gameFontSize; } set { gameFontSize = value; } }
        public static FontSize GameFontSizeStatic { get { return gameFontSize; } set { gameFontSize = value; } }

        #region fonts

        private static SpriteFont iceland_10;
        private static SpriteFont iceland_12;
        private static SpriteFont iceland_14;         
        private static SpriteFont iceland_16;
        private static SpriteFont iceland_18;   
        private static SpriteFont iSL_Jupiter_24;
        private static SpriteFont iSL_Jupiter_28;
        private static SpriteFont iSL_Jupiter_36;
        
        #endregion

        public FontManager(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        {
            gameFontSize = FontSize.Medium;

            fontColor = Color.White;
            fontOffset = Vector2.Zero;
            iceland_10 = Game.Content.Load<SpriteFont>("Fonts/Iceland_10");
            iceland_12 = Game.Content.Load<SpriteFont>("Fonts/Iceland_12");
            iceland_14 = Game.Content.Load<SpriteFont>("Fonts/Iceland_14");
            iceland_16 = Game.Content.Load<SpriteFont>("Fonts/Iceland_16");
            iceland_18 = Game.Content.Load<SpriteFont>("Fonts/Iceland_18");
            iSL_Jupiter_24 = Game.Content.Load<SpriteFont>("Fonts/ISL_Jupiter_24");
            iSL_Jupiter_28 = Game.Content.Load<SpriteFont>("Fonts/ISL_Jupiter_28");
            iSL_Jupiter_36 = Game.Content.Load<SpriteFont>("Fonts/ISL_Jupiter_36");
        }

        public SpriteFont GetFont(int size)
        {
            int tempSize = size;

            //Changes size according to user-set fontsize
            switch (gameFontSize)
            {
                case FontSize.Small:
                    tempSize -= 2;
                    break;

                case FontSize.Medium:
                    break;

                case FontSize.Big:
                    tempSize += 2;
                    break;
            }

            //Returns font depending on size
            switch (tempSize)
            {
                case 10:
                    fontOffset = new Vector2(-2, 2);
                    return iceland_10;

                case 12:
                    fontOffset = new Vector2(-2, 2);
                    return iceland_12;

                case 14:
                    fontOffset = new Vector2(-1, 0);
                    return iceland_14;

                case 16:
                    fontOffset = new Vector2(-1, 1);
                    return iceland_16;

                case 18:
                    fontOffset = new Vector2(-1, 1);
                    return iceland_18;

                case 24:
                    fontOffset = Vector2.Zero;
                    return iSL_Jupiter_24;

                case 28:
                    fontOffset = Vector2.Zero;
                    return iSL_Jupiter_28;

                case 36:
                    fontOffset = Vector2.Zero;
                    return iSL_Jupiter_36;

                default:
                    throw new ArgumentException("Invalid font size.");

            }
        }

        public static SpriteFont GetFontStatic(int size)
        {
            int tempSize = size;

            //Changes size according to user-set fontsize
            switch (gameFontSize)
            {
                case FontSize.Small:
                    tempSize -= 2;
                    break;

                case FontSize.Medium:
                    break;

                case FontSize.Big:
                    tempSize += 2;
                    break;
            }

            //Returns font depending on size
            switch (tempSize)
            {
                case 10:
                    fontOffset = new Vector2(-2, 2);
                    return iceland_10;

                case 12:
                    fontOffset = new Vector2(-2, 2);
                    return iceland_12;

                case 14:
                    fontOffset = new Vector2(-1, 0);
                    return iceland_14;

                case 16:
                    fontOffset = new Vector2(-1, 1);
                    return iceland_16;

                case 18:
                    fontOffset = new Vector2(-1, 1);
                    return iceland_18;

                case 24:
                    fontOffset = Vector2.Zero;
                    return iSL_Jupiter_24;

                case 28:
                    fontOffset = Vector2.Zero;
                    return iSL_Jupiter_28;

                default:
                    throw new ArgumentException("Invalid font size.");

            }
        }
    }
}
