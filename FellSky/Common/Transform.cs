﻿using System;
using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace FellSky
{
    public interface ITransform
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
        Vector2 Origin { get; set; }

        Matrix Matrix { get; }
    }

    public class Transform : ITransform, IComponent
    {
        private bool _matrixNeedsUpdate;
        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;
        private Matrix _matrix;
        private Vector2 _origin;

        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _matrixNeedsUpdate = true;
                _position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }

            set
            {
                _matrixNeedsUpdate = true;
                _rotation = value;
            }
        }

        public Vector2 Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _matrixNeedsUpdate = true;
                _scale = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return _origin;
            }

            set
            {
                _matrixNeedsUpdate = true;
                _origin = value;
            }
        }

        public Matrix Matrix
        {
            get
            {
                if (_matrixNeedsUpdate)
                {
                    _matrix = this.GetMatrix();
                    _matrixNeedsUpdate = false;
                }
                return _matrix;
            }
        }

        public Transform Clone()
        {
            return (Transform)MemberwiseClone();
        }

        public void Combine(Transform transform)
        {
            Vector2 position, scale;
            float rotation;
            _matrix = Matrix * transform.Matrix;

            Utilities.DecomposeMatrix2D(ref _matrix, out position, out rotation, out scale);
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
