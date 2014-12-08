﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TileOverlay : MonoBehaviour {
    public enum HighlightType {
	Move,
	Attack,
    }

    enum IconMode {
	None,
	Walk,
	Melee,
	Ranged
    }

    public Transform FocusOverlay;
    public GameObject MovementOverlay;
    public GameObject AttackOverlay;
    public GameObject WalkIcon, MeleeIcon, RangedIcon;
    public float PathElevation = 0.5f; // how far above tiles to draw paths

    private List<GameObject> _currentOverlay = new List<GameObject>();
    private GameObject _mouseIcon;
    private IconMode _iconMode;
    private LineRenderer _lineRenderer; // for drawing move paths
    private TileMapMesh _tileMapMesh;

    void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _tileMapMesh = FindObjectOfType<TileMapMesh>();
    }

    public void HighlightTiles(IEnumerable<Tile> tiles, HighlightType type) {
        ClearHighlight();
        var prefab = (type == HighlightType.Move) ? MovementOverlay : AttackOverlay;
        foreach (var tile in tiles) {
            var overlay = (GameObject)Instantiate(prefab);
            overlay.transform.position = _tileMapMesh.TileSurfaceCenter(tile);
            overlay.transform.parent = transform;
            _currentOverlay.Add(overlay);
        }
    }

    public void DrawPath(IEnumerable<Tile> tiles) {
        _lineRenderer.SetVertexCount(tiles.Count());
        int vertex = 0;
        foreach (var tile in tiles) {
            var pos = _tileMapMesh.TileSurfaceCenter(tile) + Vector3.up * PathElevation;
            _lineRenderer.SetPosition(vertex++, pos);
        }
    }

    public void ShowFocus(Tile tile) {
        FocusOverlay.gameObject.SetActive(true);
        FocusOverlay.position = _tileMapMesh.TileSurfaceCenter(tile);
    }

    public void DisplayWalkIcon(int moveCost) {
        SetIcon(IconMode.Walk);
        _mouseIcon.guiText.text = moveCost.ToString();
    }

    public void DisplayMeleeIcon(int moveCost, int damage) {
        SetIcon(IconMode.Melee);
        _mouseIcon.guiText.text = "Cost: " + moveCost + "\nDamage:" + damage;
    }

    void Update() {
        if (_mouseIcon) {
            var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            _mouseIcon.transform.position = mousePos;
        }
    }

    public void ClearIcon() {
        SetIcon(IconMode.None);
    }

    public void ClearHighlight() {
        foreach (var overlay in _currentOverlay) {
            Destroy(overlay);
        }
        _currentOverlay.Clear();
    }

    public void ClearPath() {
        _lineRenderer.SetVertexCount(0);
    }

    public void ClearFocus() {
        FocusOverlay.gameObject.SetActive(false);
    }

    public void ClearAll() {
        ClearHighlight();
        ClearIcon();
        ClearPath();
        ClearFocus();
    }

    private void SetIcon(IconMode mode) {
	if (_iconMode == mode) { return; } // no work to do
	// set new mode
        _iconMode = mode;
        if (_mouseIcon) { // destroy old icon
            Destroy(_mouseIcon);
            _mouseIcon = null;
        }
	// instantiate new icon
        switch (mode) {
            case IconMode.None:
                break;
            case IconMode.Walk:
                _mouseIcon = (GameObject) GameObject.Instantiate(WalkIcon);
                break;
            case IconMode.Melee:
                _mouseIcon = (GameObject) GameObject.Instantiate(MeleeIcon);
                break;
            case IconMode.Ranged:
                _mouseIcon = (GameObject) GameObject.Instantiate(RangedIcon);
                break;
            default:
                break;
        }
    }
}
