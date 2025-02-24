using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script handles the spinning behavior of a wheel UI element in Unity.
/// The wheel rotates at a randomized speed that gradually decreases over a set duration.
/// After spinning stops, the script determines the section the wheel lands on.
/// </summary>
public class U006_WheelRotationUI : MonoBehaviour
{
    [Header("Speed Settings")]
    [Tooltip("The lowest possible minimum speed.")]
    public float minSpeedRangeLow = 80f;

    [Tooltip("The highest possible minimum speed.")]
    public float minSpeedRangeHigh = 200f;

    [Tooltip("The lowest possible maximum speed.")]
    public float maxSpeedRangeLow = 350f;

    [Tooltip("The highest possible maximum speed.")]
    public float maxSpeedRangeHigh = 600f;

    [Header("Timing Settings")]
    [Tooltip("The duration over which the wheel slows to a stop.")]
    public float rotationDuration = 4f;

    [Tooltip("The delay before the wheel can spin again.")]
    public float resetDelay = 3f;

    private float currentSpeed; // The current rotation speed of the wheel
    private float elapsedTime;  // Time elapsed since the spin started
    private bool isSpinning = false; // Indicates if the wheel is currently spinning

    [Header("UI References")]
    [Tooltip("The image representing the wheel.")]
    public Image image; // The wheel image assigned via the Unity Inspector

    private RectTransform rectTransform; // RectTransform of the wheel image
    private float minSpeed; // Randomized minimum speed for the current spin
    private float maxSpeed; // Randomized maximum speed for the current spin

    private void Start()
    {
        // Get the RectTransform component from the assigned image
        if (image != null)
        {
            rectTransform = image.rectTransform;
        }
        else
        {
            Debug.LogError("Image is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // Handle wheel rotation while spinning
        if (isSpinning && rectTransform != null)
        {
            elapsedTime += Time.deltaTime;

            // Gradually decrease speed over time
            currentSpeed = Mathf.Lerp(maxSpeed, 0, elapsedTime / rotationDuration);

            // Rotate the wheel around the Z-axis
            rectTransform.Rotate(0, 0, -currentSpeed * Time.deltaTime);

            // Stop spinning when the duration is reached
            if (elapsedTime >= rotationDuration)
            {
                isSpinning = false;
                currentSpeed = 0;
                Debug.Log("Wheel stopped!");

                // Determine the final section of the wheel
                CheckWheelSection();
            }
        }
    }

    /// <summary>
    /// Starts the wheel spinning with randomized speed settings.
    /// </summary>
    public void StartSpin()
    {
        if (!isSpinning)
        {
            // Randomize minimum and maximum speeds
            minSpeed = Random.Range(minSpeedRangeLow, minSpeedRangeHigh);
            maxSpeed = Random.Range(maxSpeedRangeLow, maxSpeedRangeHigh);

            // Reset state and start spinning
            isSpinning = true;
            elapsedTime = 0f;
            currentSpeed = maxSpeed;

            Debug.Log($"Wheel started spinning! Min Speed: {minSpeed}, Max Speed: {maxSpeed}");
        }
    }

    /// <summary>
    /// Determines which section the wheel lands on based on its final rotation.
    /// </summary>
    private void CheckWheelSection()
    {
        if (rectTransform == null) return;

        // Get the current Z-axis rotation of the wheel
        float zRotation = rectTransform.eulerAngles.z;

        // Normalize rotation to a range of -180 to 180 degrees
        zRotation = (zRotation > 180) ? zRotation - 360 : zRotation;

        // Determine the section based on the rotation angle
        if (zRotation >= -30 && zRotation <= 30)
        {
            Debug.Log("Section: 1 (Yellow)");
        }
        else if (zRotation > 30 && zRotation <= 90)
        {
            Debug.Log("Section: 2");
        }
        else if (zRotation > 90 && zRotation <= 150)
        {
            Debug.Log("Section: 3");
        }
        else if (zRotation > 150 || zRotation <= -150)
        {
            Debug.Log("Section: 4");
        }
        else if (zRotation > -150 && zRotation <= -90)
        {
            Debug.Log("Section: 5");
        }
        else if (zRotation > -90 && zRotation < -30)
        {
            Debug.Log("Section: 6");
        }
        else
        {
            Debug.Log("Unknown Section");
        }
    }
}
