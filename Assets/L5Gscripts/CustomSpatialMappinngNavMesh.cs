using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// MRTK namespace
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

public class CustomSpatialMappinngNavMesh : MonoBehaviour, IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>
{
    /// <summary>
    /// Collection that tracks the IDs and count of updates for each active spatial awareness mesh.
    /// </summary>
    private Dictionary<int, uint> meshUpdateData = new Dictionary<int, uint>();

    /// <summary>
    /// Value indicating whether or not this script has registered for spatial awareness events.
    /// </summary>

    private void OnEnable()
    {
        // Register component to listen for Mesh Observation events, typically done in OnEnable()
        CoreServices.SpatialAwarenessSystem.RegisterHandler<IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>>(this);
    }

    private void OnDisable()
    {
        // Unregister component from Mesh Observation events, typically done in OnDisable()
        CoreServices.SpatialAwarenessSystem.UnregisterHandler<IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>>(this);
    }

    public virtual void OnObservationAdded(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
    {
        // A new mesh has been added.
        if (!meshUpdateData.ContainsKey(eventData.Id))
        {
            meshUpdateData.Add(eventData.Id, 0);
            eventData.SpatialObject.GameObject.AddComponent<NavMeshSourceTag>();
        }
    }

    public virtual void OnObservationUpdated(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
    {
        uint updateCount = 0;

        // A mesh has been updated. Find it and increment the update count.
        if (meshUpdateData.TryGetValue(eventData.Id, out updateCount))
        {
            // Set the new update count.
            meshUpdateData[eventData.Id] = ++updateCount;

            // Debug.Log($"Mesh {eventData.Id} has been updated {updateCount} times.");
            eventData.SpatialObject.GameObject.AddComponent<NavMeshSourceTag>();
        }
    }

    public virtual void OnObservationRemoved(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
    {
        // A mesh has been removed. We no longer need to track the count of updates.
        if (meshUpdateData.ContainsKey(eventData.Id))
        {
            // Debug.Log($"No longer tracking mesh {eventData.Id}.");
            meshUpdateData.Remove(eventData.Id);
        }
    }
}