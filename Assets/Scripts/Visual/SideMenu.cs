using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;
using TMPro;
using System;
using static Visual.MapMarker;
using static System.Math;
using UnityEngine.UI;

namespace Visual
{
    public class SideMenu : MonoBehaviour, IInteractable
    {
        struct LiveDataField
        {
            public TextMeshProUGUI guiElement;
            public Func<Drone, string> textSelection;

            public LiveDataField(Func<Drone, string> textSelection)
            {
                guiElement = Instantiate(activeMenu.textPrefab, activeMenu.dataHolder);
                this.textSelection = textSelection;
            }

            public void Update(Drone drone)
            {
                guiElement.text = textSelection(drone);
            }
        }

        public static SideMenu activeMenu;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform dataHolder;

        [SerializeField] TextMeshProUGUI textPrefab;
        [SerializeField] Button buttonPrefab;

        private List<MapMarker> markers;
        private List<LiveDataField> fields;
        private Drone selectedDrone;

        private void Awake()
        {

            activeMenu = this;
            markers = new List<MapMarker>();
            fields = new List<LiveDataField>();
        }

        private void FixedUpdate()
        {
            foreach (LiveDataField field in fields)
                field.Update(selectedDrone);
        }

        public void DisplayDroneData(Drone drone)
        {
            selectedDrone = drone;

            ClearData();

            title.text = drone.Identifier;

            fields.Add(new LiveDataField(d => $"Position:\n {d.Position}"));
            fields.Add(new LiveDataField(d => $"Speed: {Round(d.Speed,2)}m/s"));
            fields.Add(new LiveDataField(d => $"Heading: {Abs(Round(d.Heading,2))} deg"));

            switch (drone.type)
            {
                case Drone.Type.hunter:
                    DisplayHunter(drone);
                    break;
                case Drone.Type.target:
                    DisplayTarget(drone);
                    break;
                default:
                    break;
            }
        }

        private void DisplayTarget(Drone drone)
        {
            Button button = Instantiate(buttonPrefab, dataHolder);
            button.onClick.AddListener(delegate () {
                UserDroneController.ActiveController.AllocateDrone(drone);
            });
        }

        private void DisplayHunter(Drone drone)
        {
            markers.Add(Map.ActiveMap.PlaceDroneMarker(drone, MarkerType.Prediction));

            fields.Add(new LiveDataField(d => $"Distance to target: {Round(((HunterDrone)d).DistanceToTarget(),2)}m"));
        }

        private void ClearData()
        {
            title.text = "";

            foreach (Transform marker in dataHolder)
                Destroy(marker.gameObject);

            foreach (MapMarker marker in markers)
                Destroy(marker.gameObject);

            markers.Clear();
            fields.Clear();
        }

        public void OnInteraction()
        {
            
        }
    }
}