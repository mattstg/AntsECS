using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public enum RelativeDirection { Forward, Right, Left, Back }

    [ExposeAntMethod("Moves the ant in a relative direction")]
    public void Move(RelativeDirection dir)
    {
        Debug.Log("Dir: " + dir);
    }


}
