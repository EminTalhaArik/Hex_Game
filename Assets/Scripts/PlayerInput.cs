using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInput : MonoBehaviour
{

    private Vector2 movementInput;
    private Vector3 direction;
    public Tilemap fogOfWar;

    public bool hasMoved;
    public bool okey = false;

    public Camera cameraObject;

    public GameObject controlObject;
    public LayerMask controlLayer;

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"), Input.GetAxis("Vertical"));

        if (movementInput.x == 0)
        {
            hasMoved = false;
        }
        else if (movementInput.x != 0 && !hasMoved)
        {
            hasMoved = true;
            GetMovementDirectory();
        }
        MoveToMousePosition();
    }

    public void GetMovementDirectory()
    {
        if (movementInput.x < 0)
        {
            if (movementInput.y > 0)
            {
                direction = new Vector3(-0.5f, 0.5f);
            }
            else if (movementInput.y < 0)
            {
                direction = new Vector3(-0.5f, -0.5f);
            }
            else
            {
                direction = new Vector3(-1f, 0, 0);
            }
            transform.position += direction;
            UpgradeFogOfWar();
        }
        else if (movementInput.x > 0)
        {
            if (movementInput.y > 0)
            {
                direction = new Vector2(0.5f, 0.5f);
            }
            else if (movementInput.y < 0)
            {
                direction = new Vector2(0.5f, -0.5f);
            }
            else
            {
                direction = new Vector3(1, 0, 0);
            }

            transform.position += direction;
            UpgradeFogOfWar();
        }
    }

    public int vision = 1;

    public void UpgradeFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(transform.position);

        for (int x = -vision; x <= vision; x++)
        {
            for (int y = -vision; y <= vision; y++)
            {
                fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
            }
        }
    }

    public Vector3 mousePos;
    public void MoveToMousePosition()
    {
        if (Input.GetMouseButton(0) && okey == false)
        {
            mousePos = cameraObject.ScreenToWorldPoint(Input.mousePosition);
            okey = SendControlObject(mousePos);
            Debug.Log(mousePos);
        }
        MoveToPosition();
    }

    public GameObject playerControlObject;
    public float speed = 1;
    public void MoveToPosition()
    {
        if (okey)
        {
            if (transform.position == new Vector3(mousePos.x, mousePos.y, 0) || Physics2D.OverlapCircle(playerControlObject.transform.position, 0.2f, controlLayer))
            {
                okey = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, mousePos.y, 0), Time.deltaTime * speed);
            UpgradeFogOfWar();

        }
    }

    public bool SendControlObject(Vector3 position)
    {
        controlObject.transform.position = position;
        if (Physics2D.OverlapCircle(controlObject.transform.position, 0.2f, controlLayer))
        {
            Debug.Log("Bak obje yasaklý alana denk geldi!");
            return false;
        }

        return true;
    }
}
