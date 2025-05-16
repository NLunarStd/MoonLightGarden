using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactor : MonoBehaviour
{
    public Tilemap tilemap;
    Vector3Int focusedCell;
    public TileBase highlightTile;
    Vector3Int previousCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    public Interactable currentInteractable;
    public virtual void UseItem(PlayerController player)
    {
        currentInteractable?.UseItem(player);
    }

    private void Update()
    {
        GetCell();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Structure"))
        {
            currentInteractable = collision.GetComponent<Interactable>();
            Debug.Log(currentInteractable.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentInteractable = null;
    }
    void GetCell()
    {
        if (tilemap != null)
        {
            Vector3 worldPosition = transform.position;
            Vector3Int focusedCell = tilemap.WorldToCell(worldPosition);

            if (focusedCell != previousCell)
            {
                ClearHighlight();
                HighlightCell(focusedCell);
                previousCell = focusedCell; 
               // Debug.Log("Focusing cell: " + focusedCell);
            }
        }
    }

    void HighlightCell(Vector3Int cellPosition)
    {
        if (tilemap != null)
        {
            tilemap.SetTile(cellPosition, highlightTile);
        }
    }
    void ClearHighlight()
    {
        if(tilemap != null && previousCell != new Vector3Int(int.MinValue, int.MinValue, int.MinValue))
        {
            tilemap.SetTile(previousCell, null);
        }
    }

}
