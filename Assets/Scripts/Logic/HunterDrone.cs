using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class HunterDrone : Drone
    {
        private TargetDrone target;

        public HunterDrone(Coordinate position,  TargetDrone target, string identifier = "Hunterdrone") : base(position, identifier)
        {
            this.target = target;
        }

        /// <summary>
        ///     Calculate a path to the target.
        /// </summary>
        /// 
        /// <remarks>
        ///     Unsure what it should return yet..
        /// </remarks>
        public void CalculatePath()
        {

        }
       
    }
}