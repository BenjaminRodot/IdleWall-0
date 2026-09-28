public struct WasteData
{
    public WasteType Type;
    public byte Hardness;
    public int ReservedByRobotId;

    public bool Exists => Type != WasteType.None;
}