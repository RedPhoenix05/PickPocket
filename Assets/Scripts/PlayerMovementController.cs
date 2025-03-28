using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference sneakAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pauseAction;
}
