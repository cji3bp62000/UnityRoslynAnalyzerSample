using System;
using System.Collections.Generic;
using System.Text;
using Generators;
using MsgPack.Sample;

namespace MsgPack.Sample
{
    public class TestRecord : RecordBase
    {
        public int intProperty { get; set; }
        public long longProperty { get; set; }
        public float floatProperty { get; set; }
        public double doubleProperty { get; set; }
        public string stringProperty { get; set; }
        public DateTime dateTimeProperty { get; set; }
        public DateTimeOffset dateTimeOffsetProperty { get; set; }

        public ArbitraryClass arbitraryClassProperty { get; set; }
        public ArbitraryClass.ArbitraryNestedClass arbitraryNestedClassProperty { get; set; }
        public ArbitraryClass[] arbitraryClassArrayProperty { get; set; }
        public ArbitraryGenericClass<int> arbitraryGenericClassIntProperty { get; set; }

        public void SomeMethod() { }
        public int GetOnlyIntProperty => intProperty;
        public int PrivateSetIntProperty { get; private set; }
    }

    public class ArbitraryClass
    {
        public class ArbitraryNestedClass { }
    }
    public class ArbitraryGenericClass<T> { }
}

namespace MyNamespace
{
    public class AnotherMstRecord : RecordBase
    {
        public int mstId { get; set; }
        public string name { get; set; }
    }
}


// 目標
namespace MsgPack.RecordUnpackers.Sample
{
    public class TestRecordUnpacker : RecordUnpackerBase<TestRecordUnpacker, TestRecord>
    {
        private static readonly byte[] intPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.intProperty));
        private static readonly byte[] longPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.longProperty));
        private static readonly byte[] floatPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.floatProperty));
        private static readonly byte[] doublePropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.doubleProperty));
        private static readonly byte[] stringPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.stringProperty));
        private static readonly byte[] dateTimePropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.dateTimeProperty));
        private static readonly byte[] dateTimeOffsetPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.dateTimeOffsetProperty));
        private static readonly byte[] arbitraryClassPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.arbitraryClassProperty));
        private static readonly byte[] arbitraryNestedClassPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.arbitraryNestedClassProperty));
        private static readonly byte[] arbitraryClassArrayPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.arbitraryClassArrayProperty));
        private static readonly byte[] arbitraryGenericClassIntPropertyNameByte = Encoding.UTF8.GetBytes(nameof(TestRecord.arbitraryGenericClassIntProperty));

        private static readonly Dictionary<Memory<byte>, int> nameIndexDic = new()
        {
            [intPropertyNameByte] = 0,
            [longPropertyNameByte] = 1,
            [floatPropertyNameByte] = 2,
            [doublePropertyNameByte] = 3,
            [stringPropertyNameByte] = 4,
            [dateTimePropertyNameByte] = 5,
            [dateTimeOffsetPropertyNameByte] = 6,
            [arbitraryClassPropertyNameByte] = 7,
            [arbitraryNestedClassPropertyNameByte] = 8,
            [arbitraryClassArrayPropertyNameByte] = 9,
            [arbitraryGenericClassIntPropertyNameByte] = 10,
        };

        protected override Dictionary<Memory<byte>, int> GetNameIndexDic() => nameIndexDic;

        protected override bool TryUnpackByIndex(int index, Span<byte> nameSpan, ObjectPacker packer, MsgPackReader reader, TestRecord record, bool isIndexAlreadyChecked = false)
        {
            switch (index) {
                case 0:
                    if (isIndexAlreadyChecked || intPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.intProperty = ReadInt(packer, reader);
                    }
                    return true;
                case 1:
                    if (isIndexAlreadyChecked || longPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.longProperty = ReadLong(packer, reader);
                    }
                    return true;
                case 2:
                    if (isIndexAlreadyChecked || floatPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.floatProperty = ReadFloat(packer, reader);
                    }
                    return true;
                case 3:
                    if (isIndexAlreadyChecked || doublePropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.doubleProperty = ReadDouble(packer, reader);
                    }
                    return true;
                case 4:
                    if (isIndexAlreadyChecked || stringPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.stringProperty = ReadString(packer, reader);
                    }
                    return true;
                case 5:
                    if (isIndexAlreadyChecked || dateTimePropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.dateTimeProperty = ReadDateTime(packer, reader);
                    }
                    return true;
                case 6:
                    if (isIndexAlreadyChecked || dateTimeOffsetPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.dateTimeOffsetProperty = ReadDateTimeOffset(packer, reader);
                    }
                    return true;
                case 7:
                    if (isIndexAlreadyChecked || arbitraryClassPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.arbitraryClassProperty = (MsgPack.Sample.ArbitraryClass) ReadUnknownType(packer, reader);
                    }
                    return true;
                case 8:
                    if (isIndexAlreadyChecked || arbitraryNestedClassPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.arbitraryNestedClassProperty = (MsgPack.Sample.ArbitraryClass.ArbitraryNestedClass) ReadUnknownType(packer, reader);
                    }
                    return true;
                case 9:
                    if (isIndexAlreadyChecked || arbitraryClassArrayPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.arbitraryClassArrayProperty = (MsgPack.Sample.ArbitraryClass[]) ReadUnknownType(packer, reader);
                    }
                    return true;
                case 10:
                    if (isIndexAlreadyChecked || arbitraryGenericClassIntPropertyNameByte.AsSpan().SequenceEqual(nameSpan)) {
                        record.arbitraryGenericClassIntProperty = (MsgPack.Sample.ArbitraryGenericClass<int>) ReadUnknownType(packer, reader);
                    }
                    return true;
            }

            return false;
        }
    }
}