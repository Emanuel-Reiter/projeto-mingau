using UnityEngine;

public class PlayerDependencies : MonoBehaviour
{
    [HideInInspector] public PlayerInputManager Input;
    [HideInInspector] public CharacterController Controller;

    [HideInInspector] public Camera MainCamera;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public PlayerAnimationManager AnimationManager;

    [HideInInspector] public PlayerLocomotionParams LocomotionParams;

    [HideInInspector] public AttributesManager Attributes;

    [HideInInspector] public PlayerCollect Collect;
    [HideInInspector] public PlayerSimpleInventory Inventory;

    // Player actions
    [HideInInspector] public PlayerDash Dash;
    [HideInInspector] public PlayerJump Jump;
    [HideInInspector] public PlayerLand Land;
    [HideInInspector] public PlayerAttack Attack;

    private void Awake()
    {
        LoadReferences();
    }

    private void LoadReferences()
    {
        Input = GetComponent<PlayerInputManager>();
        Controller = GetComponent<CharacterController>();

        MainCamera = Camera.main;

        Animator = GetComponent<Animator>();
        AnimationManager = GetComponent<PlayerAnimationManager>();

        LocomotionParams = GetComponent<PlayerLocomotionParams>();

        Attributes = GetComponent<AttributesManager>();

        Collect = GetComponent<PlayerCollect>();
        Inventory = GetComponent<PlayerSimpleInventory>();

        Dash = GetComponent<PlayerDash>();
        Jump = GetComponent<PlayerJump>();
        Land = GetComponent<PlayerLand>();
        Attack = GetComponent<PlayerAttack>();
    }
}
