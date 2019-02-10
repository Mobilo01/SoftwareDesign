using System;
using System.Collections.Generic;
using System.Linq;

public static class Helper {

    public static bool ContainsAllItems<T> (this IEnumerable<T> a, IEnumerable<T> b) {
        return !b.Except (a).Any ();
    }

    public static int CountStudents () {
        return 0;
    }

    public static CourseOfStudy MapCourseOfStudyNameToCourseOfStudy (string courseOfStudyName) {
        foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) {
            if (courseOfStudyName == courseOfStudy.name)
                return courseOfStudy;
        }
        return null;
    }

    public static bool TimeCheck (DateTime a, Days d, Prof p) {
        foreach (TimeSpanDay s in p.occupied) {
            if (s.start < a && s.end > a && d == s.dayName)
                return false;
        }
        return true;
    }

    public static void SetBlock (OptionalCourse o) {
        foreach (Day d in TimetableGenerator.week) {
            if (d.dayName == o.timeSpanDay.dayName) {
                for (int i = 0; i < TimetableGenerator.times.Length; i++) {
                    if (TimetableGenerator.times[i].start == o.timeSpanDay.start) {
                        TimeSpan timespan = new TimeSpan (TimetableGenerator.times[i].start, TimetableGenerator.times[i].end);
                        Block block = new Block (timespan, i, d.dayName);
                        Course course = new Course ();
                        course.name = o.name;
                        course.prof = o.prof;
                        course.room = o.room;

                        block.blockCourses.Add (course);
                        d.blocks[i] = block;
                        foreach (Prof prof in TimetableGenerator.profs) {
                            if (o.prof.name == prof.name)
                                prof.occupied.Add (o.timeSpanDay);
                        }
                        //o.room.roomUsed = true;
                    }
                }
            }
        }
    }
    public static void PrintCourseOfStudy (string courseofStudyName) {

        Console.WriteLine (courseofStudyName + " :");
        Console.WriteLine (" ");
        CourseOfStudy courseOfStudy = MapHelper.MapCourseOfStudyNameToCourseOfStudy (courseofStudyName);
        if (courseOfStudy != null) {
            foreach (Day d in TimetableGenerator.week) {
                foreach (Block b in d.blocks) {
                    foreach (Course bc in b.blockCourses) {
                        foreach (Course cb in courseOfStudy.mandatoryCourses) {
                            if (bc == cb) {
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
            PrintPossibleOpitonalCourses (courseOfStudy);
        } else
            Console.WriteLine ("Falsche Eingabe, oder Studiengang nicht vorhanden, Bsp: 'MIB 2'");
    }

    public static void PrintPossibleOpitonalCourses (CourseOfStudy courseOfStudy) {
        Console.WriteLine ("Mit diesem Stundenplan, kannst du folgende Wahlpflichtkurse belegen");
        
        foreach (Day d in TimetableGenerator.week) {
            foreach (Block b in d.blocks) {
                foreach (OptionalCourse o in TimetableGenerator.optionalCourses) {
                    if (b.blockCourses.Count > 0){
                        foreach (Course bc in b.blockCourses) {
                            if (bc.coursesOfStudy.Count > 0){
                                foreach (CourseOfStudy cos in bc.coursesOfStudy) {
                                    if (!(cos.name == courseOfStudy.name)) {
                                        Console.WriteLine (" ");
                                        if (b.timespan.start == o.timeSpanDay.start &&
                                            d.dayName == o.timeSpanDay.dayName) {
                                            o.shouldPrint = true;
                                            //TimetableGenerator.optionalCourses.Remove(o);
                                        }
                                    }else
                                        Console.WriteLine ("keine");
                                } 
                            }else 
                                o.shouldPrint = true;
                        }
                    }else 
                        o.shouldPrint = true;
                    if(o.shouldPrint)
                        PrintOptionalCourseData(b,d,o);
                }
            }
            
        }   
    }

    public static void PrintOptionalCourseData(Block b, Day day, OptionalCourse o){
        Console.WriteLine (o.name);
        Console.WriteLine (o.room.name + ": ");
        Console.WriteLine ("gehalten von " + o.prof.name);
        Console.WriteLine ("Am " + day.dayName);
        Console.WriteLine ("im " + b.blockNumber + ". Block");
        Console.WriteLine (" ");
    }
}
