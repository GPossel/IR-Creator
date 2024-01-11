public interface IGameManagerSharedSetupInfo
{
    public WorldType MyWorldType { get; set; }
    public GameMode MyGameMode { get; set; }
    public VisualStyleTypes MyCollectionType { get; set; }
    public int LevelNumber { get; set; }
    public int RunCounter { get; set; }
}