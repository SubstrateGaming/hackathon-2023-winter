using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.ScreenStates;
using UnityEngine;

internal class TileShaders
{
    private static Renderer _tile;

    // Start is called before the first frame update
    public static void StartUpgradeTileShader(ScreenBaseState parent)
    {
        Material bestShader = Resources.Load<Material>("Shaders/BestShader");

        _tile = GridManager.GetInstance().PlayerGrid.transform.GetChild((parent as PlayScreenState).SelectedGridIndex).GetChild(0).GetComponent<Renderer>();

        _tile.SetMaterials(
            new List<Material>() {
                _tile.material,
                bestShader
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
