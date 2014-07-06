using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EscortDataCapsule
    {
        private OverworldShip shipToDefend;
        public OverworldShip ShipToDefend { get { return shipToDefend; } set { shipToDefend = value; } }

        private List<String> shipToDefendText;
        public List<String> ShipToDefendText { get { return shipToDefendText; } set { shipToDefendText = value; } }

        private List<String> attackStartText;
        public List<String> AttackStartText { get { return attackStartText; } set { attackStartText = value; } }

        private int shipToDefendHP;
        public int ShipToDefendHP { get { return shipToDefendHP; } set { shipToDefendHP = value; } }

        private List<OverworldShip> enemyShips;
        public List<OverworldShip> EnemyShips { get { return enemyShips; } set { enemyShips = value; } }

        private List<String> enemyMessages;
        public List<String> EnemyMessages { get { return enemyMessages; } set { enemyMessages = value; } }

        private List<OverworldShip> allyShips;
        public List<OverworldShip> AllyShips { get { return allyShips; } set { allyShips = value; } }

        private Vector2 startingPoint;
        public Vector2 StartingPoint { get { return startingPoint; } set { startingPoint = value; } }

        private int enemyAttackStartTime;
        public int EnemyAttackStartTime { get { return enemyAttackStartTime; } set { enemyAttackStartTime = value; } }

        private int enemyAttackFrequency;
        public int EnemyAttackFrequency { get { return enemyAttackFrequency; } set { enemyAttackFrequency = value; } }

        private List<String> levels;
        public List<String> Levels { get { return levels; } set { levels = value; } }

        public EscortDataCapsule(OverworldShip shipToDefend, List<String> shipToDefendText, List<OverworldShip> enemyShips,
            List<String> enemyMessages, List<OverworldShip> allyShips, Vector2 startingPoint, List<String> attackStartText,
            int enemyAttackStartTime, int enemyAttackFrequency, int shipToDefendHP, List<String> levels)
        {
            this.shipToDefend = shipToDefend;
            this.shipToDefendText = shipToDefendText;
            this.enemyShips = enemyShips;
            this.enemyMessages = enemyMessages;
            this.allyShips = allyShips;
            this.startingPoint = startingPoint;
            this.attackStartText = attackStartText;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;
            this.shipToDefendHP = shipToDefendHP;
            this.levels = levels;

            if (this.enemyShips == null)
            {
                this.enemyShips = new List<OverworldShip>();
            }

            if (this.allyShips == null)
            {
                this.allyShips = new List<OverworldShip>();
            }

            if (this.enemyMessages == null)
            {
                this.enemyMessages = new List<String>();
            }

            if (this.levels == null)
            {
                this.levels = new List<String>();
            }

            if (this.attackStartText == null)
            {
                this.attackStartText = new List<String>();
            }
        }
    }
}
