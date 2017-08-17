﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Pipenet.Transport
{
    /// <summary>
    /// 数据层面的包接口，只能在数据层面使用即Transport内使用
    /// </summary>
    public interface IPacket
    {
        byte[] GetData();
        void SetID(int id);
        int GetID();
    }
    /// <summary>
    /// 包
    /// 加入新包的方法：定义一个类继承此类。
    ///     在Client.InitClient中填写客户端接收包触发的事件
    ///     在Server.setReceiveEventLists中填写服务端接收包触发的事件
    /// </summary>
    [Serializable]
    public class Packet : IPacket
    {
        public int Id
        {
            private set;
            get;
        }
        public PacketType type = PacketType.None;
        /// <summary>
        /// 传输（一般为接收）Packet所使用的transport。不被序列化。
        /// </summary>
        [NonSerialized]
        public ITransport transport;
        const int NONE_ID = -1;
        public Packet()
        {
            Id = NONE_ID;
        }
        /// <summary>
        /// 将数据转成包
        /// </summary>
        /// <param name="data"></param>
        public static Packet GetPacket(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            Packet result = (Packet)bf.Deserialize(stream);
            stream.Close();
            return result;
        }
        /// <summary>
        /// 得到包数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, this);
            byte[] data = stream.GetBuffer();
            stream.Close();
            return data;
        }

        void IPacket.SetID(int id)
        {
            this.Id = id;
        }

        int IPacket.GetID()
        {
            return Id;
        }
    }
    public enum PacketType
    {
        None,
        Login
    }
}