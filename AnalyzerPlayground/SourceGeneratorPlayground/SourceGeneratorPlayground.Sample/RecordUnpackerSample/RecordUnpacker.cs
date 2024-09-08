using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;

namespace MsgPack
{
    public abstract class RecordUnpackerBase
    {
        public abstract object UnpackDelegate();
    }

    public abstract class RecordUnpackerBase<TSelf, TRecord> : RecordUnpackerBase
        where TSelf : RecordUnpackerBase<TSelf, TRecord>, new()
        where TRecord : new()
    {
        public override object UnpackDelegate()
        {
            return null;
        }

        protected abstract Dictionary<Memory<byte>, int> GetNameIndexDic();

        protected abstract bool TryUnpackByIndex(int index, Span<byte> nameSpan, ObjectPacker packer, MsgPackReader reader,
            TRecord record, bool isIndexAlreadyChecked = false);

        protected int ReadInt(ObjectPacker packer, MsgPackReader reader)
        {
            return 0;
        }

        protected long ReadLong(ObjectPacker packer, MsgPackReader reader)
        {
            return 0;
        }

        protected float ReadFloat(ObjectPacker packer, MsgPackReader reader)
        {
            return 0;
        }

        protected double ReadDouble(ObjectPacker packer, MsgPackReader reader)
        {
            return 0;
        }

        protected string ReadString(ObjectPacker packer, MsgPackReader reader)
        {
            return null;
        }

        protected DateTime ReadDateTime(ObjectPacker packer, MsgPackReader reader)
        {
            return DateTime.MinValue;
        }

        protected DateTimeOffset ReadDateTimeOffset(ObjectPacker packer, MsgPackReader reader)
        {
            return DateTimeOffset.MinValue;
        }

        protected object ReadUnknownType(ObjectPacker packer, MsgPackReader reader)
        {
            return null;
        }
    }
}
