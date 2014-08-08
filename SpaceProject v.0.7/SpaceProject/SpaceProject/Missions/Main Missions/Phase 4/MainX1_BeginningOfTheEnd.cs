using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX1_BeginningOfTheEnd : Mission
    {
        private enum EventID
        {
            Introduction,
            HighfenceEntryDenial,
            StalkersShotDown,
            TalkAtHighfence1,
            TalkAtHighfence2,
            HighfenceResponse1,
            HighfenceResponse2,
            TalkAtHighfence3,
            TalkAtSoelara,
            TalkAtFortun,
            ActionAtTelmun
        }
        private List<RebelShip> rebels;

        // Wave 1
        private RebelShip rebel1;
        private RebelShip rebel2;

        // Wave 2
        private float rebel3SpawnTime;
        private float rebel4SpawnTime;
        private float rebel5SpawnTime;

        private RebelShip rebel3;
        private RebelShip rebel4;
        private RebelShip rebel5;

        private AllyShip ally1;
        private AllyShip ally2;
        private AllyShip ally3;

        public MainX1_BeginningOfTheEnd(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Planet highfence = Game.stateManager.overworldState.GetPlanet("Highfence");
            Station soelara = Game.stateManager.overworldState.GetStation("Soelara Station");
            Station fortrun = Game.stateManager.overworldState.GetStation("Fotrun Station I");
            Planet telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            rebel1 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel1.Initialize();
            rebel1.SetPosition(MathFunctions.SpreadPos(highfence.position, 200));
            rebel1.Level = "PirateLevel1";
            rebel1.EncounterMessage = "Hello! 1";

            rebel2 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel2.Initialize();
            rebel2.SetPosition(MathFunctions.SpreadPos(highfence.position, 200));
            rebel2.Level = "PirateLevel2";
            rebel2.EncounterMessage = "Hello! 2";

            rebel3 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel3.Initialize();
            rebel3.Level = "PirateLevel1";
            rebel3.EncounterMessage = "Hello! 3";

            rebel4 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel4.Initialize();
            rebel4.Level = "PirateLevel2";
            rebel4.EncounterMessage = "Hello! 4";

            rebel5 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel5.Initialize();
            rebel5.Level = "PirateLevel3";
            rebel5.EncounterMessage = "Hello! 5";

            ally1 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally1.Initialize(Game.stateManager.overworldState.GetSectorX);

            ally2 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally2.Initialize(Game.stateManager.overworldState.GetSectorX);

            ally3 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally3.Initialize(Game.stateManager.overworldState.GetSectorX);

            rebels = new List<RebelShip>();

            rebels.Add(rebel1);
            rebels.Add(rebel2);
            rebels.Add(rebel3);
            rebels.Add(rebel4);
            rebels.Add(rebel5);

            // OBJECTIVES
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], highfence,
                new EventTextCapsule(GetEvent((int)EventID.StalkersShotDown), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate 
                {
                    if ((((ControlManager.CheckPress(RebindableKeys.Action1) 
                        || ControlManager.CheckKeypress(Keys.Enter)) 
                        && !Game.player.HyperspeedOn
                        && GameStateManager.currentState.Equals("OverworldState"))) 
                        && CollisionDetection.IsRectInRect(Game.player.Bounds, highfence.Bounds))
                    {
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.HighfenceEntryDenial).Text);
                    }
                }, 
                delegate 
                {
                    return ((rebel1.IsDead && rebel2.IsDead) 
                        && GameStateManager.currentState.Equals("OverworldState"));
                },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], highfence, 
                new EventTextCapsule(GetEvent((int)EventID.TalkAtHighfence1), null, EventTextCanvas.BaseState)));

            objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[0], highfence,
                new ResponseTextCapsule(GetEvent((int)EventID.TalkAtHighfence2), GetAllResponses((int)EventID.TalkAtHighfence2),
                    new List<System.Action>() { 
                        delegate 
                        { 
                            missionHelper.ShowEvent(GetEvent((int)EventID.HighfenceResponse1));
                        },
                        delegate 
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.HighfenceResponse2));
                        } 
                    }),
                    new EventTextCapsule(GetEvent((int)EventID.TalkAtHighfence3), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], soelara,
                new EventTextCapsule(new EventText("Objective completed"), new EventText("Objective Failed"), EventTextCanvas.MessageBox),
                delegate
                {
                    rebel3SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(2000);
                    rebel4SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(7000);
                    rebel5SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(13000);

                    ally1.SetPosition(MathFunctions.SpreadPos(Game.player.position, 100));
                    ally1.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally1);

                    ally2.SetPosition(MathFunctions.SpreadPos(Game.player.position, 100));
                    ally2.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally2);

                    ally3.SetPosition(MathFunctions.SpreadPos(Game.player.position, 100));
                    ally3.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally3);
                    
                },
                delegate
                {
                    if (rebel3SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel3SpawnTime))
                    {
                        rebel3.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                        Game.stateManager.overworldState.AddOverworldObject(rebel3);
                        rebel3SpawnTime = -1;
                    }

                    else if (rebel4SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel4SpawnTime))
                    {
                        rebel4.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                        Game.stateManager.overworldState.AddOverworldObject(rebel4);
                        rebel4SpawnTime = -1;
                    }

                    else if (rebel5SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel5SpawnTime))
                    {
                        rebel5.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                        Game.stateManager.overworldState.AddOverworldObject(rebel5);
                        rebel5SpawnTime = -1;
                    }
                        
                },
                delegate
                {
                    return false;
                },
                delegate
                {
                    return false;
                }));
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));

            Game.stateManager.overworldState.AddOverworldObject(rebel1);
            Game.stateManager.overworldState.AddOverworldObject(rebel2);
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            base.OnReset();

            ObjectiveIndex = 0;

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Reset();
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            foreach (RebelShip rebel in rebels)
            {
                if (!rebel.IsDead
                    && CollisionDetection.IsRectInRect(Game.player.Bounds, rebel.Bounds))
                {
                    Game.stateManager.overworldState.RemoveOverworldObject(rebel);
                    Game.messageBox.DisplayMessage(rebel.EncounterMessage);
                    Game.stateManager.shooterState.BeginLevel(rebel.Level);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }
    }
}
