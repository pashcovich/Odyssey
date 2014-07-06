using System;
using System.Runtime.InteropServices;

namespace Odyssey.Content
{
    public static class Serializer
    {
        public static T DeserializeStruct<T>(Byte[] data, int startIndex, int size)
            where T : struct
        {
            
            IntPtr buff = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, startIndex, buff, size);
            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
            Marshal.FreeHGlobal(buff);
            return retStruct;
        }

        public static byte[] SerializeStruct<T>(T str)
            where T : struct

        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T[] DeserializeStructArray<T>(Byte[] data, int startIndex, int count)
            where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            T[] structArray = new T[count];
            int index = startIndex;
            for (int i = 0; i < count; i++)
            {
                structArray[i] = DeserializeStruct<T>(data, index, objsize);
                index += objsize;
            }
            return structArray;
        }

        


    }
}
