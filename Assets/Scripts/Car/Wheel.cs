using UnityEngine;


[System.Serializable]
public struct Wheel 
{
    public Transform WheelMesh;
    public WheelCollider WheelCollider;
    public bool IsForwardWheel;
    

    public void UpdateMeshPosition()
    {
        Vector3 position;
        Quaternion rotation;

        WheelCollider.GetWorldPose(out position, out rotation);
        WheelMesh.position = position;
        WheelMesh.rotation = rotation;
    }
}
