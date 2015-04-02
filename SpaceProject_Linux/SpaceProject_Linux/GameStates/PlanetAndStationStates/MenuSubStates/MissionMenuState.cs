using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class MissionMenuState : MenuState
    {
        private readonly float PortraitWidth = 149;
        private readonly float PortraitHeight = 192;

        private Portrait portrait;
        private Vector2 portraitPosition;
        private Vector2 portraitBorderPosition;
        private float portraitOffset;

        private Rectangle tempRect;

        #region Mission Fields

        private List<Mission> availableMissions;

        private Mission activeMission;
        private Mission selectedMission;
        private Cursor missionCursor;

        private int missionCursorIndex;
        private int responseCursorIndex;

        #endregion

        #region Properties

        public List<Mission> AvailableMissions { get { return availableMissions; } }
        public Mission ActiveMission { get { return activeMission; } set { activeMission = value; } }

        public Mission SelectedMission { get { return selectedMission; } set { selectedMission = value; } }
        public Cursor MissionCursor { get { return missionCursor; } set { missionCursor = value; } }

        public int MissionCursorIndex { get { return missionCursorIndex; } set { missionCursorIndex = value; } }
        public int ResponseCursorIndex { get { return responseCursorIndex; } set { responseCursorIndex = value; } }

        #endregion

        public MissionMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
        }

        public override void Initialize()
        {

            #region Initialize Mission Fields

            availableMissions = new List<Mission>();

            missionCursor = new Cursor(this.Game, SpriteSheet, new Rectangle(201, 121, 14, 14), new Rectangle(201, 135, 14, 14));
            missionCursor.Initialize();

            missionCursorIndex = -1;

            #endregion

            confirmString = "Press 'Enter' to continue..";

            confirmStringPos = new Vector2((Game.ScreenSize.X * 2 / 3),
                                            Game.ScreenSize.Y - BaseState.Game.fontManager.GetFont(14).MeasureString(confirmString).Y - 10);

            confirmStringOrigin = BaseState.Game.fontManager.GetFont(14).MeasureString(confirmString) / 2;

            portraitOffset = (Game.ScreenSize.X / 2 - Game.ScreenSize.X / 3 - PortraitWidth) / 2;
            portraitPosition = new Vector2(Game.ScreenSize.X / 3 + portraitOffset, 
                Game.ScreenSize.Y / 2 + portraitOffset);
            portraitBorderPosition = new Vector2(Game.ScreenSize.X / 3, Game.ScreenSize.Y / 2);
        }

        public override void OnEnter()
        {
            availableMissions = MissionManager.ReturnAvailableMissions(BaseState.GetBase().name);
            BaseStateManager.OverviewMenuState.ButtonMission.isSelected = true;
        }

        public override void OnLeave()
        {
            BaseStateManager.OverviewMenuState.ButtonMission.isSelected = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (BaseState.GetBase() != null)
                availableMissions = MissionManager.ReturnAvailableMissions(BaseState.GetBase().name);

            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission) ||
                BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                missionCursor.isActive = true;
                missionCursor.isVisible = true;
            }

            else
            {
                missionCursor.isActive = false;
                missionCursor.isVisible = false;
            }


            if (BaseStateManager.ButtonControl == ButtonControl.Mission)
            {
                //Moves button cursor right when pressing up. 
                if (ControlManager.CheckPress(RebindableKeys.Up))
                    missionCursorIndex--;

                //Moves button cursor left when pressing down
                else if (ControlManager.CheckPress(RebindableKeys.Down))
                    missionCursorIndex++;

                if (MissionCursorIndex > availableMissions.Count)
                    missionCursorIndex = 0;

                else if (MissionCursorIndex < 0)
                    missionCursorIndex = availableMissions.Count;

                if (availableMissions.Count > 0 && missionCursorIndex != availableMissions.Count)
                {
                    selectedMission = availableMissions[MissionCursorIndex];
                }
            }

            else if (BaseStateManager.ButtonControl == ButtonControl.Response)
            {
                //Moves button cursor right when pressing up. 
                if (ControlManager.CheckPress(RebindableKeys.Up))
                    responseCursorIndex--;

                //Moves button cursor left when pressing down
                else if (ControlManager.CheckPress(RebindableKeys.Down))
                    responseCursorIndex++;

                if (MissionManager.MissionResponseBuffer.Count <= 0)
                {
                    if (responseCursorIndex > 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = 1;
                }

                else
                {
                    if (responseCursorIndex > MissionManager.MissionResponseBuffer.Count - 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = MissionManager.MissionResponseBuffer.Count - 1;
                }

            }

            base.Update(gameTime);
        }

        public void UpdateTextCursorPos()
        {
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
            {
                List<TextBox> textBoxes = BaseStateManager.TextBoxes;

                for (int i = 0; i < textBoxes.Count; i++)
                {
                    if (textBoxes[i].GetText().EndsWith(".."))
                    {
                        textBoxes.Remove(textBoxes[i]);
                    }
                }

                if (missionCursorIndex != availableMissions.Count)
                {
                    missionCursor.position.X = textBoxes[missionCursorIndex + 1].TextBoxRect.X - Game.fontManager.GetFont(16).MeasureString(availableMissions[missionCursorIndex].MissionName).X / 2 - 10;
                    missionCursor.position.Y = textBoxes[missionCursorIndex + 1].TextBoxRect.Y + 13;
                }
                else
                {
                    missionCursor.position.X = textBoxes[missionCursorIndex + 1].TextBoxRect.X - Game.fontManager.GetFont(16).MeasureString("Back").X / 2 - 10;
                    missionCursor.position.Y = textBoxes[missionCursorIndex + 1].TextBoxRect.Y + 13;
                }
            }

            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                if (MissionManager.MissionResponseBuffer.Count <= 0)
                {
                    switch (responseCursorIndex)
                    {
                        case 0:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(selectedMission.PosResponse).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle1.Y + 10;
                                break;
                            }

                        case 1:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(selectedMission.NegResponse).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle2.Y + 10;
                                break;
                            }
                    }
                }

                else
                {
                    switch (responseCursorIndex)
                    {
                        case 0:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[0]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle1.Y + 10;
                                break;
                            }

                        case 1:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle2.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[1]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle2.Y + 10;
                                break;
                            }

                        case 2:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle3.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[2]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle3.Y + 10;
                                break;
                            }

                        case 3:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle4.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[3]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle4.Y + 10;
                                break;
                            }
                    }
                }
            }
        }

        public override void ButtonActions()
        {
            TextToSpeech.Stop();

            //Actions for pressing Ok-key in "SELECTMISSION STATE" 
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
            {
                if (missionCursorIndex == availableMissions.Count)
                {
                    BaseStateManager.TextBoxes.Clear();
                    BaseStateManager.ChangeMenuSubState("Overview");
                }

                else
                {
                    DisplayMissionIntroduction();
                }
            }

            //Actions for pressing Ok-key in "MISSION STATE" 
            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                activeMission = MissionManager.GetActiveMission(BaseState.GetBase().name);

                if (MissionManager.MissionResponseBuffer.Count > 0)
                {
                    if (activeMission != null)
                    {
                        activeMission.MissionResponse = responseCursorIndex + 1;
                        activeMission.CurrentObjective.Update(StatsManager.PlayTime);
                        activeMission.MissionResponse = 0;
                        MissionEvent();
                    }
                }

                else
                {
                    if (responseCursorIndex == 0)
                    {
                        DisplayMissionAcceptText();
                    }

                    else if (responseCursorIndex == 1)
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        DisplayAvailableMissions(availableMissions);

                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                              new Rectangle((Game.ScreenSize.X * 2 / 3),
                                           ((Game.ScreenSize.Y / 2) + 40) + 20 * (availableMissions.Count + 1),
                                             Game.ScreenSize.X - 20,
                              10),
                              true,
                              "Back"));

                        //if (selectedMission == MissionManager.GetMission("Main - Tutorial Mission")
                        //    && MissionManager.GetMission("Main - New First Mission").MissionState == StateOfMission.Unavailable)
                        //{
                        //    MissionManager.UnlockMission("Main - New First Mission");
                        //    MissionManager.MarkMissionAsActive("Main - New First Mission");
                        //    MissionEvent();
                        //}
                        //
                        //else
                        //{
                        SelectMission();
                        //}
                    }
                }
            }

            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Confirm))
            {
                if (selectedMission != null)
                {
                    if (selectedMission.AcceptIndex + 1 < selectedMission.AcceptText.Count<string>())
                    {
                        selectedMission.AcceptIndex++;
                        DisplayMissionAcceptText();
                    }
                    else
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        BaseStateManager.ChangeMenuSubState("Overview");
                    }
                }

                else
                {
                    BaseStateManager.ChangeMenuSubState("Overview");
                    BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                }
            }
        }

        public override void CursorActions()
        { 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            missionCursor.Draw(spriteBatch);

            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Confirm))
            {
                //Draw confirm string
                spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                       confirmString,
                                       confirmStringPos + Game.fontManager.FontOffset,
                                       Game.fontManager.FontColor,
                                       .0f,
                                       confirmStringOrigin,
                                       1.0f,
                                       SpriteEffects.None,
                                       .75f);
            }

            if (portrait != null
                && BaseStateManager.ButtonControl != ButtonControl.Mission)
            {
                // Draws portrait border
                spriteBatch.Draw(SpriteSheet.Texture, portraitBorderPosition, new Rectangle(203, 152, 1, 1), Color.White,
                    0f, Vector2.Zero, new Vector2(portraitOffset * 2 + PortraitWidth, portraitOffset * 2 + PortraitHeight), SpriteEffects.None, 1f);

                // Draws portrait
                spriteBatch.Draw(portrait.Sprite.Texture, portraitPosition,
                    portrait.Sprite.SourceRectangle, Color.White);
            }
        }

        public void MissionEvent()
        {
            BaseStateManager.TextBoxes.Clear();

            if (MissionManager.MissionEventBuffer.Count > 0)
            {
                SetPortraitFromText(MissionManager.MissionEventBuffer[0]);

                SetTextRectangle();

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  tempRect,
                                                                  false,
                                                                  MissionManager.MissionEventBuffer[0]));



                TextToSpeech.Speak(MissionManager.MissionEventBuffer[0]);

                MissionManager.MissionEventBuffer.Remove(MissionManager.MissionEventBuffer[0]);

                if (MissionManager.MissionEventBuffer.Count > 0)
                    BaseStateManager.ButtonControl = ButtonControl.Confirm;

                else if (MissionManager.MissionResponseBuffer.Count <= 0)
                    BaseStateManager.ButtonControl = ButtonControl.Confirm;

                else
                {
                    for (int i = 0; i < MissionManager.MissionResponseBuffer.Count; i++)
                    {
                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              BaseStateManager.ResponseRectangles[i],
                                                              true,
                                                              TextUtils.WordWrap(BaseState.Game.fontManager.GetFont(14),
                                                                                 MissionManager.MissionResponseBuffer[i],
                                                                                 Game.ScreenSize.X * 2 / 3)
                                                              ));

                    }

                    BaseStateManager.ButtonControl = ButtonControl.Response;
                }
            }
        }

        public void DisplayMissionStartBufferText()
        {
            BaseStateManager.TextBoxes.Clear();

            SetPortraitFromText(MissionManager.MissionStartBuffer[0]);

            SetTextRectangle();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              tempRect,
                                                              false,
                                                              MissionManager.MissionStartBuffer[0]));

            TextToSpeech.Speak(MissionManager.MissionStartBuffer[0]);

            MissionManager.MissionStartBuffer.Remove(MissionManager.MissionStartBuffer[0]);

            if (MissionManager.MissionStartBuffer.Count > 0)
                BaseStateManager.ButtonControl = ButtonControl.Confirm;

            if (MissionManager.MissionStartBuffer.Count == 0)
            {

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle1,
                                                      true,
                                                      SelectedMission.PosResponse));

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle2,
                                                      true,
                                                      SelectedMission.NegResponse));

                BaseStateManager.ButtonControl = ButtonControl.Response;
                ResponseCursorIndex = 0;
            }
        }

        public void SelectMission()
        {
            MissionCursorIndex = 0;

            if (availableMissions.Count <= 0)
            {
                BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                BaseStateManager.ChangeMenuSubState("Overview");
            }

            else
            {
                if (missionCursorIndex != availableMissions.Count)
                {
                    SelectedMission = availableMissions[MissionCursorIndex];
                    BaseStateManager.ButtonControl = ButtonControl.Mission;
                }
            }
            BaseStateManager.ActiveMenuState.CursorActions();
        }

        public void DisplayMissionIntroduction()
        {
            BaseStateManager.TextBoxes.Clear();

            if (SelectedMission != null)
            {
                String[] temp = SelectedMission.IntroductionText.Split('#');

                SetPortraitFromText(temp[0]);

                SetTextRectangle();

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  tempRect,
                                                                  false,
                                                                  temp[0]));

                BaseStateManager.ButtonControl = ButtonControl.Response;
                ResponseCursorIndex = 0;

                if (temp.Length > 1)
                {
                    for (int i = temp.Length - 1; i > 0; i--)
                    {
                        MissionManager.MissionStartBuffer.Insert(0, temp[i]);
                        BaseStateManager.ButtonControl = ButtonControl.Confirm;
                    }
                }

                else
                {
                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle1,
                                                      true,
                                                      SelectedMission.PosResponse));

                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                          BaseStateManager.ResponseRectangle2,
                                                          true,
                                                          SelectedMission.NegResponse));

                    BaseStateManager.ButtonControl = ButtonControl.Response;
                    ResponseCursorIndex = 0;
                }

                TextToSpeech.Speak(temp[0]);
            }

        }

        public void DisplayAvailableMissions(List<Mission> availableMissions)
        {
            BaseStateManager.TextBoxes.Clear();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game.ScreenSize.X * 2 / 3),
                                                                       (Game.ScreenSize.Y / 2) + 10,
                                                                        Game.ScreenSize.X - 20,
                                                                        10),
                                                                        true,
                                                                        "Available Missions:" + "\n\n"));
            if (availableMissions.Count > 0)
            {
                for (int i = 0; i < availableMissions.Count; i++)
                {
                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                                                                      new Rectangle((Game.ScreenSize.X * 2 / 3),
                                                                                   ((Game.ScreenSize.Y / 2) + 40) + 20 * availableMissions.IndexOf(availableMissions[i]) + 1,
                                                                                     Game.ScreenSize.X - 20,
                                                                      10),
                                                                      true,
                                                                      availableMissions[i].MissionName));
                }
            }

            else
            {
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game.ScreenSize.X * 2 / 3),
                                                            ((Game.ScreenSize.Y / 2) + 60),
                                                            Game.ScreenSize.X - 20,
                                                            10),
                                                            true,
                                                            "<None>"));
            }
        }

        public void DisplayMissionAcceptText()
        {
            if (SelectedMission.AcceptText[0].ToLower().Equals("empty"))
            {
                MissionManager.MarkMissionAsActive(selectedMission.MissionID);
                BaseStateManager.ChangeMenuSubState("Overview");
                BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                selectedMission.IntroductionText += "/ok";
                
                return;
            }
            if (selectedMission.RequiresAvailableSlot)
            {
                if (ShipInventoryManager.HasAvailableSlot())
                {
                    String[] temp = SelectedMission.AcceptText[selectedMission.AcceptIndex].Split('#');

                    SetPortraitFromText(temp[0]);

                    BaseStateManager.TextBoxes.Clear();

                    MissionManager.MarkMissionAsActive(selectedMission.MissionID);

                    SetTextRectangle();

                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                        tempRect, false, temp[0]));

                    if (temp.Length > 1)
                    {
                        for (int i = temp.Length - 1; i > 0; i--)
                        {
                            MissionManager.MissionEventBuffer.Insert(0, temp[i]);
                        }
                    }

                    missionCursorIndex = 0;

                    BaseStateManager.ButtonControl = ButtonControl.Confirm;

                    selectedMission.IntroductionText += "/ok";

                    TextToSpeech.Speak(temp[0]);
                }

                else
                {
                    DisplayMissionAcceptFailedText();
                }
            }

            else
            {
                String[] temp = SelectedMission.AcceptText[selectedMission.AcceptIndex].Split('#');

                SetPortraitFromText(temp[0]);

                BaseStateManager.TextBoxes.Clear();

                MissionManager.MarkMissionAsActive(selectedMission.MissionID);

                SetTextRectangle();

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                        tempRect, false, temp[0]));

                if (temp.Length > 1)
                {
                    for (int i = temp.Length - 1; i > 0; i--)
                    {
                        MissionManager.MissionEventBuffer.Insert(0, temp[i]);
                    }
                }

                missionCursorIndex = 0;

                BaseStateManager.ButtonControl = ButtonControl.Confirm;

                selectedMission.IntroductionText += "/ok";

                TextToSpeech.Speak(temp[0]);
            }
        }

        public void DisplayMissionAcceptFailedText()
        {
            BaseStateManager.TextBoxes.Clear();

            SetPortraitFromText(selectedMission.AcceptFailedText);

            SetTextRectangle();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              tempRect,
                                                              false,
                                                              selectedMission.AcceptFailedText));

            TextToSpeech.Speak(selectedMission.AcceptFailedText);

            missionCursorIndex = 0;

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }

        public void DisplayMissionCompletedText()
        {
            if (MissionManager.MissionEventBuffer.Count <= 0)
            {
                Game.SaveOnEnterOverworld = true;

                BaseStateManager.TextBoxes.Clear();

                List<Mission> completedMissions = MissionManager.ReturnCompletedMissions(BaseState.GetBase().name);

                String[] temp = completedMissions[0].CompletedText.Split('#');

                SetPortraitFromText(temp[0]);

                SetTextRectangle();

                if (temp.Length > 1)
                {
                    List<Item> rewardItems = completedMissions[0].RewardItems;

                    if (rewardItems.Count > 0)
                    {
                        string rewardText = "";

                        foreach (Item reward in rewardItems)
                        {
                            rewardText += "\n" + reward.Name;
                        }

                        temp[temp.Length - 1] += "\n\n" + "Your reward is: \n" + completedMissions[0].MoneyReward +
                            " Rupees" + rewardText;
                    }

                    else
                    {
                        temp[temp.Length - 1] += "\n\n" + "Your reward is: \n" + completedMissions[0].MoneyReward +
                            " Rupees";
                    }

                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                          tempRect,
                                                                          false,
                                                                          temp[0]));

                    for (int i = 1; i < temp.Length; i++)
                    {
                        MissionManager.MissionEventBuffer.Add(temp[i]);
                    }

                    MissionManager.MarkCompletedMissionAsDead(completedMissions[0].MissionID);

                    BaseStateManager.ButtonControl = ButtonControl.Confirm;
                }

                else
                {

                    List<Item> rewardItems = completedMissions[0].RewardItems;

                    if (rewardItems.Count > 0)
                    {
                        string rewardText = "";

                        foreach (Item reward in rewardItems)
                        {
                            rewardText += "\n" + reward.Name;
                        }

                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                          tempRect,
                                                                          false,
                                                                          temp[0] +
                                                                          "\n\n" +
                                                                          "Your reward is: \n" +
                                                                          completedMissions[0].MoneyReward + " Rupees" +
                                                                          rewardText));
                    }

                    else
                    {
                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                          tempRect,
                                                                          false,
                                                                          temp[0] +
                                                                          "\n\n" +
                                                                          "Your reward is: \n" +
                                                                          completedMissions[0].MoneyReward + " Rupees"));
                    }

                    MissionManager.MarkCompletedMissionAsDead(completedMissions[0].MissionID);

                    BaseStateManager.ButtonControl = ButtonControl.Confirm;
                }

                TextToSpeech.Speak(temp[0]);
            }
        }

        public void DisplayMissionFailedText()
        {
            BaseStateManager.TextBoxes.Clear();

            List<Mission> failedMissions = MissionManager.ReturnFailedMissions(BaseState.GetBase().name);

            String[] temp = failedMissions[0].FailedText.Split('#');

            SetPortraitFromText(temp[0]);

            SetTextRectangle();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              tempRect,
                                                              false,
                                                              temp[0]));

            if (temp.Length > 1)
            {
                for (int i = temp.Length - 1; i > 0; i--)
                {
                    MissionManager.MissionEventBuffer.Insert(0, temp[i]);
                }
            }

            MissionManager.MarkFailedMissionAsDead(failedMissions[0].MissionID);

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }

        private void SetPortraitFromText(String text)
        {
            PortraitID id = Portrait.GetPortraitIDFromString(text);
            if (id != PortraitID.None)
            {
                portrait = new Portrait(id);
            }
            else
            {
                portrait = null;
            }
        }

        private void SetTextRectangle()
        {
            if (portrait != null)
            {
                tempRect = BaseStateManager.PortraitTextRectangle;
            }
            else
            {
                tempRect = BaseStateManager.NormalTextRectangle;
            }
        }
    }
}
