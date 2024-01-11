public class GameManagerSharedSetupInfo : IGameManagerSharedSetupInfo
{
    public WorldType MyWorldType = WorldType.None;
    public GameMode MyGameMode = GameMode.None;
    public VisualStyleTypes MyCollectionType = VisualStyleTypes.None;
    public int levelNumber = 0;
    public int runCounter = 0;

    WorldType IGameManagerSharedSetupInfo.MyWorldType { get => MyWorldType; set => MyWorldType = value; }
    GameMode IGameManagerSharedSetupInfo.MyGameMode { get => MyGameMode; set => MyGameMode = value; }
    VisualStyleTypes IGameManagerSharedSetupInfo.MyCollectionType { get => MyCollectionType; set => MyCollectionType = value; }
    int IGameManagerSharedSetupInfo.LevelNumber { get => levelNumber; set => levelNumber = value; }
    int IGameManagerSharedSetupInfo.RunCounter { get => runCounter; set => runCounter += value; }
}