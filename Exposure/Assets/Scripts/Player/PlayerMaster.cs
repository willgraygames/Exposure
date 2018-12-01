using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMaster : MonoBehaviour
{

    //Current target variables
    public Image targetHealth;                  //Reference to current target's heatlh bar
    public GameObject cursor;                   //Reference to player's cursor
    public Text interactText;                   //Reference to the text naming the player's current target
    public float interactRange;                 //The maximum range from the player to a target to be able to interact with it
    RaycastHit hit;                             //RaycastHit for when the player hits something while attacking
    public Camera mainCamera;

    //Movement variables
    public float speed;                         //Speed at which the Player moves
    public float jumpForce;                     //Force with which the Player jumps - determines jump height
    public LayerMask heightMask;                //LayerMask to tell the player's capsule collider what is ground and what is not for the purposes of being grounded
    Vector3 movement;                           //Player's movement value stored in a Vector3
    CapsuleCollider myCollider;                 //Reference to the Player's capsule collider
    Rigidbody rb3d;                             //Reference to the Player's rigidbody
    public LayerMask dropsLayer;
    

    void Awake()
    {
        //Sets the interact text to blank
        //interactText.text = "";
        //Initializes player's collider and rigidbody references
        myCollider = GetComponent<CapsuleCollider>();
        rb3d = GetComponent<Rigidbody>();
        //Locks cursor the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        //If ESC is pressed, unlock the cursor
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Physics.Raycast(myRay, out hit, interactRange))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
            if (hit.collider.gameObject.tag == "Creature")
            {
                print("hit something");
                interactText.text = hit.collider.gameObject.name;
                cursor.SetActive(true);
            }
        }
        else
        {
            interactText.text = "";
            cursor.SetActive(false);
        }
    }

    //Player movement is handled inside of FixedUpdate due to the use of Unity's rigidbody physics system
    void FixedUpdate()
    {
        //Variables to hold the player's movement inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Calls the MovePlayer function with horizontal and vertical as parameters
        MovePlayer(horizontal, vertical);

        //If the player is grounded and presses the spacebar, call the Jump function
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();
        }
    }

    //Function to handle moving the player. Takes two float values for the horizontal and vertical inputs as parameters
    void MovePlayer(float h, float v)
    {
        //Sets the movement Vector3 with the horizonal and vertical inputs
        movement.Set(h, 0f, v);
        //Calculates movement in relation to Time.deltaTime and the speed value
        movement = movement.normalized * speed * Time.deltaTime;
        //Sets the movement to be relative to the player's rotation
        movement = transform.TransformDirection(movement);
        //Moves the player's rigidbody in the direction of the movement Vector3
        rb3d.MovePosition(transform.position + movement);
    }

    //Function to handle the player jumping
    void Jump()
    {
        //Adds a force from the player in the up direction multiplied by the jumpForce variable
        rb3d.AddForce(transform.up * jumpForce);
    }

    //Boolean function to determine if the player is grounded. 
    bool isGrounded()
    {
        //Checks a capsule around the player equal to the player's capsule collider offset down to check if the ground is beneath the player. If it is, isGrounded returns true.
        return Physics.CheckCapsule(myCollider.bounds.center, new Vector3(myCollider.bounds.center.x, myCollider.bounds.center.y - 1f, myCollider.bounds.center.z), 0.18f, heightMask.value);
    }
}
