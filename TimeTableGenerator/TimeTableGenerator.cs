using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        LoadOptionalCourses ();


        Task t = Task.Factory.StartNew(() => {
        Initializer ();

        GenerateOptionalTimetable ();

        GenerateTimetable ();

        SortBy ();
        
        });
        t.Wait(new System.TimeSpan(0, 5, 0));
        if (t.Status == TaskStatus.Running)
        {
            Console.WriteLine("Das Programm läuft schon zu lange und wurde beenden...");
            Console.ReadLine();
            Environment.Exit(0);
        }
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
        
        foreach (Day du in week) {
            du.blocks = new List<Block> ();
            for (int i = 0; i < times.Length; i++) {
                du.blocks.Add (new Block (times[i], i+1, du.dayName));
            }
        }

    }

    public static void GenerateTimetable () {
        var blocks = week.SelectMany (x => x.blocks);

        foreach (Day d in week) {
            foreach (Block b in d.blocks) {
                //b.blockCourses = new List<Course> ();
                foreach (CourseOfStudy cos in coursesOfStudy) {
                    
                    foreach (Course c in cos.mandatoryCourses) {
                        if (cos.cosUsed == false){ 
                            if (c.courseUsed == false) {
                            foreach (var v in coursesOfStudy) {
                                foreach (var t in c.coursesOfStudy) {
                                    if (v.name == t.name)
                                        c.studentAmount = c.studentAmount + v.studentAmount;
                                }
                            }
                            //c.studentAmount = c.coursesOfStudy.Sum (s => s.studentAmount);
                            cos.mandatoryCourses = cos.mandatoryCourses.OrderByDescending (x => c.studentAmount).ToList();
                                rooms.OrderBy (room => room.seatAmount);    
                                foreach (Room r in rooms) {
                                    if (c.room == null) {
                                        if (r.seatAmount >= c.studentAmount == true) {
                                            if (r != null) {
                                                if (Helper.ContainsAllItems<string> (r.roomEquipment, c.equipment) != false) {
                                                    if (Helper.TimeCheck (b.timespan.start, b.day, c.prof) != false) {
                                                        if (c.prof.profUsed == false && r.roomUsed == false) {
                                                            c.room = r;
                                                            c.courseUsed = true;
                                                            c.room.roomUsed = true;
                                                            c.prof.profUsed = true;
                                                            foreach(var s in c.coursesOfStudy){
                                                                foreach(var m in coursesOfStudy){
                                                                    if(s.name == m.name){
                                                                        //s.cosUsed = true;
                                                                        m.cosUsed = true;
                                                                    }
                                                                }
                                                            }
                                                            b.blockCourses.Add (c);
                                                            //c.block = b;
                                                        }
                                                    }
                                                } 
                                            }
                                        } 
                                    }
                                }
                            }
                            c.studentAmount = 0;
                        }
                    }
                    Helper.PrintPossibleOpitonalCourses (cos);
                }
                foreach (Course cb in b.blockCourses) {
                    cb.room.roomUsed = false;
                    cb.prof.profUsed = false;
                }
                foreach(var s in coursesOfStudy)
                    s.cosUsed = false;
            }
        }
    }

    public static void SortBy(){
        //int numb;
        //string test = "MIB 1";
        //CoursesOfStudyPrinter.PrintCourseOfStudy(test,coursesOfStudy,week);
        Console.WriteLine("Wahlen Sie entweder Dozent, Raume, einen Studiengang(MIB,MKB,OMB) der Wahl mit Semester Bsp: 'OMB 4' oder nichts");
        Console.WriteLine(" ");
        string content = Console.ReadLine();
        string [] console = content.Split(' ');
        string con = console[0]; 


       /* string[] dmCoursesOfStudy = new string[3];

        dmCoursesOfStudy[0] = "MIB";
        dmCoursesOfStudy[1] = "OMB";
        dmCoursesOfStudy[2] = "MKB";
        string helper;
        for (int i = 0; i < dmCoursesOfStudy.Length; i++)
        {
            helper = StringHelper.StringContains(con,dmCoursesOfStudy[i]);
            if(con.Contains(dmCoursesOfStudy[i]))
                helper =";
        }   */
            
        //string[] tokens = con.Split(" ");

        //numb = Console.ReadLine[5];
        Console.WriteLine(" ");

        switch(console[0])
      {
        case "Dozent":
            Console.WriteLine(" ");
            foreach(Prof p in profs) {
                foreach (Day d in week) {
                    foreach (Block b in d.blocks) {
                        foreach (Course cb in b.blockCourses) {
                            if(p == cb.prof){
                                //int number = b.blockNumber + 1;
                                Console.WriteLine (cb.prof.name + " :");
                                Console.WriteLine (cb.name);
                                Console.WriteLine ("in " + cb.room.name);
                                Console.WriteLine ("Am " + d.dayName);
                                Console.WriteLine ("im " + b.blockNumber + ". Block");
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
                                //int number = b.blockNumber + 1;
                                Console.WriteLine (cb.room.name + ": ");
                                Console.WriteLine ("gehalten von " + cb.prof.name);
                                Console.WriteLine (cb.name);
                                Console.WriteLine ("Am " + d.dayName);
                                Console.WriteLine ("im " + b.blockNumber + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "MKB":
        case "MIB":
        case "OMB":
            Helper.PrintCourseOfStudy(content);
            break;
        default:
            Console.WriteLine("Gesamtstundeplan");
            Console.WriteLine(" ");
            string coma;
            foreach (Day d in week) {
                foreach (Block b in d.blocks) {
                    foreach (Course cb in b.blockCourses) {
                        Console.WriteLine(" ");

                        coma = " ";
                        //int number = b.blockNumber + 1;
                        Console.WriteLine (cb.name);
                        Console.WriteLine (cb.room.name);
                        Console.WriteLine (cb.prof.name);
                        Console.WriteLine (d.dayName);
                        Console.WriteLine (b.blockNumber + ". Block");

                        foreach (var v in cb.coursesOfStudy){
                            if(cb.coursesOfStudy.Count > 1)
                                coma = " ";
                            Console.Write (v.name + coma);
                        }
                        Console.WriteLine (" ");                        
                    }
                }
            }
            break;
        }
    }

    public static void GenerateOptionalTimetable (){
        foreach(OptionalCourse o in optionalCourses){
            Helper.SetBlock(o);
        }
    }
    public static void LoadJsonRooms () {
        var data = File.ReadAllText ("Rooms2.json");
        rooms = JsonConvert.DeserializeObject<List<Room>> (data);
    }

    public static void LoadJsonProfs () {
        var data = File.ReadAllText ("Profs2.json");
        profs = JsonConvert.DeserializeObject<List<Prof>> (data);
    }

    public static void LoadJsonCourses () {
        var data = File.ReadAllText ("module.json");
        courses = JsonConvert.DeserializeObject<List<Course>> (data);
    }

    public static void LoadJsonCoursesOfStudy () {
        var data = File.ReadAllText ("CoursesOfStudy3.json");
        coursesOfStudy = JsonConvert.DeserializeObject<List<CourseOfStudy>> (data);
    }

    public static void LoadMandatoryCourses () {
        for (int i = 0; i < coursesOfStudy.Count; i++){
            foreach(Course course in courses){
                for(int j = 0; j < coursesOfStudy[i].mandatoryCourses.Count; j++)
                    if(course.name == coursesOfStudy[i].mandatoryCourses[j].name)
                        coursesOfStudy[i].mandatoryCourses[j] = course;
                }    
            }
        }

    /*    foreach(CourseOfStudy cos in coursesOfStudy){
            foreach(Course c in courses){
                foreach(Course cb in cos.mandatoryCourses){
                    if(c.name == cb.name)
                        cos.mandatoryCourses.Add(c);
                }    
            }
        }
        var fastCourses = courses.ToDictionary (x => x.name);
        foreach (var v in coursesOfStudy) {
            v.mandatoryCourses = v.mandatoryCourses
                .Where (x => fastCourses.ContainsKey (x.name))
                .Select (x => fastCourses[x.name])
                .ToList ();
        }   */
    
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
        var data = File.ReadAllText ("OptionalCourse.json");
        optionalCourses = JsonConvert.DeserializeObject<List<OptionalCourse>> (data);
    }
}