using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX2_ContinuationOfTheEnd : Mission
    {
        private enum EventID
        {
            GoToNewNorrland,
            ArriveAtNewNorrland,
            ArriveAtFotrun,
            Question1AtFotrun,
            Question2AtFotrun,
            Question3AtFotrun,
            RightAnswerQuestion3,
            WrongAnswer
        }
        private List<RebelShip> rebels;

        // Wave 2
        private float rebel3SpawnTime;
        private float rebel4SpawnTime;
        private float rebel5SpawnTime;

        private RebelShip rebel1;
        private RebelShip rebel2;
        private RebelShip rebel3;

        private AllyShip ally1;
        private AllyShip ally2;
        private AllyShip ally3;

        public MainX2_ContinuationOfTheEnd(Game1 Game, string section, Sprite spriteSheet) :
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
            Planet newNorrland = Game.stateManager.overworldState.GetPlanet("New Norrland");

            rebel1 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel1.Initialize();
            rebel1.Level = "PirateLevel1";
            rebel1.EncounterMessage = "Hello! 3";

            rebel2 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel2.Initialize();
            rebel2.Level = "PirateLevel2";
            rebel2.EncounterMessage = "Hello! 4";

            rebel3 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel3.Initialize();
            rebel3.Level = "PirateLevel3";
            rebel3.EncounterMessage = "Hello! 5";

            ally1 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally1.Initialize(Game.stateManager.overworldState.GetSectorX);

            ally2 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally2.Initialize(Game.stateManager.overworldState.GetSectorX);

            ally3 = new AllyShip(Game, Game.spriteSheetVerticalShooter, ShipType.Alliance);
            ally3.Initialize(Game.stateManager.overworldState.GetSectorX);

            rebels = new List<RebelShip>();

            rebels.Add(rebel1);
            rebels.Add(rebel2);
            rebels.Add(rebel1);
            rebels.Add(rebel2);
            rebels.Add(rebel3);

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtNewNorrland), new EventText("Objective Failed"), EventTextCanvas.BaseState),
                delegate
                {
                    rebel3SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(7000);
                    rebel4SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(14000);
                    rebel5SpawnTime = StatsManager.PlayTime.GetFutureOverworldTime(21000);

                    ally1.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                    ally1.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally1);

                    ally2.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                    ally2.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally2);

                    ally3.SetPosition(MathFunctions.SpreadPos(Game.player.position, 400));
                    ally3.SetTarget(Game.player);
                    Game.stateManager.overworldState.AddOverworldObject(ally3);
                },
                delegate
                {
                    if (rebel3SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel3SpawnTime))
                    {
                        rebel1.SetPosition(MathFunctions.SpreadPos(Game.player.position, 600));
                        Game.stateManager.overworldState.AddOverworldObject(rebel1);
                        rebel3SpawnTime = -1;
                    }

                    else if (rebel4SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel4SpawnTime))
                    {
                        rebel2.SetPosition(MathFunctions.SpreadPos(Game.player.position, 600));
                        Game.stateManager.overworldState.AddOverworldObject(rebel2);
                        rebel4SpawnTime = -1;
                    }

                    else if (rebel5SpawnTime > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(rebel5SpawnTime))
                    {
                        rebel3.SetPosition(MathFunctions.SpreadPos(Game.player.position, 600));
                        Game.stateManager.overworldState.AddOverworldObject(rebel3);
                        rebel5SpawnTime = -1;
                    }

                    if (GameStateManager.currentState.Equals("OverworldState")
                        && !GetEvent((int)EventID.GoToNewNorrland).Displayed)
                    {
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.GoToNewNorrland).Text);
                        GetEvent((int)EventID.GoToNewNorrland).Displayed = true;
                    }
                        
                },
                delegate
                {
                    return missionHelper.IsPlayerOnPlanet("New Norrland");
                },
                delegate
                {
                    return false;
                }));

            Objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], fortrun, 
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtFotrun), null, EventTextCanvas.BaseState)));

            Objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[0], fortrun,
                new ResponseTextCapsule(GetEvent((int)EventID.Question1AtFotrun), GetAllResponses((int)EventID.Question1AtFotrun),
                    new List<System.Action>() 
                    {
                        delegate
                        {
                            missionHelper.ClearResponseText();
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        }
                    })));

            Objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[0], fortrun,
                new ResponseTextCapsule(GetEvent((int)EventID.Question2AtFotrun), GetAllResponses((int)EventID.Question2AtFotrun),
                    new List<System.Action>() 
                    {
                        delegate
                        {
                            missionHelper.ClearResponseText();
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        }
                    })));

            Objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[0], fortrun,
                new ResponseTextCapsule(GetEvent((int)EventID.Question3AtFotrun), GetAllResponses((int)EventID.Question3AtFotrun),
                    new List<System.Action>() 
                    {
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.RightAnswerQuestion3), true);
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        },
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent((int)EventID.WrongAnswer));
                            missionHelper.StartLevelAfterCondition("CoverBlown", LevelStartCondition.TextCleared);
                        }
                    })));
        }

        public override void StartMission()
        {
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
