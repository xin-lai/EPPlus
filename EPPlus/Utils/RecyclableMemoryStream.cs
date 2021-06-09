using Microsoft.IO;
using System;
using System.IO;

namespace OfficeOpenXml.Utils
{
    public static class RecyclableMemoryStream
    {

        private static readonly Lazy<RecyclableMemoryStreamManager> recyclableMemoryStreamManager = new Lazy<RecyclableMemoryStreamManager>();
        private static RecyclableMemoryStreamManager RecyclableMemoryStreamManager => recyclableMemoryStreamManager.Value;
        internal static MemoryStream GetStream()
        {
            return RecyclableMemoryStreamManager.GetStream();
        }

        internal static MemoryStream GetStream(byte[] array)
        {
            return RecyclableMemoryStreamManager.GetStream(array);
        }

        internal static MemoryStream GetStream(int capacity)
        {
            return RecyclableMemoryStreamManager.GetStream(null, capacity);
        }
    }
}
