using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Visual;

namespace Logic
{
    public class MapChunk
    {
        private readonly Vector3 _position;

        public Vector3 Position => new Vector3(_position.x, Map.Zoom, _position.z);

        public MapChunk(Vector3 position, bool fromGamePosition = false)
        {
            this._position = fromGamePosition ? GamePositionToMapPanel(position) : ValidChunkPosition(position);
        }

        public async Task<byte[]> GetMapSegment()
        {
            return await API.Map.GetMapSegment(Position);
        }

        public Vector3 GetGamePosition()
        {
            return Coordinate.MapPositionToGamePosition(_position) - new Vector3(-.5f * Map.PlaneScale, 0, .5f * Map.PlaneScale);
        }

        public static Vector3 GamePositionToMapPanel(Vector3 position)
        {
            Coordinate coord = new Coordinate(position, Coordinate.Type.Game);
            return new Vector3(
                Mathf.Floor((float)coord.MapPosition.x),
                position.y,
                Mathf.Floor((float)coord.MapPosition.z)
            );
        }

        private static Vector3 ValidChunkPosition(Vector3 position)
        {
            int maxMapIndex = Map.MapSize - 1;

            return new Vector3(
                Mathf.Clamp(position.x, 0, maxMapIndex),
                position.y,
                Mathf.Clamp(Map.MapSize - position.z, 0, maxMapIndex)
            );
        }
    }
}