﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRtcMeBindingsBlazor.Interops;
using WebRtcMeBindingsBlazor.Extensions;
using WebRTCme;

namespace WebRtcMeBindingsBlazor.Api
{
    internal class RTCDTMFSender : ApiBase, IRTCDTMFSender
    {

        internal static IRTCDTMFSender Create(IJSRuntime jsRuntime, JsObjectRef jsObjectRefDTMFSender) => 
            new RTCDTMFSender(jsRuntime, jsObjectRefDTMFSender);

        private RTCDTMFSender(IJSRuntime jsRuntime, JsObjectRef jsObjectRef) : base(jsRuntime, jsObjectRef) 
        {
            AddNativeEventListener("tonechange", (s, e) => OnToneChange?.Invoke(s, e));
        }

        public string ToneBuffer => GetNativeProperty<string>("toneBuffer");

        public event EventHandler OnToneChange;

        public void InsertDTMF(string tones, ulong duration = 100, ulong interToneGap = 70) =>
            JsRuntime.CallJsMethodVoid(NativeObject, "insertDTMF", tones, duration, interToneGap);
    }
}