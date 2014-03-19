﻿//  ----------------------------------------------------------------------
//  <copyright file="HookDefinitions.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Configuration;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.HookDefinitions
{
    [Binding]
    public class EmuHookDefinitions : EmuDefinitionBase
    {
        /*
        public EmuHookDefinitions(IConfiguration configuration)
            : base(configuration)
        {                
        }
        */

        [AfterScenario]
        public void AfterAnyScenarioMakeSureEmuIsDisposed()
        {
            var message = "Disposing of emulator";
            if (Equals("true", ConfigurationManager.AppSettings["KeepEmulatorRunningAfterScenario"]))
            {
                message = "Releasing emulator";
            }

            StepFlowOutputHelpers.Write(StepFlowOutputHelpers.WriteType.Trace, message);
            DisposeOfEmu();
        }
    }
}