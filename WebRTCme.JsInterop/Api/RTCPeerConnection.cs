﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRtcJsInterop.Extensions;
using WebRtcJsInterop.Interops;
using WebRTCme;

namespace WebRtcJsInterop.Api
{
    internal class RTCPeerConnection : BaseApi, IRTCPeerConnection
    {
        private RTCConfiguration _rtcConfiguration;

        private RTCPeerConnection(IJSRuntime jsRuntime, JsObjectRef jsObjectRef, RTCConfiguration rtcConfiguration) 
            : base(jsRuntime, jsObjectRef) 
        {
            _rtcConfiguration = rtcConfiguration;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeBaseAsync();
        }

        internal static async Task<IRTCPeerConnection> New(IJSRuntime jsRuntime, RTCConfiguration rtcConfiguration)
        {
            var jsObjectRef = await jsRuntime.CreateJsObject(null, "RTCPeerConnection", new object());
            var rtcPeerConnection = new RTCPeerConnection(jsRuntime, jsObjectRef, rtcConfiguration);
            return rtcPeerConnection;
        }
    }
}
