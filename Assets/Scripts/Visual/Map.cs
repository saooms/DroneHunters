using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public class Map : MonoBehaviour, IInteractable
    {
        public static Map ActiveMap;
        public static int MapSize => (int)Mathf.Pow(2, Zoom);

        [SerializeField] private Transform mapChunkPrefab;
        [SerializeField] private DroneMarker hunterDronePrefab;
        [SerializeField] private DroneMarker targetDronePrefab;
        [SerializeField] private DroneMarker predictionPrefab;
        [SerializeField] private Transform mapHolder;

        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;

        private Vector3 _dragOrigin = Vector2.zero;
        private readonly Dictionary<Vector3, GameObject> _panels = new Dictionary<Vector3, GameObject>();

        public static int PlaneScale => 10;
        public static int Zoom = 1;

        private void Awake()
        {
            ActiveMap = this;
            Zoom = 18;
            Focus(new Coordinate(new Vector3(51.9171581691157f, 1, 4.483959781570087f)));
        }

        /// <summary>
        ///     Sets the zoom value and updates the map.
        /// </summary>
        private bool ChangeZoom(int zoom)
        {
            zoom = Mathf.Clamp(zoom, 1, 19);

            if (Zoom == zoom)
                return false;

            Zoom = zoom;

            foreach (GameObject child in _panels.Values)
            {
                Destroy(child);
            }
            _panels.Clear();

            return true;
        }

        /// <summary>
        ///     Reloads all visible mapchunks.
        /// </summary>
        private void UpdateMap()
        {
            Vector3 panelsPos = MapChunk.GamePositionToMapPanel(new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z));
            panelsPos.z = MapSize - panelsPos.z;

            for (int x = -mapWidth; x < mapWidth; x++)
            {
                for (int z = -mapWidth; z < mapHeight; z++)
                {
                    LoadChunk(new MapChunk(panelsPos + new Vector3(x, 0, z)));
                }
            }
        }

        private async void LoadChunk(MapChunk chunk)
        {
            if (_panels.ContainsKey(chunk.Position))
                return;

            _panels.Add(chunk.Position, null);

            Texture2D tex = new Texture2D(10, 10);

            byte[] mapImage = await chunk.GetMapSegment();

            if (mapImage != null)
                tex?.LoadImage(mapImage);

            Renderer panel = Instantiate(mapChunkPrefab, chunk.GetGamePosition(), Quaternion.Euler(0,180,0), mapHolder).GetComponent<Renderer>();
            panel.material.mainTexture = tex;
            _panels[chunk.Position] = panel.gameObject;
        }

        public void OnInteraction()
        {
            // right mouse button click
            if (Input.GetMouseButtonDown(1))
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //if (Physics.Raycast(ray, out RaycastHit hit))
                //{
                //    Coordinate pos = new Coordinate(new Vector3(hit.point.x, 1, hit.point.z), Coordinate.Type.Game);

                //    PlaceDroneMarker(new HunterDrone(pos, null), MapMarker.MarkerType.Hunter);
                //}

            }
            
            // middle mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = Input.mousePosition; 
            }

            // middle mouse button click held down
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
                Vector3 move = new Vector3(-pos.x, 0, -pos.y) * 0.2f;
                Camera.main.transform.Translate(move, Space.World);

                UpdateMap();
            }

            // scroll
            if (Input.mouseScrollDelta.y != 0)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Coordinate focus = new Coordinate(worldPosition, Coordinate.Type.Game);
                if (ChangeZoom(Zoom + (int)Input.mouseScrollDelta.y))
                    Focus(focus);
            }
        }

        private void Focus(Coordinate origin)
        {
            Coordinate.MapOffset += new Vector2(origin.GamePosition.x, origin.GamePosition.z);
            Vector3 camPosition = origin.GamePosition;
            camPosition.y = Camera.main.transform.position.y;
            Camera.main.transform.position = camPosition;
            UpdateMap();
        }

        public DroneMarker PlaceDroneMarker(Drone drone, MapMarker.MarkerType type)
        {
            Vector3 dronePosition = drone.Position?.GamePosition ?? Vector3.one;
            DroneMarker marker = Instantiate(GetPrefab(type), dronePosition, GetPrefab(type).transform.rotation);
            marker.AllocateDrone(drone);

            return marker;
        }

        private DroneMarker GetPrefab(MapMarker.MarkerType type)
        {
            return type switch
            {
                MapMarker.MarkerType.Hunter => hunterDronePrefab,
                MapMarker.MarkerType.Prediction => predictionPrefab,
                _ => targetDronePrefab
            };
        }
    }
}