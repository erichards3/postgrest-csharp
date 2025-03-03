﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Postgrest.Extensions
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (var current = type; current != null; current = current.BaseType)
                yield return current;
        }
    }
}
