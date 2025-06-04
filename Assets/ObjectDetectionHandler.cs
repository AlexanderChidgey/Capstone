using UnityEngine;
using TMPro; // Add this
using System.Collections.Generic; // Add this for List<>

public class ObjectDetectionHandler : MonoBehaviour
{
    public GameObject Cube;
    public TMP_Text detectionText; 
    private DistanceMatching distanceMatching;

    void Start()
    {
        if (Cube != null)
        {
            Cube.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Cube is not assigned in ObjectDetectionHandler.");
        }

        // Get the DistanceMatching component
        distanceMatching = GetComponent<DistanceMatching>();
        if (distanceMatching == null)
        {
            distanceMatching = gameObject.AddComponent<DistanceMatching>();
            Debug.Log("Added DistanceMatching component");
        }
    }

    public void HandleDetection(int classId, float latitude, float longitude, float heading)
    {
        Debug.Log($"Detection: {classId},{latitude},{longitude},{heading}");

        // Show the cube and update its color
        if (Cube != null)
        {
            Cube.SetActive(true);
            Renderer renderer = Cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = classId == 0 ? Color.red :
                                        classId == 1 ? Color.blue : Color.white;
            }
        }

        // Get nearby substations using DistanceMatching
        string nearbySubstations = "";
        if (distanceMatching != null)
        {
            nearbySubstations = distanceMatching.FindNearbySubstations(latitude, longitude, heading);
            Debug.Log("Objects Nearby distanceMatching: " + nearbySubstations.GetType());
        }
        Debug.Log("Objects Nearby outside of loop: " + nearbySubstations);

        // Update detection text
        if (detectionText != null)
        {
            string objectType = classId == 0 ? "Pillar Box" :
                              classId == 1 ? "Power Pole" : "Unknown Object";

            string fullText = $"Detected: {objectType}\n" +
                           $"Lat: {latitude:F6}, Lon: {longitude:F6}\n" +
                           $"Heading: {heading:F6}°\n Substations:{nearbySubstations}";

            Debug.Log("Setting text to: " + fullText);
            detectionText.text = fullText;
            
            // Force the text to update
            detectionText.SetText(fullText);
            detectionText.ForceMeshUpdate();
        }
        else
        {
            Debug.LogError("Cannot update text - TextMeshPro Text component is null!");
        }
    }
}