using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ShieldEffectGenerator
    {

        public static ShieldEffect GenerateStandardShieldEffect(Game1 game, Sprite spriteSheet,
            GameObjectVertical source)
        { 
            float size = source.BoundingWidth / 2;
            ShieldEffect shieldEffect = new ShieldEffect(game, spriteSheet, source.Position, 
                source.Direction, source.Speed, size);
            return shieldEffect;
        }
    }
}
