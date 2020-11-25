﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRTCme;

//// TODO: REFACTOR TO WebRTCme.Xamarin
namespace WebRtcGuiXamarin
{
    /// <summary>
    //// TODO: RENAME TO WebRtcXamarin
    /// </summary>
    public static class WebRtcGui
    {
        internal static IWebRtc WebRtc { get; private set; }


        public static void Initialize()
        {
            WebRtc = CrossWebRtc.Current;
        }

        public static void Cleanup()
        {
            WebRtc.Cleanup();
        }
    }
}
