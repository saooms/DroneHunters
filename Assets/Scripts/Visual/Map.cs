using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public class Map : MonoBehaviour, IInteractable
    {
        public static Map activeMap; // sneaky sneaky

        private Vector2 _mapOffset = Vector2.zero;
        
        [SerializeField] private Transform mapChunkPrefab;
        [SerializeField] private DroneMarker hunterDronePrefab;
        [SerializeField] private DroneMarker targetDronePrefab;
        [SerializeField] private Transform mapHolder;

        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        
        private int zoom = 1;

        private const int planeScale = 10;
        private Vector2 mapOffset
        {
            get => _mapOffset;

            set
            {
                _mapOffset.x = value.x % Mathf.Pow(2, zoom);
                _mapOffset.y = value.y % Mathf.Pow(2, zoom);
            }
        }

        private void Awake()
        {
            activeMap = this;
            UpdateMap();
        }

        /// <summary>
        ///     Sets the zoom value and updates the map.
        /// </summary>
        private void ChangeZoom(int zoom)
        {
            zoom = Mathf.Clamp(zoom, 1, 13);

            if (this.zoom == zoom)
                return;

            this.zoom = Mathf.Clamp(zoom, 1, 13);
            UpdateMap();
        }

        /// <summary>
        ///     Reloads all visible mapchunks.
        /// </summary>
        private void UpdateMap()
        {
            foreach (Transform child in mapHolder)
            {
                Destroy(child.gameObject);
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    LoadChunk(x, y, zoom);
                }
            }
        }

        private async void LoadChunk(int x, int y, int z)
        {
            Vector3 relativePosition = new Vector3(x - mapWidth * .5f, 0, y - mapHeight * .5f) * planeScale;
            Vector3 offset = new Vector3(planeScale * .5f, 0, planeScale * .5f);

            Texture2D tex = new Texture2D(10, 10);

            byte[] mapImage =  await API.Map.GetMapSegment(CalculateSlippyCoordinates(x,y,z));
            if (mapImage != null)
            {
                tex?.LoadImage(mapImage);
            }

            Renderer panel = Instantiate(mapChunkPrefab, relativePosition + offset, Quaternion.Euler(0,180,0), mapHolder).GetComponent<Renderer>();
            panel.material.mainTexture = tex;
        }

        private Vector3 CalculateSlippyCoordinates(int x, int y, int z)
        {
            return new Vector3( Mathf.Abs((x + (int)mapOffset.x + 2 * (zoom-2)) % Mathf.Pow(2, zoom)), Mathf.Abs((zoom*2 - 1 - y + (int)mapOffset.y + 2 * (zoom-2)) % Mathf.Pow(2, zoom)), z);
        }

        public void OnInteraction()
        {
            // left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                    PlaceMarker(hit.point, hunterDronePrefab.transform);
            }

            // middle mouse button click
            if (Input.GetMouseButton(2))
            {
                Vector2 newMapOffset = mapOffset + new Vector2(Input.GetAxis("Mouse X") * 0.05f, Input.GetAxis("Mouse Y") * 0.1f);

                bool mapShift = (int)newMapOffset.magnitude != (int)mapOffset.magnitude;

                mapOffset = newMapOffset;

                if (mapShift)
                    UpdateMap();
            }

            // scroll
            if (Input.mouseScrollDelta.y != 0)
                ChangeZoom(zoom + (int)Input.mouseScrollDelta.y);
        }

        public void PlaceMarker(Vector3 position, Transform markerPrefab)
        {
            if (markerPrefab == null)
                return;

            Instantiate(markerPrefab, position, markerPrefab.rotation);
        }

        public DroneMarker PlaceDrone(Drone drone)
        {
            DroneMarker marker = Instantiate(targetDronePrefab, drone.position.worldPosition, targetDronePrefab.transform.rotation);
            marker.AllocateDrone(drone);

            return marker;
        }
    }
}