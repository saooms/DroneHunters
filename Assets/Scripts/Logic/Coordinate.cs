using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Logic
{
    [Serializable]
    public class Coordinate
    {
        [SerializeField] 
        private Vector3 _worldPosition;

        public Vector3 worldPosition { get => _worldPosition; }
        public Vector3 gamePosition {
            get {
                return WorldPositionToGamePosition(_worldPosition);
            }
        }

        public Coordinate(int x = 0, int y = 1, int z = 0, bool isWorldPosition = true)
        {
            if (isWorldPosition)
                _worldPosition = new Vector3(x, y, z);
            else
                _worldPosition = GamePositionToWorldPosition(new Vector3(x, y, z));
        }

        public static Vector2 GamePositionToWorldPosition(Vector3 gamePosition)
        {
            // Implement convertion formula: gamePosition => worldPosition
            return gamePosition;
        }

        public static Vector2 WorldPositionToGamePosition(Vector3 worldPosition)
        {
            // Implement convertion formula: worldPosition => gamePosition
            return worldPosition;
        }

    }
}