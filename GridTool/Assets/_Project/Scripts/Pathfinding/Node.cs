public class Node
{
    private int gCost;
    private int hCost;
    private int fCost;
    
    private Tile cameFromTile;

    public int GetGCost() { return gCost; }
    
    public int GetHCost() { return hCost; }
    
    public int GetFCost() { return fCost; }
    
    public void SetGCost(int gCost) { this.gCost = gCost; }
    
    public void SetHCost(int hCost) { this.hCost = hCost; }
    
    public void CalculateFCost() { fCost = gCost + hCost; }
    
    public void ResetCameFromTile() { cameFromTile = null; }
    
    public void SetCameFromTile(Tile cameFromTile) { this.cameFromTile = cameFromTile; }
    
    public Tile GetCameFromTile() { return cameFromTile; }
}
