﻿using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher;
using MelonLoader;
using UnityEngine;

namespace AcceleratorThings
{
    [RegisterTypeInIl2Cpp]
    public class FilterAccelerator : SRBehaviour
    {
        public FilterAccelerator(IntPtr ptr) : base(ptr) { }

        private Accelerator accelOne;
        private Accelerator accelTwo;

        private TechItemDisplay display;

        public void Awake()
        {
            Accelerator[] accels = transform.parent.GetComponentsInChildren<Accelerator>();
            accelOne = accels[0];
            accelTwo = accels[1];

            display = transform.parent.GetComponentInChildren<TechItemDisplay>();
        }

        public void OnTriggerEnter(Collider other)
        {
            Vacuumable.TryGetVacuumable(other.gameObject, out var vacuumable);
            if (other.attachedRigidbody == null || vacuumable == null)
                return;
            if (!accelOne.CanLaunchObject(other.attachedRigidbody.gameObject))
                return;

            IdentifiableType type = display.GetRelevantAmmo().Slots[0].Id;
            if (type == null)
            {
                accelOne.OnTriggerEnter(other);
                return;
            }

            if (other.GetComponent<IdentifiableActor>().identType == type)
                accelTwo.OnTriggerEnter(other);
            else
                accelOne.OnTriggerEnter(other);
        }
    }
}
