﻿using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebRTCme;

namespace WebRtc.iOS
{
    internal static class ModelExtensions
    {
        public static Webrtc.RTCConfiguration ToNative(this RTCConfiguration configuration) =>
            new Webrtc.RTCConfiguration
            {
                BundlePolicy = configuration.BundlePolicy?.ToNative() ?? Webrtc.RTCBundlePolicy.Balanced,
                Certificate = (Webrtc.RTCCertificate)(configuration.Certificates?.ElementAt(0).NativeObject) ??
                    //new Webrtc.RTCCertificate("TODO:private key", "TODO: certificate"),
                    Webrtc.RTCCertificate.GenerateCertificateWithParams(new NSDictionary<NSString, NSObject>(
                        new[] { new NSString("expires"), new NSString("name") },
                        new NSObject[] { new NSNumber(100000), new NSString("RSASSA-PKCS1-v1_5") })),

                IceCandidatePoolSize = configuration.IceCandidatePoolSize ?? 0,
                IceServers = configuration.IceServers.Select(server => server.ToNative()).ToArray(),
                IceTransportPolicy = configuration.IceTransportPolicy?.ToNative() ?? Webrtc.RTCIceTransportPolicy.All,
                RtcpMuxPolicy = configuration.RtcpMuxPolicy?.ToNative() ?? Webrtc.RTCRtcpMuxPolicy.Require
            };

        public static Webrtc.RTCIceServer ToNative(this RTCIceServer iceServer) =>
            new Webrtc.RTCIceServer
            (
                urlStrings: iceServer.Urls,
                username: iceServer.Username,
                credential: iceServer.Credential,
                tlsCertPolicy: iceServer.CredentialType?.ToNative() ?? Webrtc.RTCTlsCertPolicy.Secure
            );


        public static Webrtc.RTCDataChannelConfiguration ToNative(this RTCDataChannelInit dataChannelInit) =>
            new Webrtc.RTCDataChannelConfiguration
            {
                IsOrdered = dataChannelInit.Ordered ?? true,
                MaxPacketLifeTime = dataChannelInit.MaxPacketLifeTime ?? -1,
                MaxRetransmits = dataChannelInit.MaxRetransmits ?? -1,
                Protocol = dataChannelInit.Protocol ?? string.Empty,
                IsNegotiated = dataChannelInit.Negotiated ?? false,
                StreamId = dataChannelInit.Id ?? WebRTCme.WebRtc.Id
            };

        public static Webrtc.RTCSessionDescription ToNative(this RTCSessionDescriptionInit description) =>
            new Webrtc.RTCSessionDescription(description.Type.ToNative(), description.Sdp);

        public static RTCConfiguration FromNative(this Webrtc.RTCConfiguration nativeConfiguration) =>
            new RTCConfiguration
            {
                BundlePolicy = nativeConfiguration.BundlePolicy.FromNative(),
                Certificates = new IRTCCertificate[]
                    { RTCCertificate.Create(nativeConfiguration.Certificate) },
                IceCandidatePoolSize = (byte)nativeConfiguration.IceCandidatePoolSize,
                IceServers = nativeConfiguration.IceServers.Select(nativeServer => nativeServer.FromNative()).ToArray(),
                IceTransportPolicy = nativeConfiguration.IceTransportPolicy.FromNative(),
                RtcpMuxPolicy = nativeConfiguration.RtcpMuxPolicy.FromNative()
            };

        public static Webrtc.RTCIceCandidate ToNative(this RTCIceCandidateInit iceCandidateInit) =>
            new Webrtc.RTCIceCandidate(iceCandidateInit.Candidate, (int)iceCandidateInit.SdpMLineIndex, 
                iceCandidateInit.SdpMid)
            {
                ///ServerUrl = ???
            };

        public static RTCIceServer FromNative(this Webrtc.RTCIceServer nativeIceServer) =>
            new RTCIceServer
            {
                Credential = nativeIceServer.Credential,
                CredentialType = nativeIceServer.TlsCertPolicy.FromNative(),
                Urls = nativeIceServer.UrlStrings,
                Username = nativeIceServer.Username
            };

        public static RTCRtpReceiveParameters FromNativeToReceive(this Webrtc.RTCRtpParameters nativeRtpParameters) =>
            new RTCRtpReceiveParameters
            {
                Codecs = nativeRtpParameters.Codecs.Select(nativeCodec => nativeCodec.FromNative()).ToArray(),
                HeaderExtensions = nativeRtpParameters.HeaderExtensions
                    .Select(nativeHeaderExtension => nativeHeaderExtension.FromNative()).ToArray(),
                Rtcp = null//// TODO: CHECK THIS
            };

        public static RTCRtpSendParameters FromNativeToSend(this Webrtc.RTCRtpParameters nativeRtpParameters) =>
            new RTCRtpSendParameters
            {
                Codecs = nativeRtpParameters.Codecs.Select(nativeCodec => nativeCodec.FromNative()).ToArray(),
                HeaderExtensions = nativeRtpParameters.HeaderExtensions
                    .Select(nativeHeaderExtension => nativeHeaderExtension.FromNative()).ToArray(),
                Rtcp = null,//// TODO: CHECK THIS
                Encodings = nativeRtpParameters.Encodings.Select(nativeEncoding => nativeEncoding.FromNative())
                    .ToArray(),
                TransactionId = nativeRtpParameters.TransactionId
            };

        public static RTCRtpCodecParameters FromNative(this Webrtc.RTCRtpCodecParameters nativeRtpCodecParameters) =>
            new RTCRtpCodecParameters
            {
                PayloadType = (byte)nativeRtpCodecParameters.PayloadType,
                MimeType = string.Empty, //// TODO: FIX THIS
                ClockRate = nativeRtpCodecParameters.ClockRate.UInt64Value,
                Channels = nativeRtpCodecParameters.NumChannels.UInt16Value,
                SdpFmtpLine = string.Empty //// TODO: FIX THIS
            };

        public static RTCRtpHeaderExtensionParameters FromNative(this Webrtc.RTCRtpHeaderExtension
            nativeRtpHeaderExtension) =>
            new RTCRtpHeaderExtensionParameters
            {
                Uri = nativeRtpHeaderExtension.Uri,
                Id = (ushort)nativeRtpHeaderExtension.Id,
                Encrypted = nativeRtpHeaderExtension.Encrypted
            };

        public static RTCRtpEncodingParameters FromNative(this Webrtc.RTCRtpEncodingParameters nativeEncoding) =>
            new RTCRtpEncodingParameters
            {
                Active = nativeEncoding.IsActive,
                CodecPayloadType = 0, //// TODO: CHECK THIS
                Dtx = RTCDtxStatus.Enabled, //// TODO: CHECK THIS
                MaxBitrate = nativeEncoding.MaxBitrateBps.UInt64Value,
                MaxFramerate = nativeEncoding.MaxFramerate.DoubleValue,
                Ptime = 0, //// TODO: CHECK THIS
                Rid = nativeEncoding.Rid,
                ScaleResolutionDownBy = nativeEncoding.ScaleResolutionDownBy.DoubleValue
            };

        public static RTCSessionDescriptionInit FromNative(this Webrtc.RTCSessionDescription nativeDescription) =>
            new RTCSessionDescriptionInit
            {
                Type = nativeDescription.Type.FromNative(),
                Sdp = nativeDescription.Sdp
            };

        public static RTCIceCandidateInit FromNative(this Webrtc.RTCIceCandidate nativeIceCandidate) =>
            new RTCIceCandidateInit
            {
                Candidate = nativeIceCandidate.Sdp,
                SdpMid = nativeIceCandidate.SdpMid,
                SdpMLineIndex = (ushort)nativeIceCandidate.SdpMLineIndex,
                //UsernameFragment = ???
            };

    }

}
