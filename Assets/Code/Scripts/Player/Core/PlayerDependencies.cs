using UnityEngine;

public class PlayerDependencies : MonoBehaviour
{
    [HideInInspector] public PlayerInputManager Input;
    [HideInInspector] public CharacterController Controller;

    [HideInInspector] public Camera MainCamera;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public PlayerAnimationManager AnimationManager;

    [HideInInspector] public PlayerLocomotionParams LocomotionParams;

    [HideInInspector] public GlobalTimer GlobalTimer;

    // Player actions
    [HideInInspector] public PlayerDash Dash;
    [HideInInspector] public PlayerJump Jump;

    private void Awake()
    {
        LoadReferences();
    }

    private void LoadReferences()
    {
        try
        {
            Input = GetComponent<PlayerInputManager>();
            Controller = GetComponent<CharacterController>();

            MainCamera = Camera.main;

            Animator = GetComponent<Animator>();
            AnimationManager = GetComponent<PlayerAnimationManager>();

            LocomotionParams = GetComponent<PlayerLocomotionParams>();

            GlobalTimer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<GlobalTimer>();

            Dash = GetComponent<PlayerDash>();
            Jump = GetComponent<PlayerJump>();
        }
        catch 
        {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }

    }
}
