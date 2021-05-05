
public static class RoomConnector
{
    public static void Connect(
        RoomManager anchorRoom,
        RoomManager.RoomSide anchorSide,
        RoomManager toConnectRoom,
        RoomManager.RoomSide toConnectSide)
    {
        anchorRoom.InitNeighbourLoaders();
        toConnectRoom.InitNeighbourLoaders();

        NextRoomLoader anchorLoader = anchorRoom.neighbourLoaders[(int) anchorSide];
        NextRoomLoader toConnectLoader = toConnectRoom.neighbourLoaders[(int) toConnectSide];

        anchorRoom.neighbours[(int) anchorSide] = toConnectRoom;
        toConnectRoom.neighbours[(int) toConnectSide] = anchorRoom;

        toConnectRoom.transform.position =
            anchorRoom.transform.position +
            anchorLoader.transform.localPosition -
            toConnectLoader.transform.localPosition;
    }
}
