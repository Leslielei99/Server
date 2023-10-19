using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serves01
{
    internal class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex;

        public byte[] Data { get { return data; } }
        public int StartIndex { get { return startIndex; } }
        /// <summary>
        /// 剩余容量
        /// </summary>
        public int RemainSize { get { return data.Length - startIndex; } }
        /// <summary>
        /// 更新开始的下标
        /// </summary>
        /// <param name="count"></param>
        public void AddCount(int count)
        {
            startIndex += count;
        }
        /// <summary>
        /// 解析数据或叫做读取数据
        /// </summary>
        public void ReadMessage()
        {
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);//解析数据头部
                if ((startIndex - 4) >= count)
                {
                    Console.WriteLine("Startindex:"+ startIndex);
                    Console.WriteLine("count:" + count);
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);//将剩余数组覆盖旧数组
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
