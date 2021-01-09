﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebRTCme;
using WebRTCme.Middleware;

//// TODO: TESTING FOR NOW, MOVE ALL WEBRTC CODE to Middleware
namespace WebRTCme.DemoApp.Blazor.Wasm.Pages
{
    partial class VideoMeeting : IDisposable
    {
        [Inject]
        IJSRuntime JsRuntime { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        [Inject]
        ILogger<VideoMeeting> Logger { get; set; }

        private string _server;
        private string _room;
        private string _userId;

        private IWebRtcMiddleware _webRtcMiddleware;
        private IRoomService _roomService;

        private RoomParameters _roomParameters = new RoomParameters();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _webRtcMiddleware = CrossWebRtcMiddleware.Current;
            //webRtcMiddleware.Initialize(Configuration["SignallingServer:BaseUrl"]);
            _roomService = await _webRtcMiddleware.CreateRoomServiceAsync(Configuration["SignallingServer:BaseUrl"], JsRuntime);
        }

        private async void HandleValidSubmit()
        {
            await _roomService.ConnectRoomAsync(_roomParameters);
        }

        public void Dispose()
        {
            //// TODO: How to call async in Dispose??? Currently fire and forget!!!
            ///
            Task.Run(async () => await _roomService.DisposeAsync());
            _webRtcMiddleware.Dispose();
            //WebRtcMiddleware.Cleanup();
        }
    }
}

