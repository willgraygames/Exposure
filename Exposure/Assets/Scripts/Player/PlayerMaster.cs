using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMaster : MonoBehaviour
{

    //Current target variables
    public GameObject cursor;                   //Reference to player's cursor
    public Text interactText;                   //Reference to the text naming the player's current target
    public float interactRange;                 //The maximum range from the player to a target to be able to interact with it
    RaycastHit hit;                             //RaycastHit for when the player hits something while attacking
    public Camera mainCamera;

    //Movement variables
    public float speed;                         //Speed at which the Player moves
    Vector3 movement;                           //Player's movement value stored in a Vector3
    CapsuleCollider myCollider;                 //Reference to the Player's capsule collider
    Rigidbody rb3d;                             //Reference to the Player's rigidbody
    public LayerMask dropsLayer;

    //Inventory
    List<GameObject> myInventory = new List<GameObject>();
    GameObject currentHoverObject;
    

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
            if (hit.collider.gameObject.tag == "Pit")
            {
                print("hit something");
                interactText.text = "Feed the Pit";
                cursor.SetActive(true);
                myInventory.RemoveAll(item => item == null);
                if (Input.GetMouseButtonDown(0) && myInventory.Count > 0)
                {
                    //Feed the pit action
                    print("shit yea go pit boi");
                }
            } else if (hit.collider.tag == "Fire")
            {
                interactText.text = "Feed the Fire";
                cursor.SetActive(true);
                myInventory.RemoveAll(item => item == null);
                if (Input.GetMouseButtonDown(0) && myInventory.Count > 0)
                {
                    //Feed the fire action
                    print("shit yea feed that fire boi");
                }
            } else if (hit.collider.gameObject.tag == "Item")
            {
                interactText.text = hit.collider.gameObject.GetComponent<Item>().itemName;
                cursor.SetActive(true);
                currentHoverObject = hit.collider.gameObject;
                if (Input.GetMouseButtonDown(0))
                {
                    AddToInventory(hit.collider.gameObject);
                }
            }
        }
        else
        {
            interactText.text = "";
            cursor.SetActive(false);
            currentHoverObject = null;
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

    public void AddToInventory(GameObject item)
    {
        myInventory.Add(item);
        item.SetActive(false);
        currentHoverObject = null;
        interactText.text= "";
        cursor.SetActive(false);
    }
}
