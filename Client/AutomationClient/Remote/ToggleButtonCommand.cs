﻿//  ----------------------------------------------------------------------
//  <copyright file="ToggleButtonCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ToggleButtonCommand : AutomationElementCommandBase
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement(false);

            if (element == null)
            {
                SendNotFoundResult(string.Format("ToggleButtonCommand: Could not find the element : {0}",
                                                 AutomationIdentifier.ToIdOrName()));
                return;
            }

            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);

            if (peer == null)
            {
                SendNotFoundResult("Couldn't find automation peer.");
            }

            var pattern = peer.GetPattern(PatternInterface.Toggle) as IToggleProvider;
            if (pattern == null)
            {
                SendNotFoundResult();
            }

            pattern.Toggle();

            var buttonBase = element as ButtonBase;

            // Execute the bound command, if available, because IToggleProvider doesn't trigger that.
            if (buttonBase != null && buttonBase.Command != null)
            {
                buttonBase.Command.Execute(buttonBase.CommandParameter);
            }

            SendSuccessResult();
        }
    }
}