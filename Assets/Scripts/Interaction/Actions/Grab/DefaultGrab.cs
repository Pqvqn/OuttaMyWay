using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGrab : GenericGrab
{
    public DefaultGrab() : base(0.1f, 0.06f, 30f) { }
    public override short Complexity => 1;
}
