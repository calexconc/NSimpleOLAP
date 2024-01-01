using System;

namespace NSimpleOLAP.Common.Hashing
{
  internal class CityHash<T> : Hasher<T>
    where T : struct, IComparable
  {
    private const ulong k0 = 0xc3a5c85c97cb3127UL;
    private const ulong k1 = 0xb492b66fbe98f273UL;
    private const ulong k2 = 0x9ae16a3b2f90404fUL;
    private const uint c1 = 0xcc9e2d51;
    private const uint c2 = 0x1b873593;

    private Func<byte[], T> _hashfunction;

    public CityHash()
    {
      this.HashStrategy = MolapHashTypes.CITY;
      _hashfunction = this.Init();
    }

    protected override T HashingFunction(byte[] bytes)
    {
      return _hashfunction(bytes);
    }

    #region

    private Func<byte[], T> Init()
    {
      Type type = typeof(T);
      Func<byte[], T> function = null;

      if (type == typeof(int))
        function = (Func<byte[], T>)(object)FuncInt();
      else if (type == typeof(uint))
        function = (Func<byte[], T>)(object)FuncUInt();
      else if (type == typeof(long))
        function = (Func<byte[], T>)(object)FuncLong();
      else if (type == typeof(ulong))
        function = (Func<byte[], T>)(object)FuncULong();

      return function;
    }

    private Func<byte[], int> FuncInt()
    {
      return (bytes) => HashFunctionInt32(bytes);
    }

    private Func<byte[], uint> FuncUInt()
    {
      return (bytes) => HashFunctionUInt32(bytes);
    }

    private Func<byte[], long> FuncLong()
    {
      return (bytes) => HashFunctionInt64(bytes);
    }

    private Func<byte[], ulong> FuncULong()
    {
      return (bytes) => HashFunctionUInt64(bytes);
    }

    #endregion

    #region hash functions

    protected static int HashFunctionInt32(byte[] bytes)
    {
      return unchecked((int)HashFunctionUInt32(bytes));
    }

    protected static uint HashFunctionUInt32(byte[] data)
    {
      return Hash32(data);
    }

    protected static long HashFunctionInt64(byte[] bytes)
    {
      return unchecked((long)HashFunctionUInt64(bytes));
    }

    protected static ulong HashFunctionUInt64(byte[] data)
    {
      return Hash64(data, 0xe17a1465);
    }

    #endregion

    #region private methods

    private static uint Mix(uint h)
    {
      h ^= h >> 16;
      h *= 0x85ebca6b;
      h ^= h >> 13;
      h *= 0xc2b2ae35;
      h ^= h >> 16;
      return h;
    }

    private static ulong Rotate64(ulong val, int shift)
    {
      return shift == 0 ? val : ((val >> shift) | (val << (64 - shift)));
    }

    private static uint Rotate32(uint value, int shift)
    {
      return shift == 0 ? value : ((value >> shift) | (value << (32 - shift)));
    }

    private static uint MTransform(uint a, uint h)
    {
      a *= c1;
      a = Rotate32(a, 17);
      a *= c2;
      h ^= a;
      h = Rotate32(h, 19);
      return h * 5 + 0xe6546b64;
    }

    private static uint BitSwap32(uint value)
    {
      return
          (value >> 24) |
          (value & 0x00ff0000) >> 8 |
          (value & 0x0000ff00) << 8 |
          (value << 24);
    }

    private static ulong BitSwap64(ulong value)
    {
      return
          (value >> 56) |
          (value & 0x00ff000000000000) >> 40 |
          (value & 0x0000ff0000000000) >> 24 |
          (value & 0x000000ff00000000) >> 8 |
          (value & 0x00000000ff000000) << 8 |
          (value & 0x0000000000ff0000) << 24 |
          (value & 0x000000000000ff00) << 40 |
          (value << 56);
    }

    private static uint Fetch32(byte[] value, int index = 0)
    {
      return BitConverter.ToUInt32(value, index);
    }

    private static uint Fetch32(byte[] value, uint index = 0)
    {
      return BitConverter.ToUInt32(value, (int)index);
    }

    private static ulong Fetch64(byte[] value, int index = 0)
    {
      return BitConverter.ToUInt64(value, index);
    }

    private static uint Hash32Len0to4(byte[] value)
    {
      var l = (uint)value.Length;
      var b = 0u;
      var c = 9u;
      for (var i = 0; i < l; i++)
      {
        b = b * c1 + (uint)((sbyte)value[i]);
        c ^= b;
      }

      return Mix(MTransform(b, MTransform(l, c)));
    }

    private static uint Hash32Len5to12(byte[] value)
    {
      uint a = (uint)value.Length, b = a * 5, c = 9, d = b;

      a += Fetch32(value, 0);
      b += Fetch32(value, value.Length - 4);
      c += Fetch32(value, (value.Length >> 1) & 4);

      return Mix(MTransform(c, MTransform(b, MTransform(a, d))));
    }

    private static uint Hash32Len13to24(byte[] value)
    {
      var a = Fetch32(value, (value.Length >> 1) - 4);
      var b = Fetch32(value, 4);
      var c = Fetch32(value, value.Length - 8);
      var d = Fetch32(value, value.Length >> 1);
      var e = Fetch32(value, 0);
      var f = Fetch32(value, value.Length - 4);
      var h = (uint)value.Length;

      return Mix(MTransform(f, MTransform(e, MTransform(d, MTransform(c, MTransform(b, MTransform(a, h)))))));
    }

    private static void Permute3<S>(ref S a, ref S b, ref S c)
    {
      var temp = a;
      a = c; c = b; b = temp;
    }

    private static ulong ShiftMix(ulong val)
    {
      return val ^ (val >> 47);
    }

    private static void Swap<S>(ref S a, ref S b)
    {
      var temp = a;
      a = b;
      b = temp;
    }

    private static ulong HashLen16(ulong u, ulong v)
    {
      return Hash128to64(new UInt128(u, v));
    }

    private static ulong HashLen16(ulong u, ulong v, ulong mul)
    {
      var a = (u ^ v) * mul;
      a ^= (a >> 47);
      var b = (v ^ a) * mul;
      b ^= (b >> 47);
      b *= mul;
      return b;
    }

    private static ulong HashLen0to16(byte[] value, int offset = 0)
    {
      var len = (uint)(value.Length - offset);

      if (len >= 8)
      {
        var mul = k2 + (ulong)len * 2;
        var a = Fetch64(value, offset) + k2;
        var b = Fetch64(value, value.Length - 8);
        var c = Rotate64(b, 37) * mul + a;
        var d = (Rotate64(a, 25) + b) * mul;

        return HashLen16(c, d, mul);
      }

      if (len >= 4)
      {
        var mul = k2 + (ulong)len * 2;
        ulong a = Fetch32(value, offset);
        return HashLen16(len + (a << 3), Fetch32(value, (int)(offset + len - 4)), mul);
      }

      if (len > 0)
      {
        var a = value[offset];
        var b = value[offset + (len >> 1)];
        var c = value[offset + (len - 1)];

        var y = a + ((uint)b << 8);
        var z = len + ((uint)c << 2);

        return ShiftMix((y * k2 ^ z * k0)) * k2;
      }

      return k2;
    }

    private static ulong HashLen17to32(byte[] value)
    {
      var len = (ulong)value.Length;

      var mul = k2 + len * 2ul;
      var a = Fetch64(value) * k1;
      var b = Fetch64(value, 8);
      var c = Fetch64(value, value.Length - 8) * mul;
      var d = Fetch64(value, value.Length - 16) * k2;

      return HashLen16(Rotate64(a + b, 43) + Rotate64(c, 30) + d, a + Rotate64(b + k2, 18) + c, mul);
    }

    private static UInt64 HashLen33to64(byte[] value)
    {
      var mul = k2 + (ulong)value.Length * 2ul;
      var a = Fetch64(value) * k2;
      var b = Fetch64(value, 8);
      var c = Fetch64(value, value.Length - 24);
      var d = Fetch64(value, value.Length - 32);
      var e = Fetch64(value, 16) * k2;
      var f = Fetch64(value, 24) * 9;
      var g = Fetch64(value, value.Length - 8);
      var h = Fetch64(value, value.Length - 16) * mul;

      var u = Rotate64(a + g, 43) + (Rotate64(b, 30) + c) * 9;
      var v = ((a + g) ^ d) + f + 1;
      var w = BitSwap64((u + v) * mul) + h;
      var x = Rotate64(e + f, 42) + c;
      var y = (BitSwap64((v + w) * mul) + g) * mul;
      var z = e + f + c;

      a = BitSwap64((x + z) * mul + y) + b;
      b = ShiftMix((z + a) * mul + d + h) * mul;
      return b + x;
    }

    private static ulong Hash128to64(UInt128 x)
    {
      const ulong kMul = 0x9ddfea08eb382d69UL;

      var a = (x.Low ^ x.High) * kMul;
      a ^= (a >> 47);

      var b = (x.High ^ a) * kMul;
      b ^= (b >> 47);
      b *= kMul;

      return b;
    }

    private static UInt128 WeakHashLen32WithSeeds(ulong w, ulong x, ulong y, ulong z, ulong a, ulong b)
    {
      a += w;
      b = Rotate64(b + a + z, 21);

      var c = a;
      a += x;
      a += y;

      b += Rotate64(a, 44);

      return new UInt128(a + z, b + c);
    }

    private static UInt128 WeakHashLen32WithSeeds(byte[] value, int offset, ulong a, ulong b)
    {
      return WeakHashLen32WithSeeds(
          Fetch64(value, offset),
          Fetch64(value, offset + 8),
          Fetch64(value, offset + 16),
          Fetch64(value, offset + 24),
          a,
          b);
    }

    #endregion

    #region protected Hash implementation methods

    protected static UInt128 HashCtMur(byte[] value, UInt128 seed, int offset)
    {
      var a = seed.Low;
      var b = seed.High;
      ulong c;
      ulong d;

      var len = value.Length - offset;
      var l = len - 16;

      if (l <= 0)
      {
        a = ShiftMix(a * k1) * k1;
        c = b * k1 + HashLen0to16(value, offset);
        d = ShiftMix(a + (len >= 8 ? Fetch64(value, offset) : c));
      }
      else
      {
        c = HashLen16(Fetch64(value, offset + len - 8) + k1, a);
        d = HashLen16(b + (ulong)len, c + Fetch64(value, offset + len - 16));
        a += d;

        var p = offset;
        do
        {
          a ^= ShiftMix(Fetch64(value, p) * k1) * k1;
          a *= k1;
          b ^= a;
          c ^= ShiftMix(Fetch64(value, p + 8) * k1) * k1;
          c *= k1;
          d ^= c;

          p += 16;
          l -= 16;
        } while (l > 0);
      }
      a = HashLen16(a, c);
      b = HashLen16(d, b);
      return new UInt128(a ^ b, HashLen16(b, a));
    }

    protected internal static uint Hash32(byte[] value)
    {
      if (value == null)
        throw new ArgumentNullException("value");

      var len = (uint)value.Length;

      if (len <= 4)
        return Hash32Len0to4(value);

      if (len <= 12)
        return Hash32Len5to12(value);

      if (len <= 24)
        return Hash32Len13to24(value);

      uint h = len, g = c1 * len, f = g;
      {
        uint a0 = Rotate32(Fetch32(value, len - 4) * c1, 17) * c2;
        uint a1 = Rotate32(Fetch32(value, len - 8) * c1, 17) * c2;
        uint a2 = Rotate32(Fetch32(value, len - 16) * c1, 17) * c2;
        uint a3 = Rotate32(Fetch32(value, len - 12) * c1, 17) * c2;
        uint a4 = Rotate32(Fetch32(value, len - 20) * c1, 17) * c2;

        h ^= a0;
        h = Rotate32(h, 19);
        h = h * 5 + 0xe6546b64;
        h ^= a2;
        h = Rotate32(h, 19);
        h = h * 5 + 0xe6546b64;

        g ^= a1;
        g = Rotate32(g, 19);
        g = g * 5 + 0xe6546b64;
        g ^= a3;
        g = Rotate32(g, 19);
        g = g * 5 + 0xe6546b64;

        f += a4;
        f = Rotate32(f, 19);
        f = f * 5 + 0xe6546b64;
      }

      for (var i = 0; i < (len - 1) / 20; i++)
      {
        var a0 = Rotate32(Fetch32(value, 20 * i) * c1, 17) * c2;
        var a1 = Fetch32(value, 20 * i + 4);
        var a2 = Rotate32(Fetch32(value, 20 * i + 8) * c1, 17) * c2;
        var a3 = Rotate32(Fetch32(value, 20 * i + 12) * c1, 17) * c2;
        var a4 = Fetch32(value, 20 * i + 16);

        h ^= a0;
        h = Rotate32(h, 18);
        h = h * 5 + 0xe6546b64;

        f += a1;
        f = Rotate32(f, 19);
        f = f * c1;

        g += a2;
        g = Rotate32(g, 18);
        g = g * 5 + 0xe6546b64;

        h ^= a3 + a1;
        h = Rotate32(h, 19);
        h = h * 5 + 0xe6546b64;

        g ^= a4;
        g = BitSwap32(g) * 5;

        h += a4 * 5;
        h = BitSwap32(h);

        f += a0;

        Permute3(ref f, ref h, ref g);
      }

      g = Rotate32(g, 11) * c1;
      g = Rotate32(g, 17) * c1;

      f = Rotate32(f, 11) * c1;
      f = Rotate32(f, 17) * c1;

      h = Rotate32(h + g, 19);
      h = h * 5 + 0xe6546b64;
      h = Rotate32(h, 17) * c1;

      h = Rotate32(h + f, 19);
      h = h * 5 + 0xe6546b64;
      h = Rotate32(h, 17) * c1;

      return h;
    }

    protected internal static ulong Hash64(byte[] value)
    {
      if (value.Length <= 16)
        return HashLen0to16(value);

      if (value.Length <= 32)
        return HashLen17to32(value);

      if (value.Length <= 64)
        return HashLen33to64(value);

      var x = Fetch64(value, value.Length - 40);
      var y = Fetch64(value, value.Length - 16) + Fetch64(value, value.Length - 56);
      var z = HashLen16(
          Fetch64(value, value.Length - 48) + (ulong)value.Length,
          Fetch64(value, value.Length - 24));

      var v = WeakHashLen32WithSeeds(value, value.Length - 64, (ulong)value.Length, z);
      var w = WeakHashLen32WithSeeds(value, value.Length - 32, y + k1, x);

      x = x * k1 + Fetch64(value);

      var pos = 0;
      var len = (value.Length) - 1 & ~63;
      do
      {
        x = Rotate64(x + y + v.Low + Fetch64(value, pos + 8), 37) * k1;
        y = Rotate64(y + v.High + Fetch64(value, pos + 48), 42) * k1;
        x ^= w.High;
        y += v.Low + Fetch64(value, pos + 40);
        z = Rotate64(z + w.Low, 33) * k1;
        v = WeakHashLen32WithSeeds(value, pos, v.High * k1, x + w.Low);
        w = WeakHashLen32WithSeeds(value, pos + 32, z + w.High, y + Fetch64(value, pos + 16));
        Swap(ref z, ref x);

        pos += 64;
        len -= 64;
      } while (len != 0);

      return HashLen16(HashLen16(v.Low, w.Low) + ShiftMix(y) * k1 + z, HashLen16(v.High, w.High) + x);
    }

    protected static UInt128 Hash128(byte[] value, UInt128 seed, int offset)
    {
      if (value.Length - offset < 128)
        return HashCtMur(value, seed, offset);

      var len = value.Length - offset;
      var x = seed.Low;
      var y = seed.High;
      var z = (ulong)len * k1;
      var v = new UInt128
      {
        Low = Rotate64(seed.High ^ k1, 49) * k1 + Fetch64(value, offset)
      };
      v.High = Rotate64(v.Low, 42) * k1 + Fetch64(value, offset + 8);

      var w = new UInt128
      {
        Low = Rotate64(y + z, 35) * k1 + x,
        High = Rotate64(seed.Low + Fetch64(value, offset + 88), 53) * k1
      };

      var s = offset;
      do
      {
        x = Rotate64(x + y + v.Low + Fetch64(value, s + 8), 37) * k1;
        y = Rotate64(y + v.High + Fetch64(value, s + 48), 42) * k1;
        x ^= w.High;
        y += v.Low + Fetch64(value, s + 40);
        z = Rotate64(z + w.Low, 33) * k1;
        v = WeakHashLen32WithSeeds(value, s, v.High * k1, x + w.Low);
        w = WeakHashLen32WithSeeds(value, s + 32, z + w.High, y + Fetch64(value, s + 16));

        Swap(ref z, ref x);

        s += 64;

        x = Rotate64(x + y + v.Low + Fetch64(value, s + 8), 37) * k1;
        y = Rotate64(y + v.High + Fetch64(value, s + 48), 42) * k1;
        x ^= w.High;
        y += v.Low + Fetch64(value, s + 40);
        z = Rotate64(z + w.Low, 33) * k1;
        v = WeakHashLen32WithSeeds(value, s, v.High * k1, x + w.Low);
        w = WeakHashLen32WithSeeds(value, s + 32, z + w.High, y + Fetch64(value, s + 16));

        Swap(ref z, ref x);

        s += 64;
        len -= 128;
      } while (len >= 128);

      x += Rotate64(v.Low + z, 49) * k0;
      y = y * k0 + Rotate64(w.High, 37);
      z = z * k0 + Rotate64(w.Low, 27);
      w.Low *= 9;
      v.Low *= k0;

      for (var tail = 0; tail < len;)
      {
        tail += 32;

        y = Rotate64(x + y, 42) * k0 + v.High;
        w.Low += Fetch64(value, s + len - tail + 16);
        x = x * k0 + w.Low;
        z += w.High + Fetch64(value, s + len - tail);
        w.High += v.Low;
        v = WeakHashLen32WithSeeds(value, s + len - tail, v.Low + z, v.High);
        v.Low *= k0;
      }

      x = HashLen16(x, v.Low);
      y = HashLen16(y + z, w.Low);

      return new UInt128
      {
        Low = HashLen16(x + v.High, w.High) + y,
        High = HashLen16(x + w.High, y + v.High)
      };
    }

    protected static ulong Hash64(byte[] value, ulong seed)
    {
      return Hash64(value, k2, seed);
    }

    protected static ulong Hash64(byte[] value, ulong seed0, ulong seed1)
    {
      return HashLen16(Hash64(value) - seed0, seed1);
    }

    protected static UInt128 Hash128(byte[] value)
    {
      return value.Length >= 16
          ? Hash128(value, new UInt128(Fetch64(value), Fetch64(value, 8) + k0), 16)
          : Hash128(value, new UInt128(k0, k1), 0);
    }

    #endregion
  }
}