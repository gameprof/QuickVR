using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickDimArray<T> {
    [SerializeField]
    private T[]   _array;
    [SerializeField]
    private int   _totalSize = 0;
    [SerializeField]
    private int   _dims      = 0;
    [SerializeField]
    private int[] _sizes;
    [SerializeField]
    private int[] _multipliers;
    [SerializeField]
    private bool _isValid = false;
    public bool isValid => _isValid;
    [Tooltip("This will check each index for validity, and log an error if any index is out of bounds. Bypassing this can increase performance.")]
    public bool checkEachIndex = false;
    
    public QuickDimArray(params int[] sizes) {
        _isValid = false;
        _sizes = sizes;
        _dims = sizes.Length;
        _multipliers = new int[_dims];
        _totalSize = 1;
        for ( int i = 0; i < sizes.Length; i++ ) {
            int s = sizes[i];
            if ( s <= 0 ) {
                // This is not a valid length for an array dimension.
                Debug.LogError($"Invalid dimension of {s} passed into QuickDimArray constructor: {sizes} !"  );
                return;
            }
            _multipliers[i] = _totalSize;
            _totalSize *= s;
        }
        _array = new T[_totalSize];
        _isValid = true;
    }
    
    public T this[params int[] indices] {
        get {
            if (!_isValid) {
                Debug.LogError("QuickDimArray is not valid, cannot access elements!");
                return default(T);
            }
            int index = GetIndex(indices);
            if ( index < 0 ) {
                return default(T);
            }
            return _array[index];
        }
        set {
            if (!_isValid) {
                Debug.LogError("QuickDimArray is not valid, cannot set elements!");
                return;
            }
            int index = GetIndex(indices);
            if ( index < 0 ) {
                return;
            }
            _array[index] = value;
        }
    }
    
    private int GetIndex(params int[] indices) {
        if ( indices.Length != _dims ) {
            Debug.LogError($"Invalid number of indices passed into QuickDimArray: {indices.Length} != {_dims} !"  );
            return -1;
        }
        int index = 0;
        if ( checkEachIndex ) {
            for ( int i = 0; i < indices.Length; i++ ) {
                int s = indices[i];
                if ( s < 0 || s >= _sizes[i] ) {
                    Debug.LogError( $"Invalid index {s} for dimension {i} passed into QuickDimArray: {indices} !" );
                    return-1;
                }
                index += s * _multipliers[i];
            }
        } else {
            for ( int i = 0; i < indices.Length; i++ ) {
                index += indices[i] * _multipliers[i];
            }
            if (index < 0 || index >= _totalSize) {
                Debug.LogError( $"Invalid index {index} for dimensions {indices} passed into QuickDimArray: {indices} !" );
                return -1;
            }
        }
        return index;
    }
    
    
    #region Bezier Curve QuickDimArray Example

    public static Vector3 BezierCurve( float u, params Vector3[] points ) {
        int len = points.Length;
        if ( len == 0 ) {
            Debug.LogError( "BezierCurve requires at least 1 point!" );
            return Vector3.zero;
        } else if ( len == 1 ) {
            return points[0];
        } else if ( len == 2 ) {
            return( 1 - u ) * points[0] + u * points[1];
        }

        // Use a QuickDimArray to store the intermediate points when Bezier interpolation is actually needed
        // Create a QuickDimArray<Vector3> to interpolate over
        QuickDimArray<Vector3> qDA = new QuickDimArray<Vector3>( len, len );
        int row = len - 1;
        // Copy the points into the last row of the array
        for ( int i = 0; i < len; i++ ) {
            qDA[row, i] = points[i];
        }
        // Start interpolating downward
        for ( ; row > 0; row-- ) {
            for ( int col = 0; col < row; col++ ) {
                qDA[row - 1, col] = ( 1 - u ) * qDA[row, col] + u * qDA[row, col + 1];
            }
        }

        // Return the final interpolated value
        return qDA[0, 0];
    }

    /* -- This is the original code that was replaced by the QuickDimArray implementation above
    Vector3 InterpolateArray( Vector3[] points, float u ) {
        // Create a 2D Vector3[] to interpolate over
        Vector3[,] arr = new Vector3[points.Length, points.Length];
        int row = points.Length - 1;
        // Copy the points into the last row of the array
        for (int i = 0; i < points.Length; i++) {
            arr[row, i] = points[i];
        }
        // Start interpolating downward
        for (; row > 0; row--) {
            for (int col = 0; col < row; col++) {
                arr[row - 1, col] = (1 - u) * arr[row, col] + u * arr[row, col + 1];
            }
        }
         
        // Return the final interpolated value
        return arr[0, 0];
    }
    */
    
    #endregion
}
