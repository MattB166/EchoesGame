using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResettable
{
    public void InitializeState(); // Initialize the item to its starting state. useful for platforms and switches that need to be reset when the player dies.
    public void ResetState(); // Reset the item when the player dies to its starting state. useful for platforms and switches that need to be reset when the player dies.
    //or cannot be reached. 


}
