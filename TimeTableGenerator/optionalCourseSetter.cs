using System;
using System.Collections.Generic;

public class optionalCourseSetter{
    public static void SetBlock(List<Day> week, TimeSpan[] a, OptionalCourse o){
        foreach(Day d in week){
            if (d.Equals(o.timeSpanDay.day)){
                for(int i=0 ;  i < a.Length; i++){
                    if(a[i].start == o.timeSpanDay.start)
                        o.block = d.blocks[i];
                }
            }
        }        
    } 
}