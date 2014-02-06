//  ----------------------------------------------------------------------
//  <copyright file="HookDefinitions.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

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
#if DEBUG
            StepFlowOutputHelpers.Write(StepFlowOutputHelpers.WriteType.Warning,"Not disposing emulator because test is running in Debug mode. Usually causes next test to fail!");
#else
            StepFlowOutputHelpers.Write(StepFlowOutputHelpers.WriteType.Trace, "Disposing of emulator");
            DisposeOfEmu();
#endif
        }
    }
}