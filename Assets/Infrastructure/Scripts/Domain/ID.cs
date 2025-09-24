using System;

namespace Infrastructure.Domain
{
    public class ID : IEquatable<ID>
    {
        public long? Value { get; }

        public bool HasValue => Value.HasValue;

        public ID()
        {
            Value = null;
        }

        public ID(int id)
        {
            Value = id;
        }

        public ID(long id)
        {
            Value = id;
        }

        // 比較ロジック
        public bool Equals(ID other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ID);
        }

        public override int GetHashCode()
        {
            return Value.HasValue ? Value.Value.GetHashCode() : 0;
        }

        // 演算子オーバーロード
        public static bool operator ==(ID left, ID right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null)) return false;
            if (ReferenceEquals(right, null)) return false;
            return left.Value == right.Value;
        }

        public static bool operator !=(ID left, ID right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return HasValue ? Value.Value.ToString() : "null";
        }
    }
}
