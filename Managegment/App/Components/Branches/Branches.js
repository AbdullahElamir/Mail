import addBranches from './AddBranches/AddBranches.vue';
import editBranches from './EditBranches/EditBranches.vue';
import moment from 'moment';

export default {
    name: 'Branches',    
    created() {
        this.GetBranches(this.pageNo);
    },
    components: {
        'add-Branches': addBranches,
        'edit-Branches': editBranches
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
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Branches: [],
            state: 0,
        };
    },
    methods: {
        RefreshTheTable() {
            this.GetCountries(this.pageNo);
        },


        RedirectToAddComponent() {
            this.state = 1;
        },

        DeleteBranch(BranchId) {
            this.$confirm('This will permanently delete the Branch. continue?', 'Warning', {
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            type: 'warning'
            }).then(() => {
                this.$http.DeleteBranch(BranchId)
                    .then(response => {    
                        this.$message({
                            type: 'info',
                            message: "Branch has been successfully deleted"
                        });  
                        this.GetBranches(this.pageNo);
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });
        },

        GetBranches(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetBranches(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Branches = response.data.branches;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },
        EditBranch(Country) {
            this.state = 2;
            this.CountryEditObject = Country;
        }

        //EditCourse(Course) {
        //    this.CourseEdit = Course;
        //    this.state = 2;
        //},
        //AddCourse() {
        //    this.state = 1;
        //},

        //Back() {
        //    this.$router.push("/Packages");
        //},

        //DeleteCourse(courseId) {
        //    this.$confirm('سيؤدي ذلك إلى حذف الدورة نهائيا. استمر؟', 'تـحذير', {
        //        confirmButtonText: 'نـعم',
        //        cancelButtonText: 'لا',
        //        type: 'warning'
        //    }).then(() => {
        //        this.$http.DeleteCourse(courseId)
        //            .then(response => {
        //                if (this.Courses.lenght === 1) {
        //                    this.pageNo--;
        //                    if (this.pageNo <= 0) {
        //                        this.pageNo = 1;
        //                    }
        //                }
        //                this.$message({
        //                    type: 'info',
        //                    message: "تم مسح الدورة بنجاح"
        //                });
        //                this.GetCoursesBySuperPackageId(this.pageNo);
        //            })
        //            .catch((err) => {
        //                this.$message({
        //                    type: 'error',
        //                    message: err.response.data
        //                });
        //            });
        //    });
        //},
       
        //GetCoursesBySuperPackageId(pageNo) {
        //    this.pageNo = pageNo;
        //    if (this.pageNo === undefined) {
        //        this.pageNo = 1;
        //    }
        //    this.$blockUI.Start();
        //    this.$http.GetCoursesBySuperPackageId(this.pageNo, this.pageSize, this.$parent.SuperPackageParent.superPackageId)
        //        .then(response => {
        //            this.$blockUI.Stop();
        //            this.Courses = response.data.courses;
        //            this.pages = response.data.count;
        //        })
        //        .catch((err) => {
        //            this.$blockUI.Stop();
        //            console.error(err);
        //            this.pages = 0;
        //        });

        //},
       
    }    
}
