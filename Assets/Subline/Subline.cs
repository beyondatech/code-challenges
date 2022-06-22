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
        var xDirection = Vector3.right * xSign;
        var ySign = Mathf.Sign(sceneViewDirection.y);
        var yDirection = Vector3.up * ySign;
        var zSign = Mathf.Sign(sceneViewDirection.z);
        var zDirection = Vector3.forward * zSign;
        var startPoint = Vector3.zero -
                         new Vector3(
                             xSign * size.x / 2 * unit,
                             ySign * size.y / 2 * unit,
                             zSign * size.z / 2 * unit);

        GenerateGrid(Color.blue, yDirection, xDirection, startPoint, size.x, unit);
        GenerateGrid(Color.red, zDirection, yDirection, startPoint, size.y, unit);
        GenerateGrid(Color.green, xDirection, zDirection, startPoint, size.z, unit);

        // rest Gizmos settings
        Gizmos.color = originColor;
        Gizmos.matrix = originMatrix;
    }

    /// <summary>
    /// Generate grid for one dimension. 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="verticalDirection"></param>
    /// <param name="horizontalDirection"></param>
    /// <param name="startingPosition"></param>
    /// <param name="size"></param>
    /// <param name="unit"></param>
    private void GenerateGrid(Color color, Vector3 verticalDirection, Vector3 horizontalDirection,
        Vector3 startingPosition, int size, float unit)
    {
        Gizmos.color = color;
        for (var i = 0; i <= size; i++)
        {
            //Draw horizontal lines
            Gizmos.DrawLine(
                startingPosition + horizontalDirection * i * unit,
                startingPosition + (verticalDirection * size + horizontalDirection * i) * unit);
            //Draw vertical lines
            Gizmos.DrawLine(
                startingPosition + verticalDirection * i * unit,
                startingPosition + (verticalDirection * i + horizontalDirection * size) * unit
            );
        }
    }
#endif
}