using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TimetableGenerator {
    public static List<Day> week;
    public static TimeSpan[] times;
    public static List<Course> courses;
    public static List<Room> rooms;

    public static List<Prof> profs;
    public static List<CourseOfStudy> coursesOfStudy;
    static void Main (string[] args) {
        LoadJsonRooms ();
        LoadJsonProfs ();
        LoadJsonCoursesOfStudy ();
        LoadJsonCourses ();
        LoadMandatoryCourses ();
        LoadProfsToCourses ();

        Initializer ();

        GenerateTimetable ();

    }

    public static void Initializer () {

        week = new List<Day> ();
        week.Add (new Day (Days.Montag));
        week.Add (new Day (Days.Dienstag));
        week.Add (new Day (Days.Mittwoch));
        week.Add (new Day (Days.Donnerstag));
        week.Add (new Day (Days.Freitag));

        times = new TimeSpan[6];
        times[0] = new TimeSpan (new DateTime (2019, 01, 01, 7, 45, 0), new DateTime (2019, 01, 01, 9, 15, 0));
        times[1] = new TimeSpan (new DateTime (2019, 01, 01, 9, 30, 0), new DateTime (2019, 01, 01, 11, 0, 0));
        times[2] = new TimeSpan (new DateTime (2019, 01, 01, 11, 15, 0), new DateTime (2019, 01, 01, 12, 45, 0));
        times[3] = new TimeSpan (new DateTime (2019, 01, 01, 14, 0, 0), new DateTime (2019, 01, 01, 15, 30, 0));
        times[4] = new TimeSpan (new DateTime (2019, 01, 01, 15, 45, 0), new DateTime (2019, 01, 01, 17, 15, 0));
        times[5] = new TimeSpan (new DateTime (2019, 01, 01, 17, 30, 0), new DateTime (2019, 01, 01, 19, 0, 0));

    }

    public static void GenerateTimetable () {
        var blocks = week.SelectMany (x => x.blocks);

        foreach (Day du in week) {
            du.blocks = new List<Block> ();
            for (int i = 0; i < times.Length; i++) {
                du.blocks.Add (new Block (times[i], i, du));
                du.blocks[i].blockNumber = i;;
            }

        }
        foreach (Day d in week) {
            foreach (Block b in d.blocks) {
                foreach (CourseOfStudy cos in coursesOfStudy) {
                    foreach (Course c in cos.mandatoryCourses) {
                        foreach (var v in c.coursesOfStudy) {
                            foreach (var t in coursesOfStudy) {
                                if (v.name == t.name)
                                    c.studentAmount = c.studentAmount + t.studentAmount;
                            }
                        }
                        //c.studentAmount = c.coursesOfStudy.Sum (s => s.studentAmount);
                        cos.mandatoryCourses.OrderByDescending (x => c.studentAmount);
                        if (c.courseUsed != true) {
                            rooms.OrderBy (rum => rum.seatAmount);
                            foreach (Room r in rooms) {
                                if (r.roomUsed == false) {
                                    if (r.seatAmount >= c.studentAmount == true) {
                                        //c.room = r;
                                        if (r != null) {
                                            if (ListHelper.ContainsAllItems<string> (r.roomEquipment, c.equipment) != false) {
                                                if (TimeHelper.TimeCheck (b.timespan.start, b.day.day, c.prof) != false) {
                                                    if (c.prof.profUsed == false) {
                                                        c.room = r;
                                                        c.courseUsed = true;
                                                        c.room.roomUsed = true;
                                                        c.prof.profUsed = true;

                                                        Console.WriteLine (c.name);
                                                        Console.WriteLine (c.description);
                                                        Console.WriteLine ("in " + c.room.name);
                                                        Console.WriteLine ("unterrichtet von " + c.prof.name);
                                                        Console.WriteLine ("für " + c.studentAmount + " Leute");
                                                        Console.WriteLine ("im " + b.blockNumber + "");
                                                        foreach (var v in c.coursesOfStudy)
                                                            Console.WriteLine ("für " + v.name);
                                                        b.blockCourses.Add (c);
                                                    }
                                                }
                                            } else
                                                Console.WriteLine (c.name + ": Der Raum hat das entsprechende Material nicht.");
                                        }
                                    } else
                                        Console.WriteLine (c.name + ": Der Raum ist nicht groß genug.");
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    public static void LoadJsonRooms () {
        var data = File.ReadAllText ("Rooms.json");
        rooms = JsonConvert.DeserializeObject<List<Room>> (data);
    }

    public static void LoadJsonProfs () {
        var data = File.ReadAllText ("Profs.json");
        profs = JsonConvert.DeserializeObject<List<Prof>> (data);
    }

    public static void LoadJsonCourses () {
        var data = File.ReadAllText ("Courses.json");
        courses = JsonConvert.DeserializeObject<List<Course>> (data);
    }

    public static void LoadJsonCoursesOfStudy () {
        var data = File.ReadAllText ("CoursesOfStudy.json");
        coursesOfStudy = JsonConvert.DeserializeObject<List<CourseOfStudy>> (data);
    }

    public static void LoadMandatoryCourses () {
        var fastCourses = courses.ToDictionary (x => x.name);
        foreach (var v in coursesOfStudy) {
            v.mandatoryCourses = v.mandatoryCourses
                .Where (x => fastCourses.ContainsKey (x.name))
                .Select (x => fastCourses[x.name])
                .ToList ();
        }
    }
    public static void LoadProfsToCourses () {
        foreach (Course c in courses) {
            foreach (Prof p in profs) {
                foreach (string s in p.courses) {
                    if (s == c.name)
                        c.prof = p;
                }
            }
        }
    }
}