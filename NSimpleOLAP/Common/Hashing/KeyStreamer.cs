using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NSimpleOLAP.Common.Hashing
{
  internal class KeyStreamer
  {
    public static IEnumerable<byte> TransformKeys<T>(KeyValuePair<T, T>[] tuples)
        where T : struct, IComparable
    {
      foreach (KeyValuePair<T, T> item in tuples)
      {
        foreach (byte bitem in TransformKeyToBytes<T>(item))
          yield return bitem;
      }
    }

    private static IEnumerable<byte> TransformKeyToBytes<T>(KeyValuePair<T, T> pair)
      where T : struct, IComparable
    {
      foreach (byte item in ToByteArray(pair.Key))
        yield return item;

      foreach (byte item in ToByteArray(pair.Value))
        yield return item;
    }

    private static byte[] ToByteArray(object value)
    {
      int rawsize = Marshal.SizeOf(value);
      byte[] rawdata = new byte[rawsize];

      GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
      Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
      handle.Free();

      return rawdata;
    }
  }
}