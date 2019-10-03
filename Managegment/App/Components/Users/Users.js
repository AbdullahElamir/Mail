import addUsers from './AddUsers/AddUsers.vue';
import editUsers from './EditUsers/EditUsers.vue';
import moment from 'moment';
export default 
{

    name: 'Users',
    
    created() 
    {

        var loginDetails = sessionStorage.getItem('currentUser');
        
        if (loginDetails != null) 
        {
            this.loginDetails = JSON.parse(loginDetails);
            
            if (this.loginDetails.userType != 1) 
            {
                window.location.href = '/Security/Login';
            }
        } 
        else 
        {
            window.location.href = '/Security/Login';
        }
       
        this.GetUsers(this.pageNo);
        
        
        this.permissions = [
                {
                    id: 1,
                    name: "الإدارات"
                },
                {
                    id: 2,
                    name: 'إدارة الفروع'
                },
                {
                    id: 3,
                    name: 'مكاتب اللإصدار'
                },
                {
                    id: 4,
                    name: 'المكاتب الخدمية'
                }

        ];
    
    
    
    },



    components: {
        'add-Users': addUsers,
        'edit-Users': editUsers
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return 'فارغ';
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
            Users: [],
            UserType: '',
            
            state: 0,
            
            EditUsersObj: [],
            AllData: [],
            
            BrachId: '',


            permissions: [],
            permissionModale: [],
            branchesPlaceholder:'',
            Branches: [],
            BrancheModel: [],

        };
    },
    methods: 
    {

        GetBranches()
        {
            this.SetBranchesPlaceholder();
            this.GetUsersByLevel(this.pageNo);

            this.$blockUI.Start();
            this.$http.GetBranchesByLevel(this.permissionModale)

                .then(response => {
                    this.$blockUI.Stop();
                    this.BrancheModel='';
                    this.Branches = response.data.branches;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },

        SetBranchesPlaceholder()
        {
            if(this.permissionModale==1)
            {
                this.branchesPlaceholder='الإدارة';
            }
            else if(this.permissionModale==2)
            {
                this.branchesPlaceholder='الفرع';
            }
            else if(this.permissionModale==3 || this.permissionModale==4)
            {
                this.branchesPlaceholder='المكتب';
            }
        },




        AddUser() {
            this.state = 1;
        },

        ActivateUser(UserId) {
            this.$confirm('سيؤدي ذلك إلى تفعيل المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {


                this.$http.ActivateUser(UserId)
                    .then(response => {
                        if (this.Users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم تفعيل المستخدم بنجاح',
                        });
                        this.GetUsers();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });

        },

        DeactivateUser(UserId) {


            this.$confirm('سيؤدي ذلك إلى ايقاف تفعيل المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {


                this.$http.DeactivateUser(UserId)
                    .then(response => {
                        if (this.Users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم ايقاف التفعيل المستخدم بنجاح',
                        });
                        this.GetUsers();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });
        },




        GetUsers(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();

            this.$http.GetUsers(this.pageNo, this.pageSize, this.UserType)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Users = response.data.user;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();

                    this.pages = 0;
                });
        },

        GetUsersByLevel(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();

            this.$http.GetUsersByLevel(this.pageNo, this.pageSize, this.permissionModale)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Users = response.data.user;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();

                    this.pages = 0;
                });
        },

        GetUserByBranch(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();

            this.$http.GetUserByBranch(this.pageNo, this.pageSize, this.BrancheModel)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Users = response.data.user;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();

                    this.pages = 0;
                });
        },


        EditUser(User) {
            this.state = 2;
            this.EditUsersObj = User;
        },

        DeleteUser(UserId) {
            this.$confirm('سيؤدي ذلك إلى حدف المستخدم  . استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {

                this.$http.DeleteUser(UserId)
                    .then(response => {
                        if (this.Users.lenght === 1) {
                            this.pageNo--;
                            if (this.pageNo <= 0) {
                                this.pageNo = 1;
                            }
                        }
                        this.$message({
                            type: 'info',
                            message: 'تم حدف المستخدم بنجاح',
                        });
                        this.GetUsers();
                    })
                    .catch((err) => {
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            });

        },
    }
};
