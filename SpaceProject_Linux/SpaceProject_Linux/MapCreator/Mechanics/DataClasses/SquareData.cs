using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    //Contains basal information about square that can be read
    //directly from textfile.
    public abstract class SquareData
    {
        public EnemyType enemyType;
        public EventData eventData;
        public Movement movement;
        
        //Called when loading from text file
        public SquareData(String loadText)
        { }

        //Called when creating blank data
        public SquareData()
        {
            enemyType = EnemyType.none;
            movement = Movement.None;
        }

        //Called when copying data from other SquareData
        public SquareData(SquareData sd)
        {
            enemyType = sd.enemyType;
            movement = sd.movement;
        }

        protected void SetEnemy(String enemyString)
        {
            enemyType = DataConversionLibrary.GetEnemyEnumFromString(enemyString);
        }

        protected void SetMovement(String movementString)
        {
            movement = DataConversionLibrary.GetMovementEnumFromString(movementString);
        }

        public virtual void Initialize()
        {
            enemyType = EnemyType.none;
            movement = Movement.None;
        }

        public override string ToString()
        {
            String squareString = " ";

            squareString += DataConversionLibrary.GetEnemyStringFromEnum(enemyType);
            squareString += DataConversionLibrary.GetMovementStringFromEnum(movement);

            return squareString;
        }



        
    }
}
