using UnityEngine;
using System;
public class DefaultShove : GenericShove
{
    public DefaultShove() : base(0.5f, 0.25f, 7.5f, 1.5f, 0.1f) {}
    public override short Complexity => 1;
}