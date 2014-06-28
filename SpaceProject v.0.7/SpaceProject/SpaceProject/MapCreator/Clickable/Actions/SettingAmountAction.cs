using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    abstract class SettingAmountAction : Action
    {
        public Boolean PerformAction(EventSetting setting)
        {
            if (!isActionPerformed)
            {
                ActionLogic(setting);
                isActionPerformed = true;
                return true;
            }
            return false;
        }

        protected abstract void ActionLogic(EventSetting setting);
    }
}
