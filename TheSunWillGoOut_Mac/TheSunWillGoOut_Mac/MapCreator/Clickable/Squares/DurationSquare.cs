using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class DurationSquare : Square
    {
        
        private static Sprite even;
        private static Sprite gradient;

        public DurationSquare(SquareData data, Vector2 position, Sprite spriteSheet, Coordinate coordinate)
            : base(data, position, spriteSheet, coordinate)
        {
            even = spriteSheet.GetSubSprite(new Rectangle(144, 0, 6, 13));
            gradient = spriteSheet.GetSubSprite(new Rectangle(164, 0, 6, 13));
            
            emptySquare = spriteSheet.GetSubSprite(new Rectangle(50, 0, 6, 13));
            displaySprite = emptySquare;
        }

        protected override void UpdateState()
        {
            if (IsLeftClicked())
            {
                clickInformation = new SquareClick(true, data, coordinate);
            }
            else if (IsRightClicked())
            {
                clickInformation = new SquareClick(false, data, coordinate);
            }

            if (data != null)
                overlayDisplay = GetEventSprite(((DurationSquareData)data).durationEventType);
        }

        public static Sprite GetEventSprite(DurationEventType eventType)
        {
            switch (eventType)
            {
                case DurationEventType.even:
                    {
                        return even;
                    }
                case DurationEventType.gradient:
                    {
                        return gradient;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid eventType in data");
                    }
            }
        }

        protected override List<String> GetSquareInfo()
        {
            List<String> stringList = new List<String>();
            stringList.Add("Enemy: " + data.enemyType.ToString());
            stringList.Add("Movement: " + data.movement.ToString());
            stringList.Add("Event type: " + ((DurationSquareData)data).durationEventType.ToString());

            if (data.eventData != null)
            {
                List<String> eventParameters = data.eventData.GetEventSettings();

                foreach (String parameter in eventParameters)
                {
                    stringList.Add(parameter);
                }
            }

            return stringList;
        }
    }
}
