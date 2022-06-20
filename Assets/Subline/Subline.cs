using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Subline : MonoBehaviour
{
    [Header("Settings")] public Vector3Int size;
    public float unit;

#if UNITY_EDITOR
    /// <summary>
    /// Draw 3D grid base off local space matrix and handle rendering according to relative position from scene view camera.
    /// </summary>
    public void OnDrawGizmos()
    {
        var originColor = Gizmos.color;
        var originMatrix = Gizmos.matrix;
        // normalized scene camera view direction
        var sceneViewDirection = (SceneView.currentDrawingSceneView.camera.transform.position - transform.position)
            .normalized;
        //map to local space
        Gizmos.matrix = transform.localToWorldMatrix;

        var xSign = Mathf.Sign(sceneViewDirection.x);
        var ySign = Mathf.Sign(sceneViewDirection.y);
        var zSign = Mathf.Sign(sceneViewDirection.z);
        var startPoint = Vector3.zero +
                         new Vector3(
                             -xSign * size.x / 2 * unit,
                             -ySign * size.y / 2 * unit,
                             -zSign * size.z / 2 * unit);

        // use blue for x-axis
        Gizmos.color = Color.blue;
        var xEndPoint = startPoint + Vector3.up * ySign * size.x * unit;
        //var xEndPoint = startPoint + Vector3.up * ySign * size.x * unit + Vector3.right * xSign * size.x * unit;
        for (var x = 0; x < size.x; x++)
        {
            var xTemp = startPoint + Vector3.up * ySign * x * unit;
            Gizmos.DrawLine(xTemp, xTemp + Vector3.right * xSign * size.x * unit);
            Gizmos.DrawLine(startPoint + Vector3.right * xSign * x * unit,
                xEndPoint + Vector3.right * xSign * x * unit);
        }

        Gizmos.DrawLine(startPoint, startPoint + Vector3.right * xSign * size.x * unit);
        Gizmos.DrawLine(xEndPoint, xEndPoint + Vector3.right * xSign * size.x * unit);
        Gizmos.DrawLine(startPoint + Vector3.right * xSign * size.x * unit,
            xEndPoint + Vector3.right * xSign * size.x * unit);

        // use green for y-axis
        Gizmos.color = Color.red;
        var yEndPoint = startPoint + Vector3.forward * zSign * size.y * unit;
        for (var y = 0; y < size.y; y++)
        {
            var yTemp = startPoint + Vector3.forward * zSign * y * unit;
            Gizmos.DrawLine(yTemp, yTemp + Vector3.up * ySign * size.y * unit);
            Gizmos.DrawLine(startPoint + Vector3.up * ySign * y * unit, yEndPoint + Vector3.up * ySign * y * unit);
        }

        Gizmos.DrawLine(startPoint, startPoint + Vector3.up * ySign * size.y * unit);
        Gizmos.DrawLine(yEndPoint, yEndPoint + Vector3.up * ySign * size.y * unit);
        Gizmos.DrawLine(startPoint + Vector3.up * ySign * size.y * unit,
            yEndPoint + Vector3.up * ySign * size.y * unit);

        // use blue for z-axis
        Gizmos.color = Color.green;
        var zEndPoint = startPoint + Vector3.right * xSign * size.z * unit;
        for (var z = 0; z < size.z; z++)
        {
            var zTemp = startPoint + Vector3.right * xSign * z * unit;
            Gizmos.DrawLine(zTemp, zTemp + Vector3.forward * zSign * size.z * unit);
            Gizmos.DrawLine(startPoint + Vector3.forward * zSign * z * unit,
                zEndPoint + Vector3.forward * zSign * z * unit);
        }

        Gizmos.DrawLine(startPoint, startPoint + Vector3.forward * zSign * size.z * unit);
        Gizmos.DrawLine(zEndPoint, zEndPoint + Vector3.forward * zSign * size.z * unit);
        Gizmos.DrawLine(startPoint + Vector3.forward * zSign * size.z * unit,
            zEndPoint + Vector3.forward * zSign * size.z * unit);
        // rest Gizmos settings
        Gizmos.color = originColor;
        Gizmos.matrix = originMatrix;
    }
#endif
}