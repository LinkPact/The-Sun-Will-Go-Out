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

        private RebelShip rebel1;
        private RebelShip rebel2;

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

            rebels = new List<RebelShip>();

            rebels.Add(rebel1);
            rebels.Add(rebel2);

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
