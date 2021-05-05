using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NextRoomLoader : MonoBehaviour
{
    public LayerMask availableToRunLoaderMask;


    private RoomManager roomManager;


    public void RegisterRoomManager(RoomManager manager)
    {
        roomManager = manager;
    }

    private void HandleCollision(Collider2D collision, bool isExit)
    {
        if ((availableToRunLoaderMask.value & 1 << collision.gameObject.layer) != 0)
        {
            if (isExit)
            {
                roomManager.UnloadNeighbour(this);
            }
            else
            {
                roomManager.LoadNeighbour(this);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision, false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision, false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleCollision(collision, true);
    }
}
