﻿using System;

namespace Core.Extensions
{
    public static class ByteArrrayExtensions
    {
        public static string ToBase64(this byte[] bytes)
            => Convert.ToBase64String(bytes);
    }
}