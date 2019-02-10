using System;
using System.Collections.Generic;

public class CoursesOfStudyPrinter {
    public static void PrintCourseOfStudy (string courseofStudyName, List<CourseOfStudy> cos, List<Day> week) {

        Console.WriteLine (courseofStudyName + " :");
        Console.WriteLine (" ");
        CourseOfStudy courseOfStudy = Helper.MapCourseOfStudyNameToCourseOfStudy (courseofStudyName);
        if (courseOfStudy != null) {
            foreach (Day d in week) {
                foreach (Block b in d.blocks) {
                    foreach (Course bc in b.blockCourses){
                        foreach (Course cb in courseOfStudy.mandatoryCourses) {
                            if(bc == cb){
                                Console.WriteLine (cb.name);
                                Console.WriteLine (cb.room.name + ": ");
                                Console.WriteLine ("gehalten von " + cb.prof.name);
                                Console.WriteLine ("Am " + d.dayName);
                                Console.WriteLine ("im " + b.blockNumber + ". Block");
                                Console.WriteLine (" ");   
                            }    
                        }
                    }
                }
            }
        }else
            Console.WriteLine ("Falsche Eingabe, oder Studiengang nicht vorhanden, Bsp: 'MIB 2'");
    }
}