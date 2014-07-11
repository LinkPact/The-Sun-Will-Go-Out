using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main3_TheAlliance : Mission
    {
        private int tempTimer = 0;

        private bool died;
        private int tempTimer2;

        private Vector2 savedPos = Vector2.Zero;

        public Main3_TheAlliance(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getStation("Fotrun Station II"),
                new EventTextCapsule(
                    new EventText("You fight off the last pirates. You better enter the station and see what damage they caused."),
                    null,
                    EventTextCanvas.MessageBox),
                null,
                delegate
                {
                    if (CollisionDetection.IsPointInsideCircle(Game.player.position,
                    Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                    400) && savedPos == Vector2.Zero)
                    {
                        savedPos = Game.player.position;
                    }

                    else if (!CollisionDetection.IsPointInsideCircle(Game.player.position,
                        Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                        400))
                    {
                        savedPos = Vector2.Zero;
                    }

                    if (GameStateManager.currentState == "OverworldState" 
                        && CollisionDetection.IsPointInsideCircle(Game.player.position,
                        Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                        300))
                    {
                        Game.messageBox.DisplayMessage(EventList[0].Key.Text);
                        missionHelper.StartLevel("Main_TheAlliancelvl");
                    }

                    if (!died
                        && missionHelper.IsLevelFailed("Main_TheAlliancelvl"))
                    {
                        died = true;
                        tempTimer2 = 5;

                        if (savedPos != Vector2.Zero)
                        {
                            Game.player.position = savedPos;
                        }

                        savedPos = Vector2.Zero;
                        Game.player.speed = 0;
                    }

                    if (died && GameStateManager.currentState.Equals("OverworldState"))
                    {
                        tempTimer2--;

                        if (tempTimer2 < 0)
                        {
                            died = false;

                            Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                            Game.messageBox.DisplayMessage(EventList[6].Key.Text);
                            ObjectiveIndex = 1;
                            progress = 1;
                        }
                    }
                },
                delegate
                {
                    return missionHelper.IsLevelCompleted("Main_TheAlliancelvl");
                },
                delegate
                {
                    return false;
                }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.getStation("Fotrun Station II"),
                new EventTextCapsule(GetEvent(0), null, EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
                Game.stateManager.overworldState.getStation("Fotrun Station I"),
                new EventTextCapsule(GetEvent(1), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[3],
                Game.stateManager.overworldState.getPlanet("Highfence"),
                new EventTextCapsule(GetEvent(6), null, EventTextCanvas.BaseState),
                null,
                delegate
                {
                    if (GameStateManager.currentState == "OverworldState")
                    {
                        tempTimer = 100;
                    }

                    if (tempTimer == 1)
                    {
                        Game.messageBox.DisplayMessage(EventList[2].Key.Text);
                    }
                },
                delegate
                {
                    return missionHelper.IsPlayerOnPlanet("Highfence");
                },
                delegate
                {
                    return false;
                }));

            objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[4],
                Game.stateManager.overworldState.getPlanet("Highfence"),
                new ResponseTextCapsule(GetEvent(7), GetAllResponses(7),
                    new List<System.Action> 
                    {
                        delegate 
                        {
                            missionHelper.ShowEvent(EventList[8].Key);
                        },
                        delegate 
                        {
                            missionHelper.ShowEvent(EventList[9].Key);
                        }
                    })));
        }

        public override void StartMission() { }

        public override void OnLoad() { }

        public override void OnReset()
        {
            base.OnReset();
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
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
