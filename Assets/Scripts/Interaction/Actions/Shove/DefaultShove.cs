using UnityEngine;
using System;
public class DefaultShove : GenericShove
{
    public DefaultShove() : base(1, 1) {}
    public override short Complexity => 1;
}