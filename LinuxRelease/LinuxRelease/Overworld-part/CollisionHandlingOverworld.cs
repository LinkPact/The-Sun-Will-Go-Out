using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class CollisionHandlingOverWorld
    {
        private static Texture2D lineTexture;

        public static void LoadLineTexture(Game1 Game)
        {
            lineTexture = Game.Content.Load<Texture2D>("Overworld-Sprites/lineTexture");
        }

        //If called: allows player to enter system depending of the system name the player is currently over.
        public static void PlayerOverSystem(GameStateManager stateManager, GameObjectOverworld playerObject, GameObjectOverworld systemObject)
        { 
            if (playerObject.Class == "Player" && systemObject.Class == "System")
            {
                if (systemObject.name == "system1")
                {                    
                   // GameStateManager.currentSystem = "planetSystem1";
                    stateManager.ChangeState("System1State");
                }

                else if (systemObject.name == "system2")
                {
                    //GameStateManager.currentSystem = "planetSystem2";
                    stateManager.ChangeState("System2State");
                }

                else if (systemObject.name == "system3")
                {
                    //GameStateManager.currentSystem = "planetSystem3";
                    stateManager.ChangeState("System3State");
                }   
            }
        }

        public static void DrawRectAroundObject(Game1 Game, SpriteBatch spriteBatch, Rectangle objectBounds)
        {
            //Draws the upper horizontal line above the object
            spriteBatch.Draw(lineTexture,
                 new Vector2(objectBounds.Left, objectBounds.Top),
                 null,
                 Color.Green,
                 0,
                 Vector2.Zero,
                 new Vector2(objectBounds.Width, 2), 
                 SpriteEffects.None,
                 0);

            //Draws the lower horizontal line below the object
            spriteBatch.Draw(lineTexture,
                new Vector2(objectBounds.Left,
                            objectBounds.Bottom),
                null, 
                Color.Green, 
                0, 
                Vector2.Zero,
                new Vector2(objectBounds.Width, 2), 
                SpriteEffects.None, 
                0);

            //Draws the left vertical line left of the object
            spriteBatch.Draw(lineTexture,
                new Vector2(objectBounds.Left, objectBounds.Top), 
                null, 
                Color.Green,
                0, 
                Vector2.Zero,
                new Vector2(2, objectBounds.Height), 
                SpriteEffects.None, 
                0);

            //Draws the right vertical line right of the object
            spriteBatch.Draw(lineTexture,
                new Vector2(objectBounds.Right, objectBounds.Top), 
                null,
                Color.Green, 
                0, 
                Vector2.Zero,
                new Vector2(2, objectBounds.Height), 
                SpriteEffects.None, 
                0);

        }
    }

}
