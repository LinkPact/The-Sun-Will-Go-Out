using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class EditAmountAction : SettingAmountAction
    {
        private int min;
        private int max;

        public EditAmountAction(int min, int max)
            : base()
        {
            this.min = min;
            this.max = max;
        }

        protected override void ActionLogic(EventSetting eventSetting)
        {
            int newSetting = GetNewInt("Enter new setting value:", "Event settings", eventSetting.GetValue().ToString());

            if (newSetting != -1)
            {
                // Limits value to min/max value when present
                if (min != -1)
                {
                    if (newSetting < min)
                        newSetting = min;
                }
                if (max != -1)
                {
                    if (newSetting > max)
                        newSetting = max;
                }

                eventSetting.SetValue(newSetting);
            }
        }
    }
}
