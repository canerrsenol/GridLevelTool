using System.Collections.Generic;
using UnityEngine;

public interface IGridMover
{
    public Vector3 GetPosition();
    public void Move(List<TilePosition> path);
    public bool IsMoving();
}
