using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NCF.Messenger.Tests
{
    class Program
    {
        [DllImport(@"C:\Root\Project\NCF\NCF.Messenger\bin\Debug\x86\NCF.Messenger.DLL", CallingConvention = CallingConvention.Cdecl)]
        extern static int NCFx_Send_AlarmEvent(string message);

        [DllImport(@"C:\Root\Project\NCF\NCF.Messenger\bin\Debug\x86\NCF.Messenger.DLL", CallingConvention = CallingConvention.Cdecl)]
        extern static int NCFx_Send_WRRStaEvent(uint sec1980, ushort mss);

        [DllImport(@"C:\Root\Project\NCF\NCF.Messenger\bin\Debug\x86\NCF.Messenger.DLL", CallingConvention = CallingConvention.Cdecl)]
        extern static int NCFx_Send_WRRCycEvent(uint secStart, ushort msStart, uint secEnd, ushort msEnd, ushort paylad, uint msLoad, uint msUnload, ushort flags);

        static void Main(string[] args)
        {
            try
            {
                NCFx_Send_WRRStaEvent(12345, 679);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            try
            {
                NCFx_Send_WRRCycEvent(12345, 679, 23456, 286, 15678, 54778, 6532, 24);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }
    }
}
