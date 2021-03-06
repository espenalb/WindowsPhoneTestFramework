﻿//  ----------------------------------------------------------------------
//  <copyright file="SetApplicationSettingsCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.IO.IsolatedStorage;
using System.Linq;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SetApplicationSettingsCommand
    {
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

        protected override void DoImpl()
        {
            try
            {
                // Remove any keys that would be duplicates before starting.
                foreach (var key in this.Settings.Keys.Where(key => AppSettings.Contains(key)))
                {
                    AppSettings.Remove(key);
                }

                foreach (var pair in this.Settings)
                {
                    AppSettings.Add(pair.Key, pair.Value);
                }

                AppSettings.Save();
                AutomationClient.RaiseApplicationSettingsChanged(Settings);
                SendSuccessResult();
            }
            catch (Exception ex)
            {
                SendExceptionFailedResult(ex);
            }
        }
    }
}