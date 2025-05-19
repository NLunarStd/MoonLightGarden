using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    float horizontalInput;
    float verticalInput;
    Vector2 inputVector;
    public FacingDirection faceDirection;
    public enum FacingDirection
    {
        Up, Down, Left, Right
    }
    public Transform targetSelection;

    public float moveSpeed;
    public Rigidbody2D rb2D;
    public bool isWalking = false;

    public Animator animator;

    public Transform playerLatern;

    public Interactor interactor;
    public Transform handPoint;

    public InputActionReference actionInteract;
    public InputActionReference actionMove;

    public UIController uiController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("XInput", inputVector.x);
        animator.SetFloat("YInput", inputVector.y);
        animator.SetBool("isWalk",isWalking);
        animator.SetFloat("LastDirectionIndex",LastestDirection);
    }

    public void OpenLatern()
    {
        playerLatern.GetComponent<Light2D>().enabled = true;
    }

    public void CloseLatern()
    {
        playerLatern.GetComponent<Light2D>().enabled = false;
    }

    private void Start()
    {
        SetUpCharacter();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    private void OnEnable()
    {
        actionInteract.action.started += ActionInteract;
    }

    private void OnDisable()
    {
        actionInteract.action.started -= ActionInteract;
    }
    public AudioClip buildSound;
    public void UseItem()
    {
        if (interactor.currentInteractable != null)
        {
            interactor.UseItem(this);
        }
        else if (uiController != null && uiController.selectingItemSlot != null)
        {
            int selectedSlotIndex = uiController.GetSelectedIndex();
            if (selectedSlotIndex >= 0 && selectedSlotIndex < uiController.playerInventory.inventorySlots.Count)
            {
                InventorySlot selectedInventorySlot = uiController.playerInventory.inventorySlots[selectedSlotIndex];
                if (selectedInventorySlot.currentItem != null)
                {
                    if (selectedInventorySlot.currentItem.GetComponent<BuildingItem>() != null)
                    {
                        uiController.PlaceBuilding();
                        GameManager.instance.soundManager.PlayOneShotWithVaryPitch(GameManager.instance.soundManager.playerSource,buildSound);
                    }
                    else
                    {
                        selectedInventorySlot.currentItem.UseItem();
                    }
                }
            }
        }
    }
    public void ActionInteract(InputAction.CallbackContext callback)
    {
        if (interactor.currentInteractable != null)
        {
            interactor.currentInteractable.UseItem(this);
            return;
        }

        if (uiController != null && uiController.selectingItemSlot != null)
        {
            int selectedSlotIndex = uiController.GetSelectedIndex();
            if (selectedSlotIndex >= 0 && selectedSlotIndex < uiController.playerInventory.inventorySlots.Count)
            {
                InventorySlot selectedInventorySlot = uiController.playerInventory.inventorySlots[selectedSlotIndex];
                if (selectedInventorySlot.currentItem != null)
                {
                    if (selectedInventorySlot.currentItem.GetComponent<BuildingItem>() != null)
                    {
                        uiController.PlaceBuilding();
                    }
                    else
                    {
                        selectedInventorySlot.currentItem.UseItem();
                    }
                }
            }
        }
    }
    public SpriteRenderer walkSmoke;
    private void Update()
    {
        GameManager.instance.playerController = this;
        GetMovementInput();
        if (inputVector != Vector2.zero)
        {
            CheckFacing();
            CheckSelectingPosition();
            isWalking = true;
            UpdateHandPoint(); 
            UpdateWeaponSortingOrder();
            LastestDirection = GetDirectionIndex();
            walkSmoke.enabled = true;
            if (inputVector.x > 0)
            {
                walkSmoke.flipX = false;
                walkSmoke.transform.localPosition = new Vector2(-0.222f, -1.543f);
            }
            else
            {
                walkSmoke.flipX = true;
                walkSmoke.transform.localPosition = new Vector2(0.222f, -1.543f);
            }
        }
        else
        {
            isWalking = false;
            walkSmoke.enabled = false;
        }
        UpdateAnimation();
    }
    int LastestDirection;
    public SpriteRenderer weaponSpriteRenderer; 
    public int weaponSortingOrderFront = 1; // sortingOrder ��������ظ�����ҹ˹��
    public int weaponSortingOrderBack = -1; // sortingOrder ��������ظ�����ҹ��ѧ
    void UpdateWeaponSortingOrder()
    {
        if (weaponSpriteRenderer == null) return;

        if (faceDirection == FacingDirection.Down || faceDirection == FacingDirection.Right) 
        {
            weaponSpriteRenderer.sortingOrder = weaponSortingOrderFront;
        }
        else 
        {
            weaponSpriteRenderer.sortingOrder = weaponSortingOrderBack;
        }
    }

    public Vector2[] handPointPositions = new Vector2[8]; 
    public float[] handPointRotations = new float[8]; 

        void UpdateHandPoint()
    {
        int directionIndex = GetDirectionIndex(); 

        if (directionIndex >= 0 && directionIndex < 8)
        {
            handPoint.localPosition = handPointPositions[directionIndex];
            handPoint.localRotation = Quaternion.Euler(0, 0, handPointRotations[directionIndex]);
        }
    }
    int GetDirectionIndex()
    {
        if (inputVector.x > 0 && inputVector.y > 0) return 0; // ��Һ�
        if (inputVector.x > 0 && inputVector.y == 0) return 1; // ���
        if (inputVector.x > 0 && inputVector.y < 0) return 2; // �����ҧ
        if (inputVector.x == 0 && inputVector.y < 0) return 3; // ��ҧ
        if (inputVector.x < 0 && inputVector.y < 0) return 4; // ������ҧ
        if (inputVector.x < 0 && inputVector.y == 0) return 5; // ����
        if (inputVector.x < 0 && inputVector.y > 0) return 6; // ���º�
        if (inputVector.x == 0 && inputVector.y > 0) return 7; // ��

        return -1; // ����շ�ȷҧ
    }

    private void FixedUpdate()
    {
        if (inputVector != Vector2.zero)
        {
            Movement();
        }
    }

    void GetMovementInput()
    {
        inputVector = actionMove.action.ReadValue<Vector2>().normalized;
    }

    void Movement()
    {
        Vector2 moveVector = inputVector * moveSpeed;
        rb2D.linearVelocity = new Vector2(moveVector.x, moveVector.y);
    }

    void SetUpCharacter()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rb2D.freezeRotation = true;
        rb2D.linearDamping = 10;
    }

    void CheckFacing()
    {
        if (inputVector.x > 0.1f)
        {
            faceDirection = FacingDirection.Right;
        }
        else if (inputVector.x < -0.1f)
        {
            faceDirection = FacingDirection.Left;
        }
        else if (inputVector.y > 0.1f)
        {
            faceDirection = FacingDirection.Up;
        }
        else if (inputVector.y < -0.1f)
        {
            faceDirection = FacingDirection.Down;
        }
    }

    void CheckSelectingPosition()
    {
        switch (faceDirection)
        {
            case FacingDirection.Right:
                targetSelection.localPosition = Vector2.right; break;
            case FacingDirection.Left:
                targetSelection.localPosition = Vector2.left; break;
            case FacingDirection.Up:
                targetSelection.localPosition = Vector2.up * 2; break;
            case FacingDirection.Down:
                targetSelection.localPosition = Vector2.down * 2; break;
        }
    }

    PlayerCharacter playerCharacter;
    public void TakeDamage(int amount)
    {
        playerCharacter.TakeDamage(amount);
    }
}