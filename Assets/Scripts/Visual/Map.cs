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

        public static int planeScale = 10;

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
                    LoadChunk(panelsPos + new Vector3(x, y, zoom));
                }
            }
        }

        private async void LoadChunk(Vector3 position)
        {
            //position = new Vector3(position.x Mathf.Pow(2, zoom));
            Debug.Log(position);

            if (panels.ContainsKey(position))
                return;

            panels.Add(position, null);

            Vector3 relativePosition = new Vector3(position.x, 0, position.y) * planeScale;
            Vector3 offset = new Vector3(.5f * planeScale, 0, .5f * planeScale);

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
            return position;
        }

        public void OnInteraction()
        {
            // left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    PlaceMarker(hit.point, hunterDronePrefab.transform);

                    Vector3 worldPos = new Vector3(Mathf.Abs(hit.point.x) / planeScale, zoom, hit.point.z / planeScale);
                    Debug.Log(worldPos);
                    Vector3 gamePos = Coordinate.MapPositionToWorldPosition(worldPos);
                    Debug.Log(gamePos);
                    Debug.Log(Coordinate.WorldPositionToMapPosition(gamePos));
                }
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
            return new Vector3(
                Mathf.Clamp(Mathf.Floor(position.x / planeScale), 0, Mathf.Pow(2, position.z)),
                Mathf.Clamp(Mathf.Floor(position.z / planeScale), 0, Mathf.Pow(2, position.z)),
                position.z);
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

        public static Vector3 MapPositionToGamePosition()
        {
            return new();
        }

        public static Vector3 GamePositionToMapPosition(Vector3 gamePosition)
        {
            return new();
        }
    }
}