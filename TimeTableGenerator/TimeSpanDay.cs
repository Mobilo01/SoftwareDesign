using System;

public struct TimeSpanDay{
    public DateTime start;
    public DateTime end;
    public Days day;

    public TimeSpanDay(DateTime start, DateTime end, Days day) 
    {
        this.start = start;
        this.end = end;
        this.day = day;
    }

}