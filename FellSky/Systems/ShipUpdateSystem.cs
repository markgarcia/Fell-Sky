﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artemis;
using FellSky.Components;
using Microsoft.Xna.Framework;
using static FellSky.Utilities;

namespace FellSky.Systems
{
    public class ShipUpdateSystem: Artemis.System.ParallelEntityProcessingSystem
    {
        public ShipUpdateSystem()
            : base (Aspect.All(typeof(ShipComponent)))
        { }


        public override void Process(Entity ship)
        {
            var shipComponent = ship.GetComponent<ShipComponent>();
            if (ship.HasComponent<RigidBodyComponent>())
            {
                var rigidBody = ship.GetComponent<RigidBodyComponent>();
                Vector2 linearForce = shipComponent.LinearThrustVector;
                if (linearForce.X > 0) linearForce.X *= shipComponent.Ship.Handling.ForwardForce;
                else linearForce.X *= shipComponent.Ship.Handling.ManeuverForce;
                linearForce.Y *= shipComponent.Ship.Handling.ManeuverForce;
                rigidBody.Body.ApplyForce(rigidBody.Body.GetWorldVector(linearForce), rigidBody.Body.WorldCenter);
                rigidBody.Body.ApplyTorque(shipComponent.AngularTorque * shipComponent.Ship.Handling.AngularTorque);
            }          
            UpdateThrusters(ship);

            shipComponent.AngularTorque = 0;
            shipComponent.LinearThrustVector = Vector2.Zero;
        }

        private void UpdateThrusters(Entity ship)
        {
            var shipComponent = ship.GetComponent<ShipComponent>();
            var thrusters = shipComponent.Thrusters.Select(e => e.GetComponent<ThrusterComponent>());
            var xform = ship.GetComponent<Transform>();

            bool isShipTurning = Math.Abs(shipComponent.AngularTorque) > 0;
            var isShipThrusting = shipComponent.LinearThrustVector.LengthSquared() > 0;

            foreach (var thruster in thrusters)
            {
                if (thruster == null) continue;
                bool isThrusting = false;
                if (isShipThrusting)
                {
                    var offset = Math.Abs(GetLesserAngleDifference(shipComponent.LinearThrustVector.GetAngleRadians() - MathHelper.Pi / 2f, thruster.Part.Transform.Rotation));
                    isThrusting = offset < 1;
                }
                if (isShipTurning)
                {
                    var rotateDir = (AngularDirection)Math.Sign(shipComponent.AngularTorque);
                    //isThrusting = rotateDir == thruster.RotateDir;
                    isThrusting |= thruster.GetAngularThrustMult(rotateDir, Vector2.Zero) > 0;
                }

                thruster.IsThrusting = isThrusting;
                if (isThrusting)
                    thruster.ThrustPercentage = MathHelper.Clamp(thruster.ThrustPercentage + 0.05f, 0, 1);
                else
                    thruster.ThrustPercentage = MathHelper.Clamp(thruster.ThrustPercentage - 0.05f, 0, 1);
            }
        }

        private void UpdateTurrets(Entity ship)
        {

        }
    }
}
