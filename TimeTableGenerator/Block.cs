using System;
using System.Collections.Generic;

public class Block{
    public TimeSpan timespan;
    public int blockNumber;
    public List<Course> blockCourses = new List<Course>();
    public Days day;

    public Block (TimeSpan zeit, int blockNumber, Days day) 
        {
            this.timespan = zeit;
            this.blockNumber = blockNumber;
            this.day = day;
        }
}