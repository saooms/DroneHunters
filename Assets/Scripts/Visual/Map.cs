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
        private Vector3 dragOrigin = Vector2.zero;
        private Dictionary<Vector2, GameObject> panels = new Dictionary<Vector2, GameObject>();

        private const int planeScale = 10;

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

            foreach (GameObject child in panels.Values)
            {
                Destroy(child);
            }
            panels.Clear();

            UpdateMap();
        }

        /// <summary>
        ///     Reloads all visible mapchunks.
        /// </summary>
        private void UpdateMap()
        {
            Vector3 panelsPos = GamePostionToMapPanel(Camera.main.transform.position);

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    LoadChunk(panelsPos + new Vector3(x, y, 0));
                }
            }

        }

        private async void LoadChunk(Vector3 position)
        {
            if (panels.ContainsKey(position))
                return;

            panels.Add(position, null);

            Vector3 relativePosition = new Vector3(position.x - mapWidth * .5f, 0, position.y - mapHeight * .5f) * planeScale;
            Vector3 offset = new Vector3(planeScale * .5f, 0, planeScale * .5f);

            Texture2D tex = new Texture2D(10, 10);

            byte[] mapImage =  await API.Map.GetMapSegment(CalculateSlippyCoordinates(position));
            if (mapImage != null)
            {
                tex?.LoadImage(mapImage);
            }

            Renderer panel = Instantiate(mapChunkPrefab, relativePosition + offset, Quaternion.Euler(0,180,0), mapHolder).GetComponent<Renderer>();
            panel.material.mainTexture = tex;
            panels[position] = panel.gameObject;
        }

        private Vector3 CalculateSlippyCoordinates(Vector3 position)
        {
            return new Vector3( Mathf.Abs((position.x) % Mathf.Pow(2, zoom)), Mathf.Abs((zoom*2 - 1 - position.y) % Mathf.Pow(2, zoom)), position.z);
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
            if (Input.GetMouseButtonDown(2))
            {
                dragOrigin = Input.mousePosition; 
            }

            // middle mouse button click helddown
            if (Input.GetMouseButton(2))
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * 0.2f, 0, pos.y * 0.2f);
                Camera.main.transform.Translate(move, Space.World);

                UpdateMap();
            }

            // scroll
            if (Input.mouseScrollDelta.y != 0)
                ChangeZoom(zoom + (int)Input.mouseScrollDelta.y);
        }

        public Vector3 GamePostionToMapPanel(Vector3 position)
        {
            return new Vector3( Mathf.Floor(position.x / planeScale), Mathf.Floor(position.z / planeScale), zoom);
        }

        public Coordinate GameToWorldPosition(Vector3 position)
        {
            Vector3 p = new Vector3();
            float n = (float)(Mathf.PI - ((2.0 * Mathf.PI * position.z) / Mathf.Pow(2, zoom)));

            p.x = (float)((position.x / Mathf.Pow(2, zoom) * 360.0) - 180.0);
            p.y = (float)(180.0 / Mathf.PI * Mathf.Atan(Mathf.Exp(n) - Mathf.Exp(-n) / 2f));

            return new Coordinate();
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