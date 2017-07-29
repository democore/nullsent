using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of basic utility methods that may be reused.
/// </summary>
public static class Util {

    /// <summary>
    /// Checks whether or not the layermask contains the layer given.
    /// </summary>
    /// <param name="layer">The layer to check for.</param>
    /// <param name="layerMask">The layermask to check the layer against to see if it contains the layer.</param>
    /// <returns>Whether or not the layer mask contains the layer.</returns>
	public static bool IsLayerInMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    public static List<T> CopyList<T>(List<T> toCopy)
    {
        List<T> copiedList = new List<T>();

        copiedList.AddRange(toCopy.ToArray());

        return copiedList;
    }

    /// <summary>
    /// Converts Vector Rotation to an angle Rotation. So rot.x and rot.y become a rotation z value.
    /// </summary>
    /// <param name="rotation">The rotation Vector to be converted.</param>
    /// <returns>The Z rotation in euler.</returns>
    public static float VectorRotationToZRotation(Vector2 rotation)
    {
        return Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg + 90f;
    }
}
