using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TooltipType { TOOLTIP, THOUGHT}

public class TooltipHolder : MonoBehaviour
{
    public string tooltip;
    public TooltipType type;
}
