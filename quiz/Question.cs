using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace quiz
{
    [Table("m_question")]
    public class Question
    {
        public int Id { get; set; }
        public int Image { get; set; }
        public int Level { get; set; }
        public int Category { get; set; }
        public string Answer { get; set; }
    }
}
