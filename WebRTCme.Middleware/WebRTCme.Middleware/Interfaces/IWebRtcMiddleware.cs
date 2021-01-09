﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebRTCme.Middleware
{
    public interface IWebRtcMiddleware : IDisposable
    {
        Task<IRoomService> CreateRoomServiceAsync(string signallingServerBaseUrl, IJSRuntime jsRuntime = null);

        Task<IMediaStreamService> CreateMediaStreamServiceAsync(IJSRuntime jsRuntime = null);
    }
}
