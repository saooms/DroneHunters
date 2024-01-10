using Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visual
{
    public class PredictionManager : DroneMarker
    {
        [SerializeField] private DronePredictionMarker targetPredictionMarkerPrefab;
        [SerializeField] private DronePredictionMarker hunterPredictionMarkerPrefab;

        private readonly Coordinate position = new Coordinate(Vector3.zero);
        private List<GameObject> resources;

        protected override Coordinate Position => position;

        private void Awake()
        {
            resources = new List<GameObject>();
        }

        public override void AllocateDrone(Drone drone)
        {
            base.AllocateDrone(drone);

            DronePredictionMarker targetPredict = Instantiate(targetPredictionMarkerPrefab);
            targetPredict.AllocateDrone(drone);
            targetPredict.predictionSelection = d => ((HunterDrone)d).PredictTargetLocation();

            DronePredictionMarker hunterPredict = Instantiate(hunterPredictionMarkerPrefab);
            hunterPredict.AllocateDrone(drone);
            hunterPredict.predictionSelection = d => ((HunterDrone)d).CalculateLocationBehindPredictedLocation(((HunterDrone)d).PredictTargetLocation());

            resources.Add(targetPredict.gameObject);
            resources.Add(hunterPredict.gameObject);
        }
        
        private void OnDestroy()
        {
            foreach (GameObject resource in resources)
                Destroy(resource);
        }
    }
}