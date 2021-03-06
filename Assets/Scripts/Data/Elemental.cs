﻿using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

public enum Element {
    Slash,
    Crush,
    Pierce,
    Fire,
    Water,
    Air,
    Earth,
    Dark,
    Light
}

public static class ElementExtensions {
    public static bool IsPhysical(this Element element) {
        return element == Element.Slash ||
               element == Element.Crush ||
	       element == Element.Pierce;
    }
}

/// <summary>
/// Maps each element to an int (for damage/resistance)
/// </summary>
[fsObject(Converter = typeof(ElementSetConverter))]
public class ElementSet : Enumap<Element, int> { }
class ElementSetConverter : DictConverter<ElementSet> { }

/// <summary>
/// Maps each element to a float (for damage/resistance multiplication factor)
/// </summary>
[fsObject(Converter = typeof(ElementMultiplierConverter))]
public class ElementMultiplier : Enumap<Element, float> { }
class ElementMultiplierConverter : DictConverter<ElementMultiplier> { }