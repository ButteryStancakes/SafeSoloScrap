namespace SafeSoloScrap
{
    public static class ScrapSafe
    {
        static readonly string[] drillPieces = { "LungApparatus" };

        public static bool IsItemLost(GrabbableObject grabObj)
        {
            return grabObj.itemProperties.isScrap && (StartOfRound.Instance.connectedPlayersAmount > Plugin.configMaxPlayers.Value - 1 || !grabObj.scrapPersistedThroughRounds || System.Array.Exists(drillPieces, drillPiece => drillPiece == grabObj.itemProperties.name));
        }
    }
}
