﻿using System;

namespace NSimpleOLAP.Common.Hashing
{
  /// <summary>
  /// Represents a 128-bit unsigned integer.
  /// </summary>
  public struct UInt128 : IEquatable<UInt128>
  {
    public UInt128(ulong low, ulong high)
        : this()
    {
      Low = low;
      High = high;
    }

    public ulong Low { get; set; }

    public ulong High { get; set; }

    public bool Equals(UInt128 other)
    {
      return Low == other.Low && High == other.High;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is UInt128 && Equals((UInt128)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (Low.GetHashCode() * 397) ^ High.GetHashCode();
      }
    }
  }
}