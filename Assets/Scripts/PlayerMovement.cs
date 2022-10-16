using UnityEngine;

public class PlayerMovement : Player {
    public float movementSpeed = 5f;
    public float mouseSpeed = 2f;
    public float sprintMultiplier = 2f;
    public Transform gun;

    public Camera playerCam;
    private float initHeight;
    private Vector3 mouseRotation = Vector2.zero;

    private void Awake() {
        playerCam = GetComponentInChildren<Camera>();
        initHeight = transform.position.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
        }
    }

    private void Update() {
        if (!IsOwner) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }

        playerCam.depth = 5f;

        //Clamp y pos
        transform.position = new Vector3(transform.position.x, initHeight, transform.position.z);

        Move();
        Look();
    }

    private void Move() {
        if (Input.GetKey("w")) {
            transform.position += transform.forward * (Time.deltaTime * movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f));
        }

        if (Input.GetKey("s")) {
            transform.position -= transform.forward * (Time.deltaTime * movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f));
        }

        if (Input.GetKey("a")) {
            transform.position -= transform.right * (Time.deltaTime * movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f));
        }

        if (Input.GetKey("d")) {
            transform.position -= -transform.right * (Time.deltaTime * movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f));
        }
    }

    private void Look() {
        mouseRotation.y += Input.GetAxis("Mouse X");
        mouseRotation.x += -Input.GetAxis("Mouse Y");
        mouseRotation.x = Mathf.Clamp(mouseRotation.x, -15f, 15f);
        transform.eulerAngles = new Vector2(0, mouseRotation.y) * mouseSpeed;
        gun.transform.localRotation = Quaternion.Euler(mouseRotation.x * mouseSpeed, 0, 0);
        playerCam.transform.localRotation = Quaternion.Euler(mouseRotation.x * mouseSpeed, 0, 0);
    }
}