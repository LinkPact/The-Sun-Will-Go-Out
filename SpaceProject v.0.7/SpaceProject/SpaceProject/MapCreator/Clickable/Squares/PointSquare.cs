using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class PointSquare : Square
    {
        private static Sprite point;
        private static Sprite line;
        private static Sprite square;
        private static Sprite triangle;

        public PointSquare(PointSquareData data, Vector2 position, Sprite spriteSheet, Coordinate coordinate)
            : base(data, position, spriteSheet, coordinate)
        {
            point = spriteSheet.GetSubSprite(new Rectangle(144, 0, 13, 13));
            line = spriteSheet.GetSubSprite(new Rectangle(104, 0, 13, 13));
            square = spriteSheet.GetSubSprite(new Rectangle(118, 0, 13, 13));
            triangle = spriteSheet.GetSubSprite(new Rectangle(131, 0, 13, 13));
            
            emptySquare = spriteSheet.GetSubSprite(new Rectangle(50, 0, 13, 13));
            displaySprite = emptySquare;
        }

        public override void Initialize()
        {
            base.Initialize();

            // If those lines are encountered July 2014 or later, feel free to remove them. // Jakob 140610
            //((PointSquareData)data).pointEventType = PointEventType.point;
            //data.eventData = new PointEventData(ActiveData.pointEventState);
        }

        protected override void UpdateState()
        {
            if (IsTargetLeftPressed())
            {
                data.enemyType = ActiveData.enemyState;
                data.movement = ActiveData.movementState;
                ((PointSquareData)data).pointEventType = ActiveData.pointEventState;
                data.eventData = new PointEventData(ActiveData.pointEventState);
            }
            else if (IsTargetRightPressed())
            {
                data.enemyType = EnemyType.none;
                ((PointSquareData)data).pointEventType = PointEventType.point;
            }

            if (data != null)
                overlayDisplay = GetEventSprite(((PointSquareData)data).pointEventType);
        }

        public static Sprite GetEventSprite(PointEventType eventType)
        {
            switch (eventType)
            {
                case PointEventType.point:
                    {
                        return point;
                    }
                case PointEventType.line:
                    {
                        return line;
                    }
                case PointEventType.square:
                    {
                        return square;
                    }
                case PointEventType.vformation:
                    {
                        return triangle;
                    }
                case PointEventType.horizontal:
                    {
                        return point;
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
            stringList.Add("Event type: " + ((PointSquareData)data).pointEventType.ToString());

            List<String> eventParameters = data.eventData.GetEventSettings();

            foreach (String parameter in eventParameters)
            {
                stringList.Add(parameter);
            }

            return stringList;
        }
    }
}
