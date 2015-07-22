using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class EscortDataCapsule
    {
        private OverworldShip shipToDefend;
        public OverworldShip ShipToDefend { get { return shipToDefend; } set { shipToDefend = value; } }

        private int shipToDefendHP;
        public int ShipToDefendHP { get { return shipToDefendHP; } set { shipToDefendHP = value; } }

        private List<OverworldShip> enemyShips;
        public List<OverworldShip> EnemyShips { get { return enemyShips; } set { enemyShips = value; } }

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

        private List<int> timedMessageTriggers;
        public List<int> TimedMessageTriggers { get { return timedMessageTriggers; } set { timedMessageTriggers = value; } }

        private float freighterSpeed;
        public float FreighterSpeed { get { return freighterSpeed; } set { freighterSpeed = value; } }

        public EscortDataCapsule(OverworldShip shipToDefend, String shipIntroductionText, List<OverworldShip> enemyShips,
            Vector2 startingPoint, List<String> levels, int enemyAttackStartTime, int enemyAttackFrequency)
        {
            Setup(shipToDefend, enemyShips, startingPoint, levels, enemyAttackStartTime, enemyAttackFrequency);

            allyShips = new List<OverworldShip>();
            shipToDefendHP = 2000;
        }

        public EscortDataCapsule(OverworldShip shipToDefend, List<OverworldShip> enemyShips, List<OverworldShip> allyShips,
            Vector2 startingPoint, int enemyAttackStartTime, int enemyAttackFrequency,
            int shipToDefendHP, List<String> levels, List<int> timedMessageTriggers, float speed)
        {
            Setup(shipToDefend, enemyShips, startingPoint, levels, enemyAttackStartTime, enemyAttackFrequency);

            this.allyShips = allyShips;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;
            this.shipToDefendHP = shipToDefendHP;
            this.timedMessageTriggers = timedMessageTriggers;
            freighterSpeed = speed;

            if (this.allyShips == null)
            {
                this.allyShips = new List<OverworldShip>();
            }
        }

        private void Setup(OverworldShip shipToDefend, List<OverworldShip> enemyShips,
            Vector2 startingPoint, List<String> levels, int enemyAttackStartTime, int enemyAttackFrequency)
        {
            this.shipToDefend = shipToDefend;
            this.enemyShips = enemyShips;
            this.startingPoint = startingPoint;
            this.levels = levels;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;

            if (this.enemyShips == null)
            {
                this.enemyShips = new List<OverworldShip>();
            }

            if (this.levels == null)
            {
                this.levels = new List<String>();
            }
        }
    }
}
