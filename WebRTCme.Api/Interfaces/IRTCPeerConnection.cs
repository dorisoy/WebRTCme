﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebRTCme
{
    public interface IRTCPeerConnection : IAsyncDisposable
    {
        ValueTask<IAsyncDisposable> OnIceCandidate(Func<RTCPeerConnectionIceEvent, ValueTask> callback);
        ValueTask<IAsyncDisposable> OnTrack(Func<RTCTrackEvent, ValueTask> callback);
        ValueTask<IRTCRtpSender> AddTrack(IMediaStreamTrack track, IMediaStream stream);
        ValueTask<RTCSessionDescription> CreateOffer(RTCOfferOptions options);
    }
}