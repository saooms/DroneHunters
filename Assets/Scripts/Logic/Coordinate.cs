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
        private Vector3 _gamePosition;

        public Vector3 gamePosition => _gamePosition;
        public Vector3 worldPosition
        {
            get
            {
                return WorldPositionToGamePosition(_gamePosition);
            }
        }

        public Coordinate(int x = 0, int y = 1, int z = 0, bool isWorldPosition = true)
        {
            if (isWorldPosition)
                _gamePosition = GamePositionToWorldPosition(new Vector3(x, y, z));
            else
                _gamePosition = new Vector3(x % y, y, z % y);
        }

        public static Vector3 GamePositionToWorldPosition(Vector3 gamePosition)
        {
            Vector3 p = new Vector3();
            float n = (float)(Mathf.PI - ((2.0 * Mathf.PI * gamePosition.z) / Mathf.Pow(2, gamePosition.y)));

            p.x = (float)((gamePosition.x / Mathf.Pow(2, gamePosition.y) * 360.0) - 180.0);
            p.z = (float)(180.0 / Mathf.PI * Mathf.Atan((Mathf.Exp(n) - Mathf.Exp(-n)) / 2f));
            p.y = gamePosition.y;

            return p;
        }

        public static Vector3 WorldPositionToGamePosition(Vector3 worldPosition)
        {
            Vector3 p = new Vector3();

            p.x = (float)((worldPosition.x + 180.0) / 360.0 * (1 << (int)worldPosition.y));
            p.z = (float)((1.0 - Math.Log(Math.Tan(worldPosition.z * Math.PI / 180.0) + 
                1.0 / Math.Cos(worldPosition.z * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << (int)worldPosition.y));
            p.y = worldPosition.y;

            return p;
        }

    }
}