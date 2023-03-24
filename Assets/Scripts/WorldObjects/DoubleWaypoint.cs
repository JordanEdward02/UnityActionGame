using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleWaypoint : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] public DoubleWaypoint next;
    [SerializeField] public DoubleWaypoint previous;

}
