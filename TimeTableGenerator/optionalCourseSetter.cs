using System;
using System.Collections.Generic;

public class optionalCourseSetter{
    public static void SetBlock(OptionalCourse o){
        foreach(Day d in TimetableGenerator.week){
            if (d.dayName == o.timeSpanDay.dayName){
                for(int i=0 ;  i < TimetableGenerator.times.Length; i++){
                    if(TimetableGenerator.times[i].start == o.timeSpanDay.start)
                        o.block = d.blocks[i];
                        o.prof.profUsed = true;
                        o.room.roomUsed = true;
                }
            }
        }        
    } 
}