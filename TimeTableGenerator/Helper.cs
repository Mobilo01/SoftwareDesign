using System;
using System.Collections.Generic;
using System.Linq;

public static class Helper {

    public static bool ContainsAllItems<T> (this IEnumerable<T> a, IEnumerable<T> b) {
        return !b.Except (a).Any ();
    }

    public static void CountStudentAmount (Course course) {
        foreach (CourseOfStudy courseOfStudyOfCourse in course.coursesOfStudy) {
            foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) {
                if (courseOfStudyOfCourse.name == courseOfStudy.name)
                    course.studentAmount = course.studentAmount + courseOfStudy.studentAmount;
            }
        }
    }

    public static CourseOfStudy MapCourseOfStudyNameToCourseOfStudy (string courseOfStudyName) {
        foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) {
            if (courseOfStudyName == courseOfStudy.name)
                return courseOfStudy;
        }
        return null;
    }

    public static bool TimeCheck (DateTime dateTime, Days day, Prof prof) {
        foreach (TimeSpanDay timeSpanDay in prof.occupied) {
            if (timeSpanDay.start < dateTime && timeSpanDay.end > dateTime && day == timeSpanDay.dayName)
                return false;
        }
        return true;
    }

    public static void SetBlock (OptionalCourse optionalCourse) {
        foreach (Day day in TimetableGenerator.week) {
            if (day.dayName == optionalCourse.timeSpanDay.dayName) {
                for (int i = 0; i < TimetableGenerator.times.Length; i++) {
                    if (TimetableGenerator.times[i].start == optionalCourse.timeSpanDay.start) {
                        TimeSpan timespan = new TimeSpan (TimetableGenerator.times[i].start, TimetableGenerator.times[i].end);
                        Block block = new Block (timespan, i, day.dayName);
                        Course course = new Course ();
                        course.name = optionalCourse.name;
                        course.prof = optionalCourse.prof;
                        course.room = optionalCourse.room;

                        block.blockCourses.Add (course);
                        day.blocks[i] = block;
                        foreach (Prof prof in TimetableGenerator.profs) {
                            if (optionalCourse.prof.name == prof.name)
                                prof.occupied.Add (optionalCourse.timeSpanDay);
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
            foreach (Day day in TimetableGenerator.week) {
                foreach (Block block in day.blocks) {
                    foreach (Course blockCourseCourse in block.blockCourses) {
                        foreach (Course courseOfMandatoryCourses in courseOfStudy.mandatoryCourses) {
                            if (blockCourseCourse == courseOfMandatoryCourses) {
                                Console.WriteLine (courseOfMandatoryCourses.name);
                                Console.WriteLine (courseOfMandatoryCourses.room.name + ": ");
                                Console.WriteLine ("gehalten von " + courseOfMandatoryCourses.prof.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
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

    public static void PrintPossibleOpitonalCoursesLo (CourseOfStudy courseOfStudy) {
        Console.WriteLine ("Mit diesem Stundenplan, kannst du folgende Wahlpflichtkurse belegen");

        foreach (Day day in TimetableGenerator.week) {
            foreach (Block block in day.blocks) {
                foreach (OptionalCourse optionalCourse in TimetableGenerator.optionalCourses) {
                    if (block.blockCourses.Count > 0) {
                        foreach (Course course in block.blockCourses) {
                            if (course.coursesOfStudy.Count > 0) {
                                foreach (CourseOfStudy cos in course.coursesOfStudy) {
                                    if (!(cos.name == courseOfStudy.name)) {
                                        if (block.timespan.start == optionalCourse.timeSpanDay.start &&
                                            day.dayName == optionalCourse.timeSpanDay.dayName) {
                                            optionalCourse.shouldPrint = true;
                                            break;
                                            //TimetableGenerator.optionalCourses.Remove(o);
                                        }
                                    }
                                    break;
                                }
                            } else
                                optionalCourse.shouldPrint = true;
                        }
                    } else {
                        optionalCourse.shouldPrint = true;
                    }
                    if (optionalCourse.shouldPrint && optionalCourse.optionalCourseUsed == false) {
                        optionalCourse.optionalCourseUsed = true;
                        PrintOptionalCourseData (block, day, optionalCourse);

                    } else
                        Console.WriteLine ("keine");
                    //break;
                }
                //break;
            }
            //break;
        }
    }

    public static void PrintPossibleOpitonalCourses (CourseOfStudy courseOfStudy) {
        bool key = true;
        Console.WriteLine ("Mit diesem Stundenplan, koenntest du folgende Wahlpflichtkurse belegen");
        Console.WriteLine (" ");
        foreach (Day day in TimetableGenerator.week) {
            foreach (Block block in day.blocks) {
                foreach (OptionalCourse optionalCourse in TimetableGenerator.optionalCourses) {
                    if (block.blockCourses.Count == 0 && optionalCourse.optionalCourseUsed == false) {
                        if (CheckBlockToCourse (block, optionalCourse.timeSpanDay)) {
                            optionalCourse.optionalCourseUsed = true;
                            PrintOptionalCourseData (block, day, optionalCourse);
                        }
                    } else {
                        foreach (Course course in block.blockCourses) {
                            if (block.blockCourses.Count == 0)
                                break;
                            key = true;
                            for (int i = 0; i < course.coursesOfStudy.Count; i++) {
                                CourseOfStudy courseOfStudyOfCourses = course.coursesOfStudy[i];
                                if (courseOfStudyOfCourses.name == courseOfStudy.name || course.coursesOfStudy == null) {
                                    key = false;
                                }
                            }

                            if (key && optionalCourse.optionalCourseUsed == false) {
                                if (CheckBlockToCourse (block, optionalCourse.timeSpanDay)) {
                                    PrintOptionalCourseData (block, day, optionalCourse);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static bool CheckBlockToCourse (Block block, TimeSpanDay timeSpanDayCourse) {
        if (block.timespan.start == timeSpanDayCourse.start &&
            block.dayName == timeSpanDayCourse.dayName) {
            return true;
        }
        return false;
    }

    public static void PrintOptionalCourseData (Block block, Day day, OptionalCourse optionalCourse) {
            int i = block.blockNumber + 1;
            Console.WriteLine (optionalCourse.name);
            Console.WriteLine (optionalCourse.room.name + ": ");
            Console.WriteLine ("gehalten von " + optionalCourse.prof.name);
            Console.WriteLine ("Am " + day.dayName);
            Console.WriteLine ("im " + i + ". Block");
            Console.WriteLine (" ");
        }
}