using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MissionMenuState : MenuState
    {
        private readonly Vector2 PortraitOffset = new Vector2(19, 19);

        private readonly Vector2 PortraitOverlaySize = new Vector2(567, 234);
        private readonly Vector2 ResponseOverlaySize = new Vector2(571, 309);
        private readonly Vector2 SelectionOverlaySize = new Vector2(567, 234);

        private Portrait portrait;
        private Vector2 PortraitPosition 
        {
            get
            {
                if (BaseState.OverlayType == OverlayType.Response) 
                {
                    return new Vector2(Game1.ScreenSize.X / 2 - ResponseOverlaySize.X / 2,
                        Game1.ScreenSize.Y / 2 - ResponseOverlaySize.Y / 2) + PortraitOffset;
                }
                else
                {
                    return new Vector2(Game1.ScreenSize.X / 2 - PortraitOverlaySize.X / 2,
                        Game1.ScreenSize.Y / 2 - PortraitOverlaySize.Y / 2) + PortraitOffset;
                }
            }
        }

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
            availableMissions = new List<Mission>();

            missionCursor = new Cursor(this.Game, SpriteSheet, new Rectangle(48, 263, 14, 14), new Rectangle(48, 277, 14, 14));
            missionCursor.Initialize();

            missionCursorIndex = -1;
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

                for (int i = 0; i < availableMissions.Count + 1; i++)
                {
                    String text = i != availableMissions.Count ? availableMissions[i].MissionName : "Back";

                    if (ControlManager.IsMouseOverText(FontManager.GetFontStatic(14), text,
                        new Vector2(Game1.ScreenSize.X / 2,
                            Game1.ScreenSize.Y / 2 - 40 + (20 * (i + (i == availableMissions.Count ? 1 : 0))) 
                            + FindTextBoxWithText(text).TextBoxRect.Height)))
                    {
                        if (ControlManager.IsMouseMoving())
                        {
                            missionCursorIndex = i;
                        }
                    }
                }

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
                    if (ControlManager.IsMouseOverArea(new Rectangle(
                        BaseStateManager.ResponseRectangles[0].X - (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.PosResponse).X / 2,
                        BaseStateManager.ResponseRectangles[0].Y + BaseStateManager.ResponseRectangles[0].Height / 2,
                        (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.PosResponse).X,
                        (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.PosResponse).Y)))
                    {
                        responseCursorIndex = 0;
                    }

                    else if (ControlManager.IsMouseOverArea(new Rectangle(
                        BaseStateManager.ResponseRectangles[1].X - (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.NegResponse).X / 2,
                        BaseStateManager.ResponseRectangles[1].Y + BaseStateManager.ResponseRectangles[1].Height / 2,
                        (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.NegResponse).X,
                        (int)FontManager.GetFontStatic(14).MeasureString(selectedMission.NegResponse).Y)))
                    {
                        responseCursorIndex = 1;
                    }

                    if (responseCursorIndex > 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = 1;
                }

                else
                {
                    for (int i = 0; i < MissionManager.MissionResponseBuffer.Count; i++)
                    {
                        if (ControlManager.IsMouseOverArea(new Rectangle(
                            BaseStateManager.ResponseRectangles[i].X - (int)FontManager.GetFontStatic(14).MeasureString(MissionManager.MissionResponseBuffer[i]).X / 2,
                            BaseStateManager.ResponseRectangles[i].Y + BaseStateManager.ResponseRectangles[i].Height / 2,
                            (int)FontManager.GetFontStatic(14).MeasureString(MissionManager.MissionResponseBuffer[i]).X,
                            (int)FontManager.GetFontStatic(14).MeasureString(MissionManager.MissionResponseBuffer[i]).Y)))
                        {
                            responseCursorIndex = i;
                        }
                    }

                    if (responseCursorIndex > MissionManager.MissionResponseBuffer.Count - 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = MissionManager.MissionResponseBuffer.Count - 1;
                }

            }

            if (BaseStateManager.ButtonControl != ButtonControl.Response
                && ControlManager.CheckPress(RebindableKeys.Pause))
            {
                SkipMissionText();
            }
            else
            {
                base.Update(gameTime);
            }
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
                    missionCursor.position.X = textBoxes[textBoxes.Count - 1].TextBoxRect.X - Game.fontManager.GetFont(16).MeasureString("Back").X / 2 - 10;
                    missionCursor.position.Y = textBoxes[textBoxes.Count - 1].TextBoxRect.Y + 13;
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
            //Actions for pressing Ok-key in "SELECTMISSION STATE" 
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
            {
                BaseState.HideOverlay();

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
                if (BaseStateManager.HasTextBoxesFinishedScrolling())
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
                            BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexY];
                            BaseState.HideOverlay();
                            DisplayAvailableMissions(availableMissions);

                            SelectMission();
                        }
                    }
                }
                else
                {
                    FlushText();
                }
            }

            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Confirm))
            {
                if (TextFinishedScrolling())
                {
                    BaseState.HideOverlay();

                    if (selectedMission != null)
                    {
                        if (selectedMission.AcceptIndex + 1 < selectedMission.AcceptText.Count<string>())
                        {
                            selectedMission.AcceptIndex++;
                            DisplayMissionAcceptText();
                        }
                        else
                        {
                            BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexY];
                            BaseStateManager.ChangeMenuSubState("Overview");
                        }
                    }

                    else
                    {
                        BaseStateManager.ChangeMenuSubState("Overview");
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexY];
                    }
                }
                else
                {
                    FlushText();
                }
            }
        }

        public override void CursorActions()
        { 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            missionCursor.Draw(spriteBatch);

            if (portrait != null
                && BaseStateManager.ButtonControl != ButtonControl.Mission)
            {
                // Draws portrait
                spriteBatch.Draw(portrait.Sprite.Texture, PortraitPosition,
                    portrait.Sprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        public void MissionEvent()
        {
            BaseStateManager.TextBoxes.Clear();

            if (MissionManager.MissionEventBuffer.Count > 0)
            {
                SetPortraitFromText(MissionManager.MissionEventBuffer[0]);

                SetTextRectangle();

                if (MissionManager.MissionResponseBuffer.Count > 0)
                {
                    tempRect = BaseStateManager.ResponseTextRectangle;
                    BaseState.DisplayOverlay(OverlayType.Response);
                }
                
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  tempRect,
                                                                  false, true,
                                                                  MissionManager.MissionEventBuffer[0]));

                MissionManager.MissionEventBuffer.RemoveAt(0);

                BaseStateManager.ButtonControl = ButtonControl.Confirm;

                if (MissionManager.MissionResponseBuffer.Count > 0)
                {
                    for (int i = 0; i < MissionManager.MissionResponseBuffer.Count; i++)
                    {
                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              BaseStateManager.ResponseRectangles[i],
                                                              true, false,
                                                              TextUtils.WordWrap(BaseState.Game.fontManager.GetFont(14),
                                                                                 MissionManager.MissionResponseBuffer[i],
                                                                                 Game1.ScreenSize.X * 2 / 3)
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

            tempRect = BaseStateManager.ResponseTextRectangle;
            BaseState.DisplayOverlay(OverlayType.Response);

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              tempRect,
                                                              false, true,
                                                              MissionManager.MissionStartBuffer[0]));

            MissionManager.MissionStartBuffer.Remove(MissionManager.MissionStartBuffer[0]);

            if (MissionManager.MissionStartBuffer.Count > 0)
                BaseStateManager.ButtonControl = ButtonControl.Confirm;

            if (MissionManager.MissionStartBuffer.Count == 0)
            {

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle1,
                                                      true, false,
                                                      SelectedMission.PosResponse));

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle2,
                                                      true, false,
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
                BaseStateManager.ButtonControl = ButtonControl.Mission;
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
                tempRect = BaseStateManager.ResponseTextRectangle;
                BaseState.DisplayOverlay(OverlayType.Response);
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  tempRect,
                                                                  false, true,
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
                                                      true, false,
                                                      SelectedMission.PosResponse));

                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                          BaseStateManager.ResponseRectangle2,
                                                          true, false,
                                                          SelectedMission.NegResponse));

                    BaseStateManager.ButtonControl = ButtonControl.Response;
                    ResponseCursorIndex = 0;
                }
            }

        }

        public void DisplayAvailableMissions(List<Mission> availableMissions)
        {
            int selectionCount;

            BaseState.DisplayOverlay(OverlayType.MissionSelection);
            BaseStateManager.TextBoxes.Clear();
            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game1.ScreenSize.X / 2),
                              (Game1.ScreenSize.Y / 2) - (int)SelectionOverlaySize.Y / 2 + 20,
                               Game1.ScreenSize.X - 20, 10),
                               true, false, "Available Missions:" + "\n\n"));

            if (availableMissions.Count > 0)
            {
                for (int i = 0; i < availableMissions.Count; i++)
                {
                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                              new Rectangle((Game1.ScreenSize.X / 2),
                                           Game1.ScreenSize.Y / 2 - 40 + 20 * availableMissions.IndexOf(availableMissions[i]) + 1,
                                           Game1.ScreenSize.X - 20,
                              10),
                              true, false,
                              availableMissions[i].MissionName));
                }

                selectionCount = availableMissions.Count;
            }

            else
            {
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game1.ScreenSize.X / 2),
                        ((Game1.ScreenSize.Y / 2) - 40),
                        Game1.ScreenSize.X - 20,
                        10),
                        true, false,
                        "<None>"));

                selectionCount = 1;
            }

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                    new Rectangle((Game1.ScreenSize.X / 2),
                                 ((Game1.ScreenSize.Y / 2) - 40) + 20 * (selectionCount + 1),
                                   Game1.ScreenSize.X / 2, 10),
                    true, false,
                    "Back"));
        }

        public void DisplayMissionAcceptText()
        {
            if (SelectedMission.AcceptText[0].ToLower().Equals("empty"))
            {
                MissionManager.MarkMissionAsActive(selectedMission.MissionID);
                BaseStateManager.ChangeMenuSubState("Overview");
                BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexY];
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
                        tempRect, false, true, temp[0]));

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
                        tempRect, false, true, temp[0]));

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
            }
        }

        public void DisplayMissionAcceptFailedText()
        {
            BaseStateManager.TextBoxes.Clear();

            SetPortraitFromText(selectedMission.AcceptFailedText);

            SetTextRectangle();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              tempRect,
                                                              false, true,
                                                              selectedMission.AcceptFailedText));

            missionCursorIndex = 0;

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }

        public void DisplayMissionCompletedText()
        {
            if (MissionManager.MissionEventBuffer.Count <= 0)
            {
                List<String> temp = new List<String>();

                Game.SaveOnEnterOverworld = true;

                BaseStateManager.TextBoxes.Clear();

                List<Mission> completedMissions = MissionManager.ReturnCompletedMissions(BaseState.GetBase().name);
                    
                foreach (String str in completedMissions[0].CompletedText.Split('#')) {
                    temp.Add(str);
                }

                SetPortraitFromText(temp[0]);
                SetTextRectangle();

                if (HasReward(completedMissions[0]))
                {
                    if (HasRewardItem(completedMissions[0]))
                    {
                        if (ShipInventoryManager.HasAvailableSlot())
                        {
                            temp.Add(GetRewardText(completedMissions[0]));
                        }

                        else
                        {
                            temp.Add(GetNoEmptySlotText(completedMissions[0]));
                        }
                    }
                    else
                    {
                        temp.Add(GetRewardText(completedMissions[0]));
                    }
                }

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                              tempRect,
                              false, true,
                              temp[0]));

                MissionManager.MarkCompletedMissionAsDead(completedMissions[0].MissionID);

                BaseStateManager.ButtonControl = ButtonControl.Confirm;

                for (int i = 1; i < temp.Count; i++)
                {
                    MissionManager.MissionEventBuffer.Add(temp[i]);
                }
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
                                                              false, true,
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
                BaseState.DisplayOverlay(OverlayType.Portrait);
            }
            else
            {
                tempRect = BaseStateManager.NormalTextRectangle;
                BaseState.DisplayOverlay(OverlayType.Text);
            }
        }

        private bool TextFinishedScrolling()
        {
            foreach (TextBox txtbox in BaseStateManager.TextBoxes)
            {
                if (txtbox.Scrolling
                    && txtbox.FinishedScrolling)
                {
                    return true;
                }
            }

            return false;
        }

        private void FlushText()
        {
            foreach (TextBox txtbox in BaseStateManager.TextBoxes)
            {
                if (txtbox.Scrolling)
                {
                    txtbox.FlushText();
                }
            }
        }

        private TextBox FindTextBoxWithText(string text)
        {
            foreach (TextBox tb in BaseStateManager.TextBoxes)
            {
                if (tb.GetText().ToLower().Equals(text.ToLower()))
                    return tb;
            }

            return null;
        }

        private void SkipMissionText()
        {
            MissionManager.MissionEventBuffer.Clear();
            BaseState.HideOverlay();
            BaseStateManager.ChangeMenuSubState("Overview");
            BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexY];
        }

        private String GetRewardText(Mission completedMission)
        {
            List<Item> rewardItems = completedMission.RewardItems;

            if (rewardItems.Count > 0)
            {
                StringBuilder rewardText = new StringBuilder("");

                rewardText.Append("Your reward is: \n");
                if (completedMission.MoneyReward > 0)
                {
                    rewardText.Append("\n" + completedMission.MoneyReward.ToString() + " Crebits");
                }
                foreach (Item reward in rewardItems)
                {
                    rewardText.Append("\n" + reward.Name);
                }

                return rewardText.ToString();
            }

            else if (completedMission.MoneyReward > 0)
            {
                return "Your reward is: \n" + completedMission.MoneyReward +
                    " Crebits";
            }

            return "NO REWARD";
        }
        
        private String GetNoEmptySlotText(Mission completedMission)
        {
            int rewardItemValue = 0;

            foreach (Item reward in completedMission.RewardItems)
            {
                rewardItemValue += (int)Math.Round(reward.Value * 0.8f, 0);
            }

            return String.Format("You do not have room for your reward! You will recieve its value in Crebits instead.\n\nYour reward is: \n{0} Crebits",
                completedMission.MoneyReward + rewardItemValue);
        }

        private bool HasRewardItem(Mission completedMission)
        {
            return completedMission.RewardItems.Count > 0;
        }

        private bool HasReward(Mission completedMission)
        {
            return completedMission.RewardItems.Count > 0 || completedMission.MoneyReward > 0;
        }
    }
}
