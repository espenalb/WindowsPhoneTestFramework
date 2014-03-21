//  ----------------------------------------------------------------------
//  <copyright file="StepFlowContextHelpers.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Test.EmuSteps
{
    public static class StepFlowContextHelpers
    {
        private const string EmuShotPrefix = "ScreenShot_";
        private const string EmuControllerKey = "Emu.AutomationController";
        private const string EmuPictureIndexKey = "Emu.PictureIndex";
        private const string EmuRandomGeneratorKey = "Emu.RandomGenerator";

        public static IAutomationController GetEmuAutomationController(ScenarioContext context,
                                                                       IConfiguration configuration)
        {
            Assert.That(context != null);
            Assert.That(configuration != null);

            lock (context)
            {
                IAutomationController emu = null;
                if (context.TryGetValue(EmuControllerKey, out emu))
                    return emu;

                emu = Server.Core.Loader.LoadFrom(configuration.AutomationControllerName);
                emu.Trace += (sender, args) => StepFlowOutputHelpers.Write(args.Message);
                emu.Start(
                    configuration.ControllerInitialisationString,
                    configuration.AutomationIdentification);

                context[EmuControllerKey] = emu;
                return emu;
            }
        }

        public static string GetNextPictureName()
        {
            return GetNextPictureName(FeatureContext.Current, ScenarioContext.Current);
        }

        public static string GetNextPictureName(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            return GetNextSupportingInfoFileName(featureContext, scenarioContext, EmuShotPrefix, "png");
        }

        public static string GetNextSupportingInfoFileName(FeatureContext featureContext,
                                                           ScenarioContext scenarioContext,
                                                           string prefix, string extension)
        {
            Assert.NotNull(featureContext);
            Assert.NotNull(scenarioContext);

            lock (featureContext)
                lock (scenarioContext)
                {
                    object objectPictureIndex;
                    if (!scenarioContext.TryGetValue(EmuPictureIndexKey, out objectPictureIndex))
                        objectPictureIndex = 0;
                    var pictureIndex = (int)objectPictureIndex;
                    scenarioContext[EmuPictureIndexKey] = ++pictureIndex;

                    var fileName = String.Format("{0}{1}_{2}_{3}.{4}",
                                                 prefix,
                                                 featureContext.FeatureInfo.Title.MaxLength(20),
                                                 scenarioContext.ScenarioInfo.Title.MaxLength(40),
                                                 pictureIndex,
                                                 extension);

                    foreach (var ch in Path.GetInvalidFileNameChars())
                        fileName = fileName.Replace(ch, '_');

                    return fileName;
                }
        }

        public static string MaxLength(this string text, int length)
        {
            if (String.IsNullOrEmpty(text) || text.Length<length)
                return text;
            return text.Substring(0, length);
        }
        public static Random GetRandom()
        {
            return GetRandom(FeatureContext.Current, ScenarioContext.Current);
        }

        public static Random GetRandom(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            Assert.NotNull(featureContext);
            Assert.NotNull(scenarioContext);

            object randomObject;
            if (featureContext.TryGetValue(EmuRandomGeneratorKey, out randomObject))
            {
                if (randomObject is Random)
                    return randomObject as Random;
            }

            var random = new Random();
            featureContext[EmuRandomGeneratorKey] = random;
            return random;
        }

        public static void DisposeOfEmu(ScenarioContext context)
        {
            Assert.That(context != null);

            lock (context)
            {
                IAutomationController emu = null;
                if (!context.TryGetValue(EmuControllerKey, out emu))
                    return;

                if (emu.DisplayInputController != null)
                    emu.DisplayInputController.ReleaseWindowFromForeground();

                emu.Dispose();
                context.Remove(EmuControllerKey);
            }
        }
    }
}