﻿using System;
using UnityEngine;
using IPA.Config.Data;
using IPA.Config.Stores;

namespace AutoScreenShot.Converteres
{
    /// <summary>
    /// A config converter for BSIPA which can serialize and deserialize <see cref="Vector2"/> values.
    /// </summary>
    public class Vector2Converter : ValueConverter<Vector2>
    {
        /// <summary>
        /// Converts a config value map into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value">The config value.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public override Vector2 FromValue(Value value, object parent)
        {
            if (value is Map m) {
                m.TryGetValue("x", out Value x);
                m.TryGetValue("y", out Value y);
                Vector2 vec = Vector2.zero;
                if (x is FloatingPoint pointX) {
                    vec.x = (float)pointX.Value;
                }
                if (y is FloatingPoint pointY) {
                    vec.y = (float)pointY.Value;
                }
                return vec;
            }
            throw new ArgumentException("Value cannot be parsed into a Vector", nameof(value));
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> into a config value map.
        /// </summary>
        /// <param name="obj">The vector to convert.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public override Value ToValue(Vector2 obj, object parent)
        {
            var map = Value.Map();
            map.Add("x", Value.Float((decimal)obj.x));
            map.Add("y", Value.Float((decimal)obj.y));
            return map;
        }
    }
}