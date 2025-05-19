using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimation : MonoBehaviour
{
    public RectTransform uiToMoveOut;
    public RectTransform uiToMoveIn;
    public float moveDistance = 2500f;
    public float moveDuration = 0.5f;
    bool isMoveToCharacterState = false;

    private Vector2 uiToMoveOutOriginalPosition;
    private Vector2 uiToMoveInOriginalPosition;
    private bool isMoving = false;

    public LobbyUISoundControl lobbyUISoundControl;
    private void Start()
    {
        uiToMoveOutOriginalPosition = uiToMoveOut.anchoredPosition;
        uiToMoveInOriginalPosition = uiToMoveIn.anchoredPosition;
        lobbyUISoundControl = GetComponent<LobbyUISoundControl>();
    }

    public void MoveUIOutAndIn()
    {
        lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.clickSound);
        if (isMoving) return;
        if (isMoveToCharacterState) return;
        StartCoroutine(MoveUI(uiToMoveOut, -moveDistance, uiToMoveIn, -moveDistance));
        isMoveToCharacterState = true; 
    }

    public void MoveUIBack()
    {
        lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.clickSound);
        if (isMoving) return;
        StartCoroutine(MoveUI(uiToMoveOut, moveDistance, uiToMoveIn, moveDistance));
        isMoveToCharacterState = false;
    }

    private IEnumerator MoveUI(RectTransform moveOut, float moveOutXOffset, RectTransform moveIn, float moveInXOffset)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector2 moveOutStartPosition = moveOut.anchoredPosition;
        Vector2 moveInStartPosition = moveIn.anchoredPosition;
        Vector2 moveOutEndPosition = new Vector2(moveOutStartPosition.x + moveOutXOffset, moveOutStartPosition.y);
        Vector2 moveInEndPosition = new Vector2(moveInStartPosition.x + moveInXOffset, moveInStartPosition.y);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);

            moveOut.anchoredPosition = Vector2.Lerp(moveOutStartPosition, moveOutEndPosition, t);
            moveIn.anchoredPosition = Vector2.Lerp(moveInStartPosition, moveInEndPosition, t);

            yield return null;
        }

        isMoving = false;
    }
}