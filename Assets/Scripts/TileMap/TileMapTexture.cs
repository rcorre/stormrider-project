using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMapTexture : MonoBehaviour {
    public List<int> elevationThresholds;
    public int frameSize;

    public Vector2[] elevationToUv(int elevation) {
        var tex = renderer.sharedMaterial.mainTexture;
        int numCols = tex.width / frameSize;
        int numRows = tex.height / frameSize;
        int idx = elevationThresholds.FindLastIndex(x => x <= elevation);

        int row = idx / numCols;
        int col = idx % numCols;

        float top	= (float)row	    / numRows;
        float bottom	= (float)(row + 1)  / numRows;
        float left	= (float)col	    / numCols;
        float right	= (float)(col + 1)  / numCols;

        return new Vector2[] { 
	    new Vector2(left, top), 
	    new Vector2(right, top), 
	    new Vector2(left, bottom), 
	    new Vector2(right, bottom) 
        };
    }
}
