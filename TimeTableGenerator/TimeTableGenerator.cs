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
        
        foreach (Day day in week) {
            day.blocks = new List<Block> ();
            for (int i = 0; i < times.Length; i++) {
                day.blocks.Add (new Block (times[i], i+1, day.dayName));
            }
        }

    }

    public static void GenerateTimetable () {

        foreach (Day day in week) {
            foreach (Block block in day.blocks) {
                foreach (CourseOfStudy courseOfStudy in coursesOfStudy) {
                    
                    foreach (Course course in courseOfStudy.mandatoryCourses) {
                        if (courseOfStudy.cosUsed == false){ 
                            if (course.courseUsed == false) {
                            Helper.CountStudentAmount(course);
                            courseOfStudy.mandatoryCourses = courseOfStudy.mandatoryCourses.OrderByDescending (x => course.studentAmount).ToList();
                                rooms.OrderBy (room => room.seatAmount);    
                                foreach (Room room in rooms) {
                                    if (course.room == null) {
                                        if (room.seatAmount >= course.studentAmount == true) {
                                            if (room != null) {
                                                if (Helper.ContainsAllItems<string> (room.roomEquipment, course.equipment) != false) {
                                                    if (Helper.TimeCheck (block.timespan.start, block.dayName, course.prof) != false) {
                                                        if (course.prof.profUsed == false && room.roomUsed == false) {
                                                            course.room = room;
                                                            course.courseUsed = true;
                                                            course.room.roomUsed = true;
                                                            course.prof.profUsed = true;
                                                            foreach(var coursesOfStudyOfCourse in course.coursesOfStudy){
                                                                foreach(var coursesOfStudy in coursesOfStudy){
                                                                    if(coursesOfStudyOfCourse.name == coursesOfStudy.name){
                                                                        coursesOfStudy.cosUsed = true;
                                                                    }
                                                                }
                                                            }
                                                            block.blockCourses.Add (course);
                                                        }
                                                    }
                                                } 
                                            }
                                        } 
                                    }
                                }
                            }
                            course.studentAmount = 0;
                        }
                    }
                }
                foreach (Course course in block.blockCourses) {
                    course.room.roomUsed = false;
                    course.prof.profUsed = false;
                }
                foreach(var coursesOfStudy in coursesOfStudy)
                    coursesOfStudy.cosUsed = false;
            }
        }
    }

    public static void SortBy(){
       
        Console.WriteLine("Wahlen Sie entweder Dozent, Raume, einen Studiengang(MIB,MKB,OMB) der Wahl mit Semester Bsp: 'OMB 4' oder nichts");
        Console.WriteLine(" ");
        string content = Console.ReadLine();
        string [] console = content.Split(' ');
        string con = console[0]; 
        Console.WriteLine(" ");

        switch(con)
      {
        case "Dozent":
            Console.WriteLine(" ");
            foreach(Prof prof in profs) {
                foreach (Day day in week) {
                    foreach (Block block in day.blocks) {
                        foreach (Course course in block.blockCourses) {
                            if(prof == course.prof){
                                Console.WriteLine (course.prof.name + " :");
                                Console.WriteLine (course.name);
                                Console.WriteLine ("in " + course.room.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "Raume":
            Console.WriteLine(" ");
            foreach(Room room in rooms) {
                foreach (Day day in week) {
                    foreach (Block block in day.blocks) {
                        foreach (Course course in block.blockCourses) {
                            if(room == course.room){
                                Console.WriteLine (course.room.name + ": ");
                                Console.WriteLine ("gehalten von " + course.prof.name);
                                Console.WriteLine (course.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
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
            string coma = " ";
            foreach (Day day in week) {
                foreach (Block block in day.blocks) {
                    foreach (Course course in block.blockCourses) {
                        Console.WriteLine(" ");
                        Console.WriteLine (course.name);
                        Console.WriteLine (course.room.name);
                        Console.WriteLine (course.prof.name);
                        Console.WriteLine (day.dayName);
                        Console.WriteLine (block.blockNumber + ". Block");

                        foreach (var CoursesOfStudyOfCourse in course.coursesOfStudy){
                            if(course.coursesOfStudy.Count > 1)
                                coma = " ";
                            Console.Write (CoursesOfStudyOfCourse.name + coma);
                        }
                        Console.WriteLine (" ");                        
                    }
                }
            }
            break;
        }
    }

    public static void GenerateOptionalTimetable (){
        foreach(OptionalCourse optionalCourse in optionalCourses){
            Helper.SetBlock(optionalCourse);
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
    
    public static void LoadProfsToCourses () {
        foreach (Course course in courses) {
            foreach (Prof prof in profs) {
                foreach (string profCourse in prof.courses) {
                    if (profCourse == course.name)
                        course.prof = prof;
                }
            }
        }
    }

    public static void LoadOptionalCourses (){
        var data = File.ReadAllText ("OptionalCourse.json");
        optionalCourses = JsonConvert.DeserializeObject<List<OptionalCourse>> (data);
    }
}