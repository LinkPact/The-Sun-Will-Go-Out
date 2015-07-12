using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class EscortObjective : Objective
    {
        private EscortDataCapsule escortDataCapsule;
        private List<OverworldShip> enemies;
        private List<String> descriptions;

        private int startingNumberOfEnemyShips;
        private int numberOfEnemyShips;
        private int enemyShipSpawnDelay;
        private float enemyAttackStartTime;
        private float enemyNextWaveTime;

        private string introductionText;
        private PortraitID introductionPortrait;

        private List<string> enemyMessages;
        private List<PortraitID> enemyPortraits;

        private List<string> attackStartText;
        private List<PortraitID> attackStartPortraits;

        private List<string> afterAttackMessages;
        private List<PortraitID> afterAttackPortraits;

        private List<String> timedMessages;
        private List<PortraitID> timedMessagePortraits;

        private List<string> levels;

        private float shipToDefendHP;
        private float shipToDefendMaxHP;

        private int messageTimer;
        private bool started;
        public bool Started { get { return started; } }
        private List<float> timedMessageTimes;
        private int timedMessageCount;

        private bool autofollow;
        private bool showInventoryTutorial = true;

        private bool levelCompleted;
        private int lastLevelCompletedIndex = -1;
        private int levelCompletedIndex;
        private int levelIndex = 0;

        private bool skip;

        public EscortObjective(Game1 game, Mission mission, List<String> descriptions,
            EscortDataCapsule escortDataCapsule, bool autofollow = false) :
            base(game, mission, descriptions[0])
        {
            descriptions.RemoveAt(0);
            this.descriptions = descriptions;
            this.autofollow = autofollow;

            Setup(escortDataCapsule);
        }

        public EscortObjective(Game1 game, Mission mission, String description,
            EscortDataCapsule escortDataCapsule, EventTextCapsule eventTextCapsule) :
            base(game, mission, description)
        {
            Setup(escortDataCapsule);

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        private void Setup(EscortDataCapsule escortDataCapsule)
        {
            this.escortDataCapsule = escortDataCapsule;
            startingNumberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            numberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            enemyShipSpawnDelay = escortDataCapsule.EnemyAttackFrequency;
            levels = escortDataCapsule.Levels;
            shipToDefendMaxHP = escortDataCapsule.ShipToDefendHP;
            shipToDefendHP = escortDataCapsule.ShipToDefendHP;
            enemies = new List<OverworldShip>();
            timedMessageCount = -1;
            timedMessageTimes = new List<float>();

            for (int i = 0; i < escortDataCapsule.EnemyShips.Count; i++)
            {
                enemies.Add(escortDataCapsule.EnemyShips[i]);
            }
        }

        public override void OnActivate()
        {
            if (escortDataCapsule.ShipToDefend is FreighterShip)
            {
                FreighterShip ship = (FreighterShip)escortDataCapsule.ShipToDefend;

                ship.Direction.SetDirection(new Vector2(ship.destination.X - ship.position.X,
                    ship.destination.Y - ship.position.Y));

                game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    ship, escortDataCapsule.StartingPoint);

                ship.Wait();
            }

            OverworldShip.FollowPlayer = false;

            base.OnActivate();
        }

        public override void Reset()
        {
            base.Reset();

            started = false;
            startingNumberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            numberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            enemies = new List<OverworldShip>();

            for (int i = 0; i < levels.Count; i++)
            {
                game.stateManager.shooterState.GetLevel(levels[i]).Initialize();
            }

            for (int i = 0; i < escortDataCapsule.EnemyShips.Count; i++)
            {
                escortDataCapsule.EnemyShips[i].Initialize();
                enemies.Add(escortDataCapsule.EnemyShips[i]);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (autofollow && started)
            {
                if (!escortDataCapsule.ShipToDefend.HasArrived)
                {
                    game.player.Direction.SetDirection(new Vector2(
                        escortDataCapsule.ShipToDefend.destination.X - game.player.position.X,
                        escortDataCapsule.ShipToDefend.destination.Y - game.player.position.Y));
                }

                game.player.speed = escortDataCapsule.ShipToDefend.speed;

                if (ControlManager.CheckPress(RebindableKeys.Pause)
                    && GameStateManager.currentState.Equals("OverworldState"))
                {
                    PopupHandler.DisplaySelectionMenu("Do you want to skip travel?",
                        new List<string>() { "Yes", "No" },
                        new List<System.Action>(){
                            delegate 
                            {
                                SkipForward();
                            },
                            delegate {}
                        });
                }
            }

            if (timedMessageCount >= 0
                && timedMessageCount < timedMessages.Count
                && StatsManager.PlayTime.HasOverworldTimePassed(timedMessageTimes[timedMessageCount]))
            {
                if (timedMessagePortraits[timedMessageCount] != PortraitID.None)
                {
                    PopupHandler.DisplayRealtimePortraitMessage(3000, new [] {timedMessagePortraits[timedMessageCount]},
                        new List<int>(), timedMessages[timedMessageCount]);
                }
                else
                {
                    PopupHandler.DisplayRealtimeMessage(3000, timedMessages[timedMessageCount]);
                }
                timedMessageCount++;
            }

            // Player talks to freighter to begin escort
            if (!started
                && GameStateManager.currentState.Equals("OverworldState")
                && CollisionDetection.IsRectInRect(game.player.Bounds, escortDataCapsule.ShipToDefend.Bounds)
                && ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter))))
            {
                if (showInventoryTutorial
                    && ShipInventoryManager.equippedShield is EmptyShield)
                {
                    PopupHandler.DisplayPortraitMessage(introductionPortrait, "[Alliance Pilot] \"You need to equip a shield before we leave. Go to the shop next to Highfence and I will tell you what to do.");
                    game.tutorialManager.EnableEquipTutorial();
                }

                else
                {
                    if (introductionPortrait != PortraitID.None)
                    {
                        PopupHandler.DisplayPortraitMessage(introductionPortrait, introductionText);
                    }
                    else
                    {
                        PopupHandler.DisplayMessage(introductionText);
                    }

                    ((FreighterShip)escortDataCapsule.ShipToDefend).Start();
                    escortDataCapsule.ShipToDefend.speed = escortDataCapsule.FreighterSpeed;

                    started = true;

                    if (autofollow)
                    {
                        game.player.DisableControls();
                    }

                    this.Description = descriptions[0];

                    enemyAttackStartTime = StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.EnemyAttackStartTime);

                    for (int i = 0; i < timedMessages.Count; i++)
                    {
                        timedMessageTimes.Add(StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.TimedMessageTriggers[i]));
                    }
                    timedMessageCount = 0;
                }
            }

            // Escort mission begins
            if (started 
                && GameStateManager.currentState.Equals("OverworldState") &&
                numberOfEnemyShips > 0 &&
                StatsManager.PlayTime.HasOverworldTimePassed(enemyAttackStartTime))
            {
                // Ready to spawn a new enemy ship
                if (StatsManager.PlayTime.HasOverworldTimePassed(enemyNextWaveTime))
                {
                    int i = startingNumberOfEnemyShips - numberOfEnemyShips;

                    if (descriptions.Count > 0)
                    {
                        if (descriptions.Count > 1)
                        {
                            descriptions.RemoveAt(0);
                        }

                        this.Description = descriptions[0];
                    }

                    if (attackStartPortraits[i] != PortraitID.None)
                    {
                        PopupHandler.DisplayPortraitMessage(attackStartPortraits[i], attackStartText[i]);
                    }
                    else
                    {
                        PopupHandler.DisplayMessage(attackStartText[i]);
                    }

                    game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        enemies[0],
                        escortDataCapsule.ShipToDefend.position +
                        (650 * escortDataCapsule.ShipToDefend.Direction.GetDirectionAsVector()),
                        levels[levelIndex], escortDataCapsule.ShipToDefend);

                    numberOfEnemyShips--;

                    if (numberOfEnemyShips > 0)
                    {
                        enemyNextWaveTime = StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.EnemyAttackFrequency);
                    }
                }
            }
            
            // Transfers freigter hp between levels
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("ShooterState")
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).IsObjectiveCompleted)
                {
                    shipToDefendHP = game.stateManager.shooterState.CurrentLevel.GetFreighterHP();

                    levelCompleted = true;
                    levelCompletedIndex = i;
                    levelIndex = i + 1;
                }
            }

            if (GameStateManager.currentState.Equals("OverworldState"))
            {
                if (levelCompleted
                && levelCompletedIndex > lastLevelCompletedIndex)
                {
                    levelCompleted = false;
                    lastLevelCompletedIndex = levelCompletedIndex;

                    if (afterAttackPortraits[levelCompletedIndex] != PortraitID.None)
                    {
                        PopupHandler.DisplayPortraitMessage(afterAttackPortraits[levelCompletedIndex],
                            afterAttackMessages[levelCompletedIndex]);
                    }
                    else
                    {
                        PopupHandler.DisplayMessage(afterAttackMessages[levelCompletedIndex]);
                    }
                }
                else
                {
                    levelCompleted = false;
                }
            }

            Collision();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!started 
                && GameStateManager.currentState.Equals("OverworldState") 
                && CollisionDetection.IsRectInRect(game.player.Bounds, escortDataCapsule.ShipToDefend.Bounds))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, escortDataCapsule.ShipToDefend.Bounds);
                game.helper.DisplayText("Press 'Enter' to talk to freighter captain..");
            }
        }

        public override bool Completed()
        {
            return (escortDataCapsule.ShipToDefend.AIManager.Finished 
                || escortDataCapsule.ShipToDefend.IsDead || skip);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
            game.player.EnableControls();
            OverworldShip.FollowPlayer = true;

            game.stateManager.GotoStationSubScreen("Soelara Station", "Overview");
        }

        public override bool Failed()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("OverworldState")
                    && game.stateManager.shooterState.CurrentLevel != null
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).GetFreighterHP() != -1
                    && game.stateManager.shooterState.GetLevel(levels[i]).GetFreighterHP() <= 0)
                {
                    return true;
                }

                else if (GameStateManager.currentState.Equals("OverworldState")
                    && game.stateManager.shooterState.CurrentLevel != null 
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.CurrentLevel.IsObjectiveFailed)
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();

            game.player.EnableControls();
            OverworldShip.FollowPlayer = true;

            game.stateManager.overworldState.RemoveOverworldObject(escortDataCapsule.ShipToDefend);
        }

        private void Collision()
        {
            messageTimer--;

            if (numberOfEnemyShips < startingNumberOfEnemyShips)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (CollisionDetection.IsRectInRect(escortDataCapsule.ShipToDefend.Bounds, enemies[i].Bounds))
                    {
                        game.stateManager.overworldState.RemoveOverworldObject(enemies[i]);
                        StartEnemyLevel(i, enemies[i].Level);
                    }
                }
            }
        }

        public void SetIntroductionMessage(String message, PortraitID portrait = PortraitID.None)
        {
            introductionText = message;
            introductionPortrait = portrait;
        }

        public void SetEnemyMessage(List<PortraitID> portraits, params string[] messages)
        {
            enemyMessages = new List<string>();
            enemyPortraits = new List<PortraitID>();

            for (int i = 0; i < messages.Length; i++ )
            {
                if (portraits != null)
                {
                    enemyPortraits.Add(portraits[i]);
                }
                else
                {
                    enemyPortraits.Add(PortraitID.None);
                }
                enemyMessages.Add(messages[i]);
            }
        }

        public void SetAttackStartText(List<PortraitID> portraits, params string[] messages)
        {
            attackStartText = new List<string>();
            attackStartPortraits = new List<PortraitID>();

            for (int i = 0; i < messages.Length; i++)
            {
                if (portraits != null)
                {
                    attackStartPortraits.Add(portraits[i]);
                }
                else
                {
                    attackStartPortraits.Add(PortraitID.None);
                }

                attackStartText.Add(messages[i]);
            }
        }

        public void SetAfterAttackMessages(List<PortraitID> portraits, params string[] messages)
        {
            afterAttackMessages = new List<string>();
            afterAttackPortraits = new List<PortraitID>();

            for (int i = 0; i < messages.Length; i++)
            {
                if (portraits != null)
                {
                    afterAttackPortraits.Add(portraits[i]);
                }
                else
                {
                    afterAttackPortraits.Add(PortraitID.None);
                }

                afterAttackMessages.Add(messages[i]);
            }
        }

        public void SetTimedMessages(List<PortraitID> portraits, params string[] messages)
        {
            timedMessages = new List<string>();
            timedMessagePortraits = new List<PortraitID>();

            for (int i = 0; i < messages.Length; i++)
            {
                if (portraits != null)
                {
                    timedMessagePortraits.Add(portraits[i]);
                }
                else
                {
                    timedMessagePortraits.Add(PortraitID.None);
                }

                timedMessages.Add(messages[i]);
            }
        }

        private void SkipForward()
        {
            PopupHandler.SkipRealTimeMessages();
            timedMessageCount = timedMessages.Count;

            foreach (OverworldShip ship in enemies)
            {
                if (game.stateManager.overworldState.GetAllOverworldGameObjects.Contains(ship))
                {
                    game.stateManager.overworldState.RemoveOverworldObject(ship);
                }
            }

            if (enemies.Count > 0)
            {
                numberOfEnemyShips--;
                StartEnemyLevel(0, levels[levelIndex]);
            }
            else
            {
                escortDataCapsule.ShipToDefend.HasArrived = true;
                skip = true;
            }
        }

        private void StartEnemyLevel(int enemyIndex, string level)
        {
            game.stateManager.shooterState.BeginLevel(level);
            game.stateManager.shooterState.CurrentLevel.SetFreighterMaxHP(shipToDefendMaxHP);
            game.stateManager.shooterState.CurrentLevel.SetFreighterHP(shipToDefendMaxHP);
            enemies.RemoveAt(enemyIndex);

            if (enemyMessages.Count > 0)
            {
                if (enemyPortraits[0] != PortraitID.None)
                {
                    PopupHandler.DisplayPortraitMessage(enemyPortraits[0], enemyMessages[0]);
                }
                else
                {
                    PopupHandler.DisplayMessage(enemyMessages[0]);
                }
                enemyMessages.RemoveAt(0);
                enemyPortraits.RemoveAt(0);
            }
        }
    }
}
