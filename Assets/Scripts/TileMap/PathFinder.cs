using System.Collections.Generic;
using System.Linq;

public class PathFinder {
    public Tile[] TilesInRange { get; private set; }
    private int[] _distance;
    private int[] _parent;
    private int _sourceRow, _sourceCol;
    private TileMap _tileMap;

    public PathFinder(TileMap tileMap, Battler battler) {
        _sourceRow = battler.tile.row;
        _sourceCol = battler.tile.col;
        _tileMap = tileMap;
        int numNodes = tileMap.numRows * tileMap.numCols;
        _distance = new int[numNodes];
        for (int i = 0; i < numNodes; i++) {
            _distance[i] = int.MaxValue;
        }
	int startIndex = TileToIndex(battler.tile);
        _distance[startIndex] = 0;

        _parent = new int[numNodes];

        var queue = new List<int>();
        queue.Add(startIndex);
        while (queue.Count > 0) {
	    var u = queue.OrderBy(a => _distance[a]).First();
	    queue.Remove(u);

	    var tile = IndexToTile(u);
	    foreach (var v in tileMap.TileNeighbors(tile)) {
		int alt = _distance[u] + battler.MoveCost(v);
		int idx = TileToIndex(v);
		if (alt < _distance[idx]) {
		    _distance[idx] = alt;
		    _parent[idx] = u;
		    queue.Add(idx);
		}
	    }
        }

        List<Tile> tilesInRange = new List<Tile>();
        for (int i = 0; i < numNodes; i++) {
            if (_distance[i] <= battler.MoveRange && i != startIndex) {
                tilesInRange.Add(IndexToTile(i));
            }
        }
        TilesInRange = tilesInRange.ToArray();
    }

    public int CostToTile(Tile tile) {
        var idx = TileToIndex(tile);
        if (idx < _distance.Length) {
            return _distance[idx];
        }
        return int.MaxValue;
    }

    public Stack<Tile> PathToTile(Tile endTile) {
        int idx = TileToIndex(endTile);
        if (_distance[idx] == int.MaxValue) {
            return null;
        }
        else {
            var route = new Stack<Tile>();
            var startIdx = CoordsToIndex(_sourceRow, _sourceCol);
	    while (idx != startIdx) {
		route.Push(IndexToTile(idx));
		idx = _parent[idx];
	    }
	    return route;
        }
    }

    private int TileToIndex(Tile tile) {
        return CoordsToIndex(tile.row, tile.col);
    }

    private int CoordsToIndex(int row, int col) {
        return row * _tileMap.numCols + col;
    }

    private Tile IndexToTile(int idx) {
        return _tileMap.TileAt(idx / _tileMap.numRows, idx % _tileMap.numCols);
    }
}
