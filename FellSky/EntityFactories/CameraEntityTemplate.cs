﻿using Artemis.Interface;
using Artemis;

using FellSky.Components;
using Microsoft.Xna.Framework.Graphics;

namespace FellSky.EntityFactories
{
    public class CameraEntityFactory
    {
        public CameraEntityFactory(EntityWorld world)
        {
            World = world;
        }

        public EntityWorld World { get; private set; }

        public Entity CreateCamera(string tag, GraphicsDevice device)
        {
            var entity = World.CreateEntity();
            var camera = new CameraComponent(device);
            entity.AddComponent(camera);
            entity.AddComponent(camera.Transform);
            entity.Tag = tag;
            return entity;
        }
    }
}
