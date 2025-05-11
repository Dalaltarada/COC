using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 30)]
    private float movement_speed = 10;

    [SerializeField]
    [Range(1f, 3f)]
    private float runMultiplier = 1.92f; 

    [SerializeField]
    private Camera player_camera;

    private CharacterController character_controller;

    private float vertical_velocity;
    private float jump_height = 0.15f;
    private float gravity_scale = 0.03f;

    public bool canMove = true; // ✅ Movement control flag

    public bool is_player_moving { get; private set; }

    public bool is_player_grounded
    {
        get { return character_controller.isGrounded; }
    }

    void Start()
    {
        character_controller = GetComponent<CharacterController>();
    }

    public void movePlayer(Vector3 direction, Quaternion look, bool jump)
    {
        if (!canMove) return; // ⛔ Don't move if disabled

        // ✅ Use Shift key for running
        float speed = Input.GetKey(KeyCode.LeftShift) ? movement_speed * runMultiplier : movement_speed;
        Vector3 move_vector = new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;

        // Apply rotation based on look direction
        transform.rotation = Quaternion.Euler(look.eulerAngles.x, look.eulerAngles.y, 0);
        move_vector = transform.rotation * move_vector;

        // Gravity and jumping
        addPlayerGravity();

        if (jump && is_player_grounded)
        {
            setJump(jump_height);
        }

        is_player_moving = move_vector.magnitude > 0;
        Vector3 character_move = new Vector3(move_vector.x, vertical_velocity, move_vector.z);
        character_controller.Move(character_move);
    }

    private void addPlayerGravity()
    {
        if (!character_controller.isGrounded)
        {
            vertical_velocity += Physics.gravity.y * Time.deltaTime * gravity_scale;
        }
        else if (vertical_velocity < 0)
        {
            vertical_velocity = -1f; // Keeps grounded nicely
        }
    }

    public void setJump(float lift)
    {
        if (character_controller.isGrounded)
        {
            vertical_velocity = lift;
        }
    }

    public void LockControl()
    {
        SetControlLock(true);
    }

    public void UnlockControl()
    {
        SetControlLock(false);
    }

    public void SetControlLock(bool isLocked)
    {
        canMove = !isLocked;
        Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isLocked;
    }
}
