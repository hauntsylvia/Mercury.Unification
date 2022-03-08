using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Unification.IO.File
{
    public class Record<T>
    {
        public Record(T ObjectToStore, IList<string> Notes)
        {
            this.objectToStore = ObjectToStore;
            this.notes = Notes;
        }

        private readonly T objectToStore;
        public T ObjectToStore => this.objectToStore;


        private readonly IList<string> notes;
        public IList<string> Notes => this.notes;


        private readonly DateTime utcTimestamp = DateTime.UtcNow;
        public DateTime UTCTimestamp => this.utcTimestamp;
    }
}
