using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public RoomManager[] rooms;

    public GameObject player;
    public Transform playerFollower;

    public ContactFilter2D contactFilter2D;

    [Tooltip("First parameter - old room, Second - new room")]
    public UnityEvent<RoomManager, RoomManager> onRoomChange;


    private readonly List<Collider2D> overlappedPlayerPositionPoints = new();

    private RoomManager currentRoom = null;
    private int currentRoomIndex;


    public bool IsCurrentRoom(RoomManager roomManager)
    {
        return roomManager == currentRoom;
    }


    private int RoomIndex(RoomManager roomManager)
    {
        int index = Array.IndexOf(rooms, roomManager);

        if (index == -1)
        {
            Debug.LogError("Cannot find room");
            return rooms.Length;
        }

        return index;
    }

    private void InitRooms()
    {
        foreach (RoomManager roomManager in rooms)
        {
            roomManager.RegisterLevelManager(this);
        }
    }


    private void Awake()
    {
        InitRooms();

        RoomConnector.Connect(rooms[0], RoomManager.RoomSide.BottomRight, rooms[1], RoomManager.RoomSide.BottomLeft);
        RoomConnector.Connect(rooms[1], RoomManager.RoomSide.TopLeft, rooms[2], RoomManager.RoomSide.BottomRight);
        RoomConnector.Connect(rooms[1], RoomManager.RoomSide.TopRight, rooms[3], RoomManager.RoomSide.BottomLeft);
        RoomConnector.Connect(rooms[1], RoomManager.RoomSide.BottomRight, rooms[4], RoomManager.RoomSide.BottomLeft);
    }

    private void Update()
    {
        Vector2 playerPosition = player.transform.position;

        int collisionAmount = Physics2D.OverlapPoint(playerPosition, contactFilter2D, overlappedPlayerPositionPoints);

        if (collisionAmount == 0)
        {
            Debug.LogError("Player out of any rooms");
            return;
        }
        if (collisionAmount > 1)
        {
            Debug.LogError("Player in more than one room");
            return;
        }

        if (overlappedPlayerPositionPoints[0].TryGetComponent(out LinkToRoomManager linkToRoomManager))
        {
            RoomManager roomManager = linkToRoomManager.roomManager;

            if (roomManager == currentRoom)
            {
                return;
            }

            onRoomChange.Invoke(currentRoom, roomManager);
            roomManager.PlayerEnterToRoom(playerFollower);

            if (currentRoom)
            {
                currentRoom.cinemachineCamera.SetActive(false);
            }
            roomManager.cinemachineCamera.SetActive(true);

            currentRoom = roomManager;
            currentRoomIndex = RoomIndex(roomManager);
        }
        else
        {
            Debug.LogError("Game Object with Room Shape layout does not contain Link To Room Manager");
            return;
        }
    }
}
