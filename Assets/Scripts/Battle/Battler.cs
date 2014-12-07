using UnityEngine;
using System.Collections;

public class Battler : MonoBehaviour {
    public CharacterData character { get; private set; }
    public int hp { get; private set; }
    public int stamina { get; private set; }
    public Tile tile { get; private set; }

    public void PlaceOnTile(Tile tile) {
        var mapMesh = FindObjectOfType<TileMapMesh>();
	var surface = mapMesh.TileSurfaceCenter(tile);
        float height = GetComponent<Collider>().bounds.size.y;
        transform.position = new Vector3(surface.x, surface.y + height / 2, surface.z);
    }

    public static Battler Create(CharacterData data, Tile startTile) {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        var battler = obj.AddComponent<Battler>();
        battler.character = data;
        battler.PlaceOnTile(startTile);
        return battler;
    }
}
