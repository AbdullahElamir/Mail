import addCourses from './AddCourses/AddCourses.vue';
import editCourses from './EditCourses/EditCourses.vue';
import moment from 'moment';
export default {
    name: 'Courses',    
    created() {
        //console.log(this.$route.params.SuperPackageId)
        //console.log(this.$parent.SuperPackageParent);
        //if (this.$parent.SuperPackageParent==null) {
        //    this.$router.push("/Packages/SuperPackages");
        //}
       // this.GetCoursesBySuperPackageId(this.pageNo);  
        //welcome
    },
    components: {
        'add-Courses': addCourses,
        'edit-Courses': editCourses
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
           // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {
            CourseEdit:[],
            SuperPackageParent: this.$parent.SuperPackageParent,
            pageNo: 1,
            pageSize: 6,
            pages: 0,  
            Courses: [],
            state:0,
          
        };
    },
    methods: {
        EditCourse(Course) {
            this.CourseEdit = Course;
            this.state = 2;
        },
        AddCourse() {
            this.state = 1;
        },

        Back() {
            this.$router.push("/Packages");
        },

        DeleteCourse(courseId) {
            this.$confirm('سيؤدي ذلك إلى حذف الدورة نهائيا. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$http.DeleteCourse(courseId)
                    .then(response => {
                        if (this.Courses.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: "تم مسح الدورة بنجاح"
                        });
                        this.GetCoursesBySuperPackageId(this.pageNo);
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });
        },
       
        GetCoursesBySuperPackageId(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetCoursesBySuperPackageId(this.pageNo, this.pageSize, this.$parent.SuperPackageParent.superPackageId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Courses = response.data.courses;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });

        },
       
    }    
}
