using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = Vector3.up * 2;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private float followDistance = -5f;

    private Rigidbody playerRigidbody;

    private void Start()
    {
        transform.parent = null;
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned.");
            enabled = false;
            return;
        }

        playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component is not found on the player.");
            enabled = false;
        }
    }

  
    private void FixedUpdate()
    {
        if (playerRigidbody == null || playerTransform == null) return;

        Vector3 playerForward = CalculatePlayerForward();
        Vector3 targetPosition = CalculateTargetPosition(playerForward);

        MoveCamera(targetPosition);
        LookAtPlayer();
    }

    private Vector3 CalculatePlayerForward()
    {
        return (playerRigidbody.velocity + playerTransform.forward).normalized;
    }

    private Vector3 CalculateTargetPosition(Vector3 playerForward)
    {
        return playerTransform.position
               + playerTransform.TransformVector(offset)
               + playerForward * followDistance;
    }

    private void MoveCamera(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(playerTransform);
    }
}
