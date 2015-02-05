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


        //If called: allows player to enter planet depending of the planet name the player is currently over.
        public static void PlayerOverPlanet(GameStateManager stateManager,    
                                            PlayerOverworld player,
                                            Planet planet)
        {
           
        }

        //If called: draws a rectangle around the GameObject sent into the method
        public static void DrawRectAroundObject(Game1 Game, SpriteBatch spriteBatch, GameObjectOverworld obj)
        {
            //Draws the upper horizontal line above the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X 
            - obj.sprite.SourceRectangle.Value.Width / 2 , obj.position.Y 
            - (obj.sprite.SourceRectangle.Value.Height / 2) -2), new Rectangle(0,0, 
            obj.sprite.SourceRectangle.Value.Width, 2),Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            //Draws the lower horizontal line below the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X 
            - obj.sprite.SourceRectangle.Value.Width / 2, obj.position.Y 
            - (obj.sprite.SourceRectangle.Value.Height / 2) + obj.sprite.SourceRectangle.Value.Height),
            new Rectangle(0, 0, obj.sprite.SourceRectangle.Value.Width, 2), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            //Draws the left vertical line left of the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2, obj.position.Y
            - obj.sprite.SourceRectangle.Value.Height / 2), new Rectangle(0,0, obj.sprite.SourceRectangle.Value.Height, 2), Color.Green,
            ((float)(Math.PI * 90) / 180), Vector2.Zero, 1, SpriteEffects.None, 0);

            //Draws the right vertical line right of the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2 + obj.sprite.SourceRectangle.Value.Width +2
            , obj.position.Y - obj.sprite.SourceRectangle.Value.Height / 2), new Rectangle(0,0, obj.sprite.SourceRectangle.Value.Height, 2),
            Color.Green, ((float)(Math.PI * 90) / 180), Vector2.Zero, 1, SpriteEffects.None, 0);
  
        }

        //If called: draws a rectangle around the GameObject sent into the method
        public static void DrawRectAroundObject(Game1 Game, SpriteBatch spriteBatch, GameObjectOverworld obj, Rectangle spriteRect)
        {
            //Draws the upper horizontal line above the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2, obj.position.Y
            - (obj.sprite.SourceRectangle.Value.Height / 2) - 2), new Rectangle(spriteRect.X, spriteRect.Y,
            obj.sprite.SourceRectangle.Value.Width, 2), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            //Draws the lower horizontal line below the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2, obj.position.Y
            - (obj.sprite.SourceRectangle.Value.Height / 2) + obj.sprite.SourceRectangle.Value.Height),
            new Rectangle(spriteRect.X, spriteRect.Y, obj.sprite.SourceRectangle.Value.Width, 2), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            //Draws the left vertical line left of the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2, obj.position.Y
            - obj.sprite.SourceRectangle.Value.Height / 2), new Rectangle(spriteRect.X, spriteRect.Y, obj.sprite.SourceRectangle.Value.Height, 2), Color.Green,
            ((float)(Math.PI * 90) / 180), Vector2.Zero, 1, SpriteEffects.None, 0);

            //Draws the right vertical line right of the object
            spriteBatch.Draw(obj.sprite.Texture, new Vector2(obj.position.X
            - obj.sprite.SourceRectangle.Value.Width / 2 + obj.sprite.SourceRectangle.Value.Width + 2
            , obj.position.Y - obj.sprite.SourceRectangle.Value.Height / 2), new Rectangle(spriteRect.X, spriteRect.Y, obj.sprite.SourceRectangle.Value.Height, 2),
            Color.Green, ((float)(Math.PI * 90) / 180), Vector2.Zero, 1, SpriteEffects.None, 0);

        }
    }

}
