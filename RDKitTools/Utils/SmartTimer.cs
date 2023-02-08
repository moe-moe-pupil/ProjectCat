// --------------------------------------------------------------------------------------------------------------
// <copyright file="SmartTimer.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDKitTools.Utils
{
    /// <summary lang='Zh-CN'>
    /// 中心计时器.
    /// </summary>
    /// <typeparam name="T">计时器内部数组类.</typeparam>
    public class SmartTimer<T>
        where T : TimerAction, ITimerAction, new()
    {
        private static readonly List<T> s_actions = new ();

        public void AddAction(float startTime, string name)
        {
            s_actions.Add(new T { StartTime = startTime, Name = name });
        }

        public static void RemAction(string actionName) //Removes all actions with <actionName> from the list
        {
            foreach (Action action in new List<Action>(actions))
            {
                if (action.name.Equals(actionName)) actions.Remove(action);
            }
        }

        public static void ClearActions() //Clears all actions
        {
            actions.Clear();
        }

        public static bool ContainsAction(string actionName) //Checks if list contains action with <actionName>
        {
            foreach (Action action in new List<Action>(actions))
            {
                if (action.name.Equals(actionName)) return true;
            }

            return false;
        }

        public static void ResetAction(string actionName) //Resets the time of all actions with <actionName>
        {
            foreach (Action action in new List<Action>(actions))
            {
                if (action.name.Equals(actionName)) action.time = action.startTime;
            }
        }

        public static void ResetAllActions() //Resets the time of all actions
        {
            foreach (Action action in new List<Action>(actions))
            {
                action.time = action.startTime;
            }
        }

        public static void Update(float delta, bool removeTimeout = true) //Updates the timer, removes timeout actions if <removeTimeout> is true
        {
            foreach (Action action in new List<Action>(actions))
            {
                action.time -= delta;
                if (removeTimeout && action.time <= 0) actions.Remove(action);
            }
        }
    }
}
