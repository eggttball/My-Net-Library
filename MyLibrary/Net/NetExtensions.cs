using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.Net
{
    
    public static class NetExtensions
    {

        /// <summary>
        /// 自訂 TCP Socket 連線的相關參數來協助偵測斷線
        /// </summary>
        /// <param name="tcp"></param>
        /// <param name="KeepAliveTime">client靜止多久後才開始送偵測訊息(millisecond)</param>
        /// <param name="KeepAliveInterval">偵測間隔(millisecond)</param>
        public static void SetSocketKeepAliveValues(this TcpClient tcp, int KeepAliveTime, int KeepAliveInterval)
        {
            // KeepAliveTime: default value is 2hr (太久了)
            // KeepAliveInterval: default value is 1s and Detect 5 times

            /* Argument structure for SIO_KEEPALIVE_VALS  (C++ 語法)
             struct tcp_keepalive   {
                u_long onoff;
                u_long keepalivetime;
                u_long keepaliveinterval;
             }
             
             u_long 是 4bytes，所以一共是 12bytes
             */

            uint dummy = 0; //lenth = 4
            byte[] inOptionValues = new byte[global::System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 3]; //size = lenth * 3 = 12
            bool OnOff = true;

            BitConverter.GetBytes((uint)(OnOff ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)KeepAliveTime).CopyTo(inOptionValues, global::System.Runtime.InteropServices.Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)KeepAliveInterval).CopyTo(inOptionValues, global::System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);

            // call WSAIoctl via IOControl
            tcp.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }

    }


}



