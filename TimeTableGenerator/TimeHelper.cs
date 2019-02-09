using System;

public class TimeHelper{
    public static bool TimeCheck(DateTime a, Days d, Prof p){
        foreach(TimeSpanDay s in p.occupied){
            if(s.start < a && s.end > a && d == s.day)
                return false;
        }
        return true;
    }
}