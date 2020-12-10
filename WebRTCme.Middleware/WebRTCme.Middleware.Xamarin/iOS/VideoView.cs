﻿using AVFoundation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using WebRTCme;

//namespace WebRtcMiddlewareXamarin.iOS
//namespace WebRTCme.Middleware.Xamarin
namespace WebRtcMiddlewareXamarin
{
    public class VideoView : UIView
    {
        public UIView LocalVideoView { get; }

        public VideoView(UIView view)
        {
            LocalVideoView = view;
            AddSubview(LocalVideoView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            LocalVideoView.Frame = Bounds;
        }
    }
}
