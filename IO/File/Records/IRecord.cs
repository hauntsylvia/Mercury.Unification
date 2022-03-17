namespace Mercury.Unification.IO.File.Records
{
    public interface IRecord<T>
    {
        public T ObjectToStore { get; }
        public DateTime UTCTimestamp { get; }
    }
}
