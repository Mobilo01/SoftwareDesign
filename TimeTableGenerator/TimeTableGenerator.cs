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
    public static List<OptionalCourse> optionalCourses;
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
       // LoadOptionalCourses ();

        Initializer ();

        //GenerateOptionalTimetable ();

        GenerateTimetable ();

        SortBy ();

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
                List<Course> blockCourses = new List<Course> ();
                b.blockCourses = blockCourses;
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
                                                        
                                                        //int number = b.blockNumber + 1;

                                                        /*Console.WriteLine (c.name);
                                                        //Console.WriteLine (c.description);
                                                        Console.WriteLine ("in " + c.room.name);
                                                        Console.WriteLine ("unterrichtet von " + c.prof.name);
                                                        //Console.WriteLine ("fur " + c.studentAmount + " Leute");
                                                        Console.WriteLine ("Am " + d.day);
                                                        Console.WriteLine ("im " + number + ". Block");
                                                        //foreach (var v in c.coursesOfStudy)
                                                        //    Console.WriteLine ("fur " + v.name);
                                                        Console.WriteLine (" ");*/
                                                        b.blockCourses.Add (c);
                                                    }
                                                }
                                            } //else
                                             //   Console.WriteLine (c.name + ": Der Raum " + r.name + " hat das entsprechende Material nicht.");
                                        }
                                    } //else
                                     //   Console.WriteLine (c.name + ": Der Raum " + r.name + " ist nicht gross genug.");
                                }
                            }
                        }
                        c.studentAmount = 0;
                    }
                }
                foreach (Course cb in b.blockCourses) {
                    cb.room.roomUsed = false;
                    cb.prof.profUsed = false;
                }
            }
        }
    }

    public static void SortBy(){
        Console.WriteLine("Wahlen Sie entweder Dozent, Raume, MIB, MKB, OMB oder nichts");
        Console.WriteLine(" ");
        string con = Console.ReadLine();
        Console.WriteLine(" ");

        switch(con)
      {
        case "Dozent":
            Console.WriteLine(" ");
            foreach(Prof p in profs) {
                foreach (Day d in week) {
                    foreach (Block b in d.blocks) {
                        foreach (Course cb in b.blockCourses) {
                            if(p == cb.prof){
                                int number = b.blockNumber + 1;
                                Console.WriteLine (cb.prof.name + " :");
                                Console.WriteLine (cb.name);
                                Console.WriteLine ("in " + cb.room.name);
                                Console.WriteLine ("Am " + d.day);
                                Console.WriteLine ("im " + number + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "Raume":
            Console.WriteLine(" ");
            foreach(Room r in rooms) {
                foreach (Day d in week) {
                    foreach (Block b in d.blocks) {
                        foreach (Course cb in b.blockCourses) {
                            if(r == cb.room){
                                int number = b.blockNumber + 1;
                                Console.WriteLine (cb.room.name + ": ");
                                Console.WriteLine ("gehalten von " + cb.prof.name);
                                Console.WriteLine (cb.name);
                                Console.WriteLine ("Am " + d.day);
                                Console.WriteLine ("im " + number + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "MKB":
            Console.WriteLine("Case 2");
            break;
        case "MIB":
            Console.WriteLine("Case 2");
            break;
        case "OMB":
            Console.WriteLine("Case 2");
            break;
        default:
            Console.WriteLine("Gesamtstundeplan");
            Console.WriteLine(" ");
            string coma;
            foreach (Day d in week) {
                foreach (Block b in d.blocks) {
                    foreach (Course cb in b.blockCourses) {
                        coma = "";
                        Console.WriteLine (" ");
                        int number = b.blockNumber + 1;
                        Console.WriteLine (cb.name);
                        Console.WriteLine (cb.room.name);
                        Console.WriteLine (cb.prof.name);
                        Console.WriteLine (d.day);
                        Console.WriteLine (number + ". Block");
                        foreach (var v in cb.coursesOfStudy){
                            if(coursesOfStudy.Count > 1)
                                coma = ", ";
                            Console.Write (v.name + coma);
                        }
                    }
                }
            }
            break;
        }
    }

    public static void GenerateOptionalTimetable (){
        foreach(var v in optionalCourses){

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

    public static void LoadOptionalCourses (){
        var data = File.ReadAllText ("OptionalCourses.json");
        optionalCourses = JsonConvert.DeserializeObject<List<OptionalCourse>> (data);
    }
}