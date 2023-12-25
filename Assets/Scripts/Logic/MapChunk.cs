using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Visual;

namespace Logic
{
    public class MapChunk
    {
        private Vector3 position;

        public MapChunk(Vector3 position, bool fromGamePosition = true)
        {
            this.position = fromGamePosition ? GamePostionToMapPanel(position) : ValidChunkPosition(position);
        }

        public async Task<byte[]> GetMapSegment()
        {
            return await API.Map.GetMapSegment(position);
        }

        public static Vector3 GamePostionToMapPanel(Vector3 position)
        {
            return new Vector3(
                Mathf.Clamp(Mathf.Floor(position.x / Map.planeScale), 0, Mathf.Pow(2, position.z)),
                Mathf.Clamp(Mathf.Floor(position.z / Map.planeScale), 0, Mathf.Pow(2, position.z)),
                position.z);
        }
        public static Vector3 ValidChunkPosition(Vector3 position)
        {
            return new Vector3(
                Mathf.Clamp(position.x, 0, Mathf.Pow(2, position.z)),
                Mathf.Clamp(position.z, 0, Mathf.Pow(2, position.z)),
                position.z);
        }
    }
}