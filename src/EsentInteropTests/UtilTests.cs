//-----------------------------------------------------------------------
// <copyright file="UtilTests.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

namespace InteropApiTests
{
    using System;
    using System.Text;
    using Microsoft.Isam.Esent.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Util class.
    /// </summary>
    [TestClass]
    public class UtilTests
    {
        /// <summary>
        /// Test DumpBytes with a null array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a null array")]
        public void TestDumpBytesNull()
        {
            Assert.AreEqual("<null>", Util.DumpBytes(null, 0, 0));
        }

        /// <summary>
        /// Test DumpBytes with a zero-length array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a zero-length array")]
        public void TestDumpBytesZeroLength()
        {
            Assert.AreEqual(string.Empty, Util.DumpBytes(new byte[0], 0, 0));
        }

        /// <summary>
        /// Test DumpBytes with a negative offset.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a negative offset")]
        public void TestDumpBytesNegativeOffset()
        {
            Assert.AreEqual("<invalid>", Util.DumpBytes(new byte[1], -1, 1));
        }

        /// <summary>
        /// Test DumpBytes with an offset past the start of the array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with an offset past the start of the array")]
        public void TestDumpBytesInvalidOffset()
        {
            Assert.AreEqual("<invalid>", Util.DumpBytes(new byte[1], 2, 1));
        }

        /// <summary>
        /// Test DumpBytes with a negative count.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a negative count")]
        public void TestDumpBytesNegativeCount()
        {
            Assert.AreEqual("<invalid>", Util.DumpBytes(new byte[2], 2, -1));
        }

        /// <summary>
        /// Test DumpBytes with a count past the end of the array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a count past the end of the array")]
        public void TestDumpBytesInvalidCount()
        {
            Assert.AreEqual("<invalid>", Util.DumpBytes(new byte[1], 1, 2));
        }

        /// <summary>
        /// Test DumpBytes with a short array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a short array")]
        public void TestDumpBytesShortArray()
        {
            Assert.AreEqual("DD-CC-BB-AA", Util.DumpBytes(BitConverter.GetBytes(0xAABBCCDD), 0, 4));
        }

        /// <summary>
        /// Test DumpBytes with an offset.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with an offset")]
        public void TestDumpBytesOffset()
        {
            Assert.AreEqual("CC-BB", Util.DumpBytes(BitConverter.GetBytes(0xAABBCCDD), 1, 2));
        }

        /// <summary>
        /// Test DumpBytes with a truncated array.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test DumpBytes with a truncated array")]
        public void TestDumpBytesTruncatedArray()
        {
            var b = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9 };
            Assert.AreEqual("00-01-02-03-04-05-06-07... (10 bytes)", Util.DumpBytes(b, 0, b.Length));
        }
            
        /// <summary>
        /// Test ArrayEqual with equal arrays.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ArrayEqual with equal arrays")]
        public void TestArrayEqualTrue()
        {
            byte[] a = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6 };
            byte[] b = new byte[] { 0xF, 0x2, 0x3, 0x4, 0x5, 0xF, 0x0 };
            Assert.IsTrue(Util.ArrayEqual(a, b, 1, 4));
        }

        /// <summary>
        /// Test ArrayEqual with unequal arrays.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ArrayEqual with unequal arrays")]
        public void TestArrayEqualFalse()
        {
            byte[] a = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6 };
            byte[] b = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6 };

            for (int offset = 0; offset < a.Length - 1; ++offset)
            {
                for (int count = 1; count < a.Length - offset; ++count)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        b[offset + i] ^= 0xFF;
                        Assert.IsFalse(
                            Util.ArrayEqual(a, b, offset, count),
                            "{0} is equal to {1} (offset = {2}, count = {3})",
                            BitConverter.ToString(a),
                            BitConverter.ToString(b),
                            offset,
                            count);
                        b[offset + i] ^= 0xFF;
                    }
                }
            }
        }

        /// <summary>
        /// Check that ArrayObjectContentEquals compares null arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayObjectContentEquals compares null arrays correctly")]
        public void TestArrayObjectContentEqualsNullArrays()
        {
            Assert.IsTrue(Util.ArrayObjectContentEquals<JET_SPACEHINTS>(null, null, 0)); 
        }

        /// <summary>
        /// Check that ArrayObjectContentEquals compares null entries correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayObjectContentEquals compares null members correctly")]
        public void TestArrayObjectContentEqualsNullMembers()
        {
            var x = new JET_SPACEHINTS[] { null, null };
            var y = new JET_SPACEHINTS[] { null, null };

            Assert.IsTrue(Util.ArrayObjectContentEquals(x, y, x.Length));
        }

        /// <summary>
        /// Check that ArrayObjectContentEquals compares differing arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayObjectContentEquals compares differeing arrays correctly")]
        public void TestArrayObjectContentEqualsDiffer()
        {
            var a = new[]
            {
                new JET_SPACEHINTS
                {
                    ulInitialDensity = 33,
                    cbInitial = 4096,
                }
            };

            var b = new[]
            {
                new JET_SPACEHINTS
                {
                    ulInitialDensity = 34,
                    cbInitial = 4096,
                }
            };

            var c = new[]
            {
                new JET_SPACEHINTS
                {
                    ulInitialDensity = 33,
                    cbInitial = 4077,
                }
            };

            var d = new JET_SPACEHINTS[]
            {
                null,
            };

            JET_SPACEHINTS[][] values = new[] { a, b, c, d, null };

            for (int i = 0; i < values.Length - 1; i++)
            {
                for (int j = i + 1; j < values.Length; j++)
                {
                    Assert.IsFalse(Util.ArrayObjectContentEquals(values[i], values[j], 1));
                }
            }
        }

        /// <summary>
        /// Check that ArrayObjectContentEquals compares identical arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayObjectContentEquals compares identical arrays correctly")]
        public void TestArrayObjectContentEqualsIdentical()
        {
            var x = new[]
            {
                new JET_SPACEHINTS
                {
                    ulInitialDensity = 33,
                    cbInitial = 4096,
                }
            };

            var y = new[]
            {
                new JET_SPACEHINTS
                {
                    ulInitialDensity = 33,
                    cbInitial = 4096,
                }
            };

            var z = new[]
            {
                x[0],
            };

            JET_SPACEHINTS[][] values = new[] { x, y, z };

            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    Assert.IsTrue(Util.ArrayObjectContentEquals(values[i], values[j], 1));
                }
            }
        }

        /// <summary>
        /// Check that ArrayStructEquals compares null arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayStructEquals compares null arrays correctly")]
        public void TestArrayStructEqualsNullArrays()
        {
            Assert.IsTrue(Util.ArrayStructEquals<byte>(null, null, 0));
        }

        /// <summary>
        /// Check that ArrayStructEquals compares the same array correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayStructEquals compares the same array correctly")]
        public void TestArrayStructEqualsSameArrays()
        {
            var x = new long[] { 0x1, 0x2, 0x3, 0x4 };
            Assert.IsTrue(Util.ArrayStructEquals(x, x, x.Length));
        }

        /// <summary>
        /// Check that ArrayStructEquals compares equal arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayStructEquals compares equal arrays correctly")]
        public void TestArrayStructEqualsEqualArrays()
        {
            Assert.IsTrue(Util.ArrayStructEquals(new byte[] { 0x1, 0x2 }, new byte[] { 0x1, 0x2, 0x3 }, 2));
        }

        /// <summary>
        /// Check that ArrayStructEquals compares unequal arrays correctly.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Check that ArrayStructEquals compares unequal arrays correctly")]
        public void TestArrayStructEqualsUnequalArrays()
        {
            Assert.IsFalse(Util.ArrayStructEquals(new byte[] { 0x1, 0x3 }, new byte[] { 0x1, 0x2 }, 2));
        }

        /// <summary>
        /// Check that AddTrailingDirectorySeparator returns null when given
        /// a null string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test AddTrailingDirectorySeparator with null")]
        public void TestAddTrailingDirectorySeparatorNullString()
        {
            Assert.IsNull(Util.AddTrailingDirectorySeparator(null));
        }

        /// <summary>
        /// Check that AddTrailingDirectorySeparator returns an empty string when given
        /// an empty string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test AddTrailingDirectorySeparator with an empty string")]
        public void TestAddTrailingDirectorySeparatorEmptyString()
        {
            Assert.AreEqual(string.Empty, Util.AddTrailingDirectorySeparator(string.Empty));
        }

        /// <summary>
        /// Check that AddTrailingDirectorySeparator returns a terminated string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test AddTrailingDirectorySeparator with a string that has the separator")]
        public void TestAddTrailingDirectorySeparatorTerminatedString()
        {
            string expected = @"foo\";
            Assert.AreEqual(expected, Util.AddTrailingDirectorySeparator(expected));
        }

        /// <summary>
        /// Check that AddTrailingDirectorySeparator returns a terminated string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test AddTrailingDirectorySeparator with a string that does not have the separator")]
        public void TestAddTrailingDirectorySeparatorNonTerminatedString()
        {
            string expected = @"foo\bar\";
            Assert.AreEqual(expected, Util.AddTrailingDirectorySeparator(@"foo\bar"));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedAsciiByteArray with a unicode string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedAsciiByteArray with a unicode string")]
        public void TestConvertStringToNullTerminatedAsciiByteArray()
        {
            string test = "SampleStringThatILike";
            
            byte[] byteArray = Util.ConvertToNullTerminatedAsciiByteArray(test);
            Assert.AreEqual(test.Length + 1, byteArray.Length);
            Assert.IsTrue(Util.ArrayStructEquals(byteArray, LibraryHelpers.EncodingASCII.GetBytes(test + char.MinValue), byteArray.Length));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedAsciiByteArray with an empty string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedAsciiByteArray with an empty string")]
        public void TestConvertEmptyStringToNullTerminatedAsciiByteArray()
        {
            string test = string.Empty;
            
            byte[] byteArray = Util.ConvertToNullTerminatedAsciiByteArray(test);
            Assert.AreEqual(test.Length + 1, byteArray.Length);
            Assert.IsTrue(Util.ArrayStructEquals(byteArray, LibraryHelpers.EncodingASCII.GetBytes(test + char.MinValue), byteArray.Length));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedAsciiByteArray with a null string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedAsciiByteArray with a null string")]
        public void TestConvertNullStringToNullTerminatedAsciiByteArray()
        {
            byte[] byteArray = Util.ConvertToNullTerminatedAsciiByteArray(null);
            Assert.AreEqual(null, byteArray);
        }

        /// <summary>
        /// Test ConvertToNullTerminatedAsciiByteArray with a japanese unicode string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedAsciiByteArray with a japanese unicode string")]
        public void TestConvertJapaneseUnicodeStringToNullTerminatedAsciiByteArray()
        {
            // This japanese string should get converted to "??" in ASCII.
            string japanese = new string(new char[] { '\u70B9', '\u83DC' });

            byte[] byteArray = Util.ConvertToNullTerminatedAsciiByteArray(japanese);

            Assert.AreEqual(3, byteArray.Length);
            Assert.AreEqual((byte)'?', byteArray[0]);
            Assert.AreEqual((byte)'?', byteArray[1]);
            
            Assert.AreEqual(japanese.Length + 1, byteArray.Length);
            Assert.IsTrue(Util.ArrayStructEquals(byteArray, LibraryHelpers.EncodingASCII.GetBytes(japanese + char.MinValue), byteArray.Length));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedAsciiByteArray with a key string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedAsciiByteArray with a key string")]
        public void TestConvertKeyStringToNullTerminatedAsciiByteArray()
        {
            string key = "+col1\0-col2\0";
        
            byte[] byteArray = Util.ConvertToNullTerminatedAsciiByteArray(key);
        
            Assert.AreEqual(key.Length + 1, byteArray.Length);

            // We expect the byte array to be double-null terminated.
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 2]);
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 1]);
            Assert.IsTrue(Util.ArrayStructEquals(byteArray, LibraryHelpers.EncodingASCII.GetBytes(key + char.MinValue), byteArray.Length));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedUnicodeByteArray with a key string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedUnicodeByteArray with a key string")]
        public void TestConvertToNullTerminatedUnicodeByteArray()
        {
            string key = "+col1\0-col2\0";

            byte[] byteArray = Util.ConvertToNullTerminatedUnicodeByteArray(key);

            Assert.AreEqual(sizeof(char) * (key.Length + 1), byteArray.Length);

            // We expect the byte array to be double-null terminated.
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 4]);
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 3]);
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 2]);
            Assert.AreEqual((byte)0, byteArray[byteArray.Length - 1]);

            Assert.IsTrue(Util.ArrayStructEquals(byteArray, Encoding.Unicode.GetBytes(key + char.MinValue), byteArray.Length));
        }

        /// <summary>
        /// Test ConvertToNullTerminatedUnicodeByteArray with a null string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedUnicodeByteArray with a null string")]
        public void TestConvertToNullTerminatedUnicodeByteArrayWithNull()
        {
            byte[] byteArray = Util.ConvertToNullTerminatedUnicodeByteArray(null);

            Assert.IsNull(byteArray);
        }

        /// <summary>
        /// Test ConvertToNullTerminatedUnicodeByteArray with an empty string.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Test ConvertToNullTerminatedUnicodeByteArray with an empty string")]
        public void TestConvertToNullTerminatedUnicodeByteArrayWithEmptyString()
        {
            byte[] byteArray = Util.ConvertToNullTerminatedUnicodeByteArray(string.Empty);

            Assert.IsNotNull(byteArray);
            Assert.AreEqual(2, byteArray.Length);
        }
    }
}