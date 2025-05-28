using UnityEngine;

public class MoonflowerWorkSpaceTrigger : MonoBehaviour
{
    public Transform moonflowerCanvas;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            moonflowerCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moonflowerCanvas.gameObject.SetActive(false);
        }
    }
}
