﻿using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellSky.Models.Ships.Parts
{
    public abstract class ShipPart: ICloneable
    {
        public string Name { get; set; }
        public PartGroup Group { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public string SpriteId { get; set; }
        public float Depth { get; set; } = 0;

        public ShipPart()
        {
             Name = Name ?? this.GetType().Name;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
