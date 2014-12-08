using UnityEngine;
using System.Collections;

public enum Alignment {
    Ally,
    Enemy,
    Neutral
}

public class Battler : MonoBehaviour {
    public CharacterData character { get; private set; }
    public Alignment alignment { get; private set; }
    public int hp { get; private set; }
    public int stamina { get; private set; }
    public Tile tile { get; private set; }

    public void PlaceOnTile(Tile newTile) {
        if (tile != null) { // remove self from previous tile
            tile.battler = null;
        }
        var mapMesh = FindObjectOfType<TileMapMesh>();
	var surface = mapMesh.TileSurfaceCenter(newTile);
        float height = GetComponent<Collider>().bounds.size.y;
        transform.position = new Vector3(surface.x, surface.y + height / 2, surface.z);
        newTile.battler = this;
        tile = newTile;
    }

    public static Battler Create(CharacterData data, Alignment alignment, Tile startTile) {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        var battler = obj.AddComponent<Battler>();
        battler.character = data;
        battler.alignment = alignment;
        battler.PlaceOnTile(startTile);
	// REMOVE THIS LATER
        obj.renderer.material.color = alignment == Alignment.Ally ? Color.green : Color.red;
        return battler;
    }
}
