﻿//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Runtime.InteropServices;

namespace xsVlc.Core.Utils
{
    internal unsafe class MemoryHeap
    {
        private static readonly IntPtr Ph = GetProcessHeap();

        private MemoryHeap()
        {
            /* Empty private constructor */
        }

        public static void* Alloc(int size)
        {
            var result = HeapAlloc(Ph, HeapZeroMemory, size);
            if (result == null)
            {
                throw new OutOfMemoryException();
            }

            return result;
        }

        public static void Free(void* block)
        {
            if (!HeapFree(Ph, 0, block))
            {
                throw new InvalidOperationException();
            }
        }

        public static void* ReAlloc(void* block, int size)
        {
            var result = HeapReAlloc(Ph, HeapZeroMemory, block, size);
            if (result == null)
            {
                throw new OutOfMemoryException();
            }

            return result;
        }

        public static int SizeOf(void* block)
        {
            var result = HeapSize(Ph, 0, block);
            if (result == -1)
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        // Heap API flags
        private const int HeapZeroMemory = 0x00000008;

        // Heap API functions
        [DllImport("kernel32")]
        static extern IntPtr GetProcessHeap();

        [DllImport("kernel32")]
        static extern void* HeapAlloc(IntPtr hHeap, int flags, int size);

        [DllImport("kernel32")]
        static extern bool HeapFree(IntPtr hHeap, int flags, void* block);

        [DllImport("kernel32")]
        static extern void* HeapReAlloc(IntPtr hHeap, int flags, void* block, int size);

        [DllImport("kernel32")]
        static extern int HeapSize(IntPtr hHeap, int flags, void* block);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = true)]
        public static extern void CopyMemory(void* dest, void* src, int size);
    }
}
