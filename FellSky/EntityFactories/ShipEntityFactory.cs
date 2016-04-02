﻿using Artemis;
using FellSky.Models.Ships.Parts;
using FellSky.Components;
using FellSky.Services;
using FellSky.Ships;
using Microsoft.Xna.Framework;
using FellSky.Systems;
using System;
using Artemis.Interface;

namespace FellSky.EntityFactories
{
    public class ShipEntityFactory
    {
        private ISpriteManagerService _spriteManager;

        public EntityWorld World { get; set; }

        public ShipEntityFactory(EntityWorld world, ISpriteManagerService spriteManager)
        {
            _spriteManager = spriteManager;
            World = world;
        }
        
        public Entity CreateShipEntity(Ship ship, Vector2 position, float rotation=0, bool addPhysics=false)
        {
            var shipEntity = World.CreateEntity();
            shipEntity.AddComponent(new ShipComponent(ship));
            shipEntity.AddComponent(new Transform());

            if (addPhysics)
            {
                var physics = World.SystemManager.GetSystem<PhysicsSystem>();
                shipEntity.AddComponent(physics.CreateRigidBody(position, rotation));
            }

            foreach(var hull in ship.Hulls)
            {
                CreateHullEntityInternal(shipEntity, hull, addPhysics);
            }

            return shipEntity;
        }

        public Entity CreateHullEntity(Entity ship, Hull hull, bool addPhysics = false)
        {
            var entity = CreateHullEntityInternal(ship, hull, addPhysics);
            ship?.GetComponent<ShipComponent>().Ship.Hulls.Add(hull);
            return entity;
        }

        public Entity CreateThrusterEntity(Entity ship, Thruster thruster)
        {
            var entity = CreatePartEntity(ship, new ThrusterComponent(thruster, ship));
            ship?.GetComponent<ShipComponent>().ThrusterEntities.Add(entity);
            return entity;
        }

        private Entity CreatePartEntity<TComponent>(Entity ship, TComponent component)
            where TComponent: IShipPartComponent, IComponent
        {
            var part = component.Part;
            var entity = World.CreateEntity();
            entity.AddComponent(component);
            entity.AddComponent(new LocalTransformComponent(ship));
            entity.AddComponent(part.Transform);
            var spriteComponent = _spriteManager.CreateSpriteComponent(part.SpriteId);
            entity.AddComponent(spriteComponent);
            entity.AddComponent(new BoundingBoxComponent(new FloatRect(0, 0, spriteComponent.TextureRect.Width, spriteComponent.TextureRect.Height)));
            return entity;
        }

        private Entity CreateHullEntityInternal(Entity ship, Hull hull, bool addPhysics)
        {
            var entity = CreatePartEntity(ship, new HullComponent(hull, ship));
            ship?.GetComponent<ShipComponent>().HullEntities.Add(entity);
            if (addPhysics)
            {
                var physics = World.SystemManager.GetSystem<PhysicsSystem>();
                RigidBodyComponent body;
                if (hull.PhysicsBodyIndex == 0)
                    body = ship?.GetComponent<RigidBodyComponent>();
                else
                    body = ship?.GetComponent<ShipComponent>()?.AdditionalRigidBodyEntities[hull.PhysicsBodyIndex].GetComponent<RigidBodyComponent>();
                entity.AddComponent(physics.CreateAndAttachFixture(ship.GetComponent<RigidBodyComponent>(), hull.ShapeId, hull.Transform));
            }
            return entity;
        }
    }
}
