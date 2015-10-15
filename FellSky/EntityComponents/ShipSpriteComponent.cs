﻿using Artemis.Interface;
using FellSky.Common;
using FellSky.Graphics;
using FellSky.Mechanics.Ships;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FellSky.EntityComponents
{
    public class ShipSpriteComponent: IComponent
    {
        public Ship Ship { get; set; }
        public ShipSprite Sprite { get; set; }

    }
}
