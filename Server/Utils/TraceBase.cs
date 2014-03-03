//  ----------------------------------------------------------------------
//  <copyright file="TraceBase.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Linq;

namespace WindowsPhoneTestFramework.Server.Utils
{
    public class TraceBase : ITrace
    {
        public event EventHandler<SimpleMessageEventArgs> Trace;

        protected void InvokeTrace(string message, params object[] args)
        {
            string msg;
            try
            {
                msg = args.Length>0 ? string.Format(message, args) : message;
            }
            catch (FormatException)
            {
                msg = message + String.Format("Error formatting message: Args='{0}",
                           String.Join(",", args.Select(a => (a == null) ? "null" : a.ToString()))); ;
            }
            InvokeTrace(new SimpleMessageEventArgs { Message = msg });
        }

        protected void InvokeTrace(SimpleMessageEventArgs e)
        {
            EventHandler<SimpleMessageEventArgs> handler = Trace;
            if (handler != null) handler(this, e);
        }
    }
}