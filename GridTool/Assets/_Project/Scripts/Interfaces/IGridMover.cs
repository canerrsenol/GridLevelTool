using System.Collections.Generic;
using UnityEngine;

public interface IGridMover
{
    public void Move(List<Vector2Int> path);
}
