﻿using Artemis.Interface;

using Microsoft.Xna.Framework;
using System;

namespace FellSky.Game.Ships.Parts
{
    public class NavLight: ShipPart
    {
        public float Amplitude { get; set; } = 1;
        public float Frequency { get; set; } = 1;
        public float PhaseShift { get; set; } = 0;
        public float VerticalShift { get; set; } = 0;
    }
}