using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedVault.Modules
{
    internal class m_process
    {
        public enum SYSTEM_INFORMATION_CLASS
        {
            SystemBasicInformation = 0,
            SystemProcessorInformation = 1,
            SystemPerformanceInformation = 2,
            SystemTimeOfDayInformation = 3,
            SystemProcessInformation = 5,
            SystemProcessorPerformanceInformation = 8,
            SystemModuleInformation = 11,
            SystemHandleInformation = 16,
            SystemObjectInformation = 17,
            SystemPageFileInformation = 18,
            SystemKernelDebuggerInformation = 35,
            SystemCodeIntegrityInformation = 103,
            SystemExtendedHandleInformation = 64
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_PROCESS_INFORMATION
        {
            public uint NextEntryOffset;
            public uint NumberOfThreads;

            private long WorkingSetPrivateSize;
            private uint HardFaultCount;
            private uint NumberOfThreadsHighWatermark;
            private ulong CycleTime;

            private long CreateTime;
            private long UserTime;
            private long KernelTime;

            public UNICODE_STRING ImageName;
            public int BasePriority;

            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;

            public uint HandleCount;
            public uint SessionId;

            public UIntPtr UniqueProcessKey;

            public UIntPtr PeakVirtualSize;
            public UIntPtr VirtualSize;

            public uint PageFaultCount;

            public UIntPtr PeakWorkingSetSize;
            public UIntPtr WorkingSetSize;

            public UIntPtr QuotaPeakPagedPoolUsage;
            public UIntPtr QuotaPagedPoolUsage;

            public UIntPtr QuotaPeakNonPagedPoolUsage;
            public UIntPtr QuotaNonPagedPoolUsage;

            public UIntPtr PagefileUsage;
            public UIntPtr PeakPagefileUsage;

            public UIntPtr PrivatePageCount;

            public long ReadOperationCount;
            public long WriteOperationCount;
            public long OtherOperationCount;

            public long ReadTransferCount;
            public long WriteTransferCount;
            public long OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemHandleInformation
        {
            public ulong NumberOfHandles;
            public IntPtr Handles; // SYSTEM_HANDLE_TABLE_ENTRY_INFO 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_TABLE_ENTRY_INFO
        {
            public ushort UniqueProcessId; // ulong
            public ushort CreatorBackTraceIndex;
            public byte ObjectTypeIndex;
            public byte HandleAttributes;
            public ushort HandleValue;
            public IntPtr Object;
            public uint GrantedAccess;
        }

        public enum OBJECT_INFORMATION_CLASS
        {
            ObjectBasicInformation = 0,
            ObjectTypeInformation = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PUBLIC_OBJECT_TYPE_INFORMATION
        {
            public UNICODE_STRING TypeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public uint[] Reserved;

        }


        public record ProcessEntry(
            uint ProcessId,
            uint NumberOfThreads,
            string Name);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtQuerySystemInformation(
            SYSTEM_INFORMATION_CLASS SystemInformationClass,
            IntPtr SystemInformation,
            uint SytemInformationLenght,
            out uint ReturnLenght);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            uint DesiredAccess,
            bool InheritHandle,
            uint ProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DuplicateHandle(
            IntPtr SourceProcessHandle,
            IntPtr SourceHandle,
            IntPtr TargetProcessHandle,
            out IntPtr TargetHandle,
            uint dwDesiredAccess,
            bool InheritHandle,
            uint Options);

        [DllImport("ntdll.dll",SetLastError = true)]
        public static extern int NtQueryObject(
            IntPtr Handle,
            OBJECT_INFORMATION_CLASS ObjectInformationClass,
            IntPtr ObjectInformation,
            int ObjectInformationLenght,
            out int ReturnLength);
            


    }
}
