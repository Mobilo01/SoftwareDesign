public class MapHelper {
    public static CourseOfStudy MapCourseOfStudyNameToCourseOfStudy (string courseOfStudyName) {
        foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) {
            if (courseOfStudyName == courseOfStudy.name)
                return courseOfStudy;  
        }
        return null;
    }   
}