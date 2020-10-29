﻿using System;
using System.Collections.Generic;
using System.Text;
using WebRTCme;

namespace WebRtc.iOS
{
    public abstract class ApiBase : INativeObjects
    {
        protected ApiBase(object nativeObject = null) => SelfNativeObject = nativeObject;

        public object SelfNativeObject { get; }
        public List<object> OtherNativeObjects { get; set; } = new List<object>();

        public void Dispose()
        {
            foreach(var otherNativeObject in OtherNativeObjects)
            {
                if (otherNativeObject is IDisposable otherDisposable)
                {
                    otherDisposable.Dispose();
                }
            }

            if (SelfNativeObject != null && SelfNativeObject is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}