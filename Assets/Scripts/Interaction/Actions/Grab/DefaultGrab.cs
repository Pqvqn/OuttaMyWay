using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGrab : GenericGrab
{
    public DefaultGrab() : base(0.5f, 0.125f, 15f) { }
    public override short Complexity => 1;
}
