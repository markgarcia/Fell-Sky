﻿using Artemis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellSky.Game.Combat
{
    public interface IWeapon
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }

        float DamagePerSecond { get; }

        void Fire(EntityWorld world, Entity owner, Entity weapon);
        Entity Install(EntityWorld world, Entity owner, Entity hardpoint);
        void Uninstall(Entity owner, Entity weapon);
    }


}
