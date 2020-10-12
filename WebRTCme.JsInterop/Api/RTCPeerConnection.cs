﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRtcJsInterop.Extensions;
using WebRtcJsInterop.Interops;
using WebRTCme;
using WebRTCme.Models;

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


        async ValueTask<IAsyncDisposable> IRTCPeerConnection.OnIceCandidate(
            Func<RTCPeerConnectionIceEvent, ValueTask> callback)
        {
            return await JsRuntime.AddEventListener(JsObjectRef, null, "onicecandidate",
                JsEventHandler.New<RTCPeerConnectionIceEvent>(async e => 
                { 
                    await callback.Invoke(e).ConfigureAwait(false); 
                },
                null, false)).ConfigureAwait(false);
        }

        internal static async Task<IRTCPeerConnection> New(IJSRuntime jsRuntime, RTCConfiguration rtcConfiguration)
        {
            var jsObjectRef = await jsRuntime.CreateJsObject("window", "RTCPeerConnection", new object());
            var rtcPeerConnection = new RTCPeerConnection(jsRuntime, jsObjectRef, rtcConfiguration);
            return rtcPeerConnection;
        }

    }
}
