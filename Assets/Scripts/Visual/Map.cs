using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public class Map : MonoBehaviour, IInteractable
    {
        public static Map activeMap; // sneaky sneaky


        Vector2 _mapOffset = Vector2.zero;

        [SerializeField] private Transform mapChunkPrefab;
        [SerializeField] private DroneMarker hunterDronePrefab;
        [SerializeField] private DroneMarker targetDronePrefab;
        [SerializeField] private Transform mapHolder;

        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        
        private int zoom = 1;

        private const int planeScale = 10;
        private Vector2 mapOffset {
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

        private void ChangeZoom(int zoom)
        {
            zoom = Mathf.Clamp(zoom, 1, 13);

            if (this.zoom == zoom)
                return;

            this.zoom = Mathf.Clamp(zoom, 1, 13);
            UpdateMap();
        }

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

            //Debug.Log(x + (int)mapOffset.x);
            //Debug.Log(mapHeight - y - 1 + (int)mapOffset.y);

            byte[] mapImage =  await API.Map.GetMapSegment(x + (int)mapOffset.x, mapHeight - y - 1 + (int)mapOffset.y, z);
            if (mapImage != null)
            {
                tex?.LoadImage(mapImage);
            }

            Renderer panel = Instantiate(mapChunkPrefab, relativePosition + offset, Quaternion.Euler(0,180,0), mapHolder).GetComponent<Renderer>();
            panel.material.mainTexture = tex;
        }

        public void OnInteraction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                    PlaceMarker(hit.point, hunterDronePrefab.transform);
            }

            if (Input.GetMouseButtonDown(2))
            {
                Debug.Log(Input.GetAxis("Mouse X"));
                Debug.Log(Input.GetAxis("Mouse Y"));

                mapOffset = mapOffset + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                UpdateMap();
            }

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