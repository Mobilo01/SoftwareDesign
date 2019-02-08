using System;
using System.Collections.Generic;

public class Block{
    public TimeSpan timespan;
    public int blockNumber;
    public List<Course> blockCourses;
    public Day day;

    public Block (TimeSpan zeit, int blockNumber, Day day) 
        {
            this.timespan = zeit;
            this.blockNumber = blockNumber;
            this.day = day;
        }
}