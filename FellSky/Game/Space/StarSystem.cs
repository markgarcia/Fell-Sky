﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellSky.Game.Space
{
    public sealed class StarSystem
    {
        [JsonIgnore]
        public SpaceObject MainBody { get { return Objects[0]; } }
        public List<SpaceObject> Objects { get; } = new List<SpaceObject>();
        
        public string Name { get; set; }
    }
}
