using UnityEngine;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
    public string dataName;
    public List<string> playerCharacters;
    private List<Battler> _npcs;
    private List<Battler> _playerParty;
    private TileMap _map;

    void Start() {
	_map = FindObjectOfType<TileMap>();
	_map.GenerateMap();
        var battleData = DataManager.LoadOnce<BattleData>(dataName);
        _npcs = new List<Battler>();
        foreach (var npc in battleData.npcs) {
	    var npcData = battleData.npcData[npc.index];
	    var tile = _map.TileAt(npc.row, npc.col);
	    _npcs.Add(Battler.Create(npcData, tile));
        }
    }
}
