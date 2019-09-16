import addUsers from './AddUsers/AddUsers.vue';
import editUsers from './EditUsers/EditUsers.vue';
import moment from 'moment';
export default {
    name: 'Users',
    created() {

        
        //this.GetAllOffices();
        // permission depending on login user

        //var loginDetails = sessionStorage.getItem('currentUser');
        //if (loginDetails != null) {
        //    this.loginDetails = JSON.parse(loginDetails);
        //    if (this.loginDetails.userType == 1 || (this.loginDetails.userType == 3 && this.loginDetails.officeType == 1)) {       
        //    } else {
        //        window.location.href = '/Security/Login';
        //    }
        //} else {  
        //    window.location.href = '/Security/Login';
        //}

        if (this.loginDetails.userType == 1) {

            this.Permissions = [
                {
                    id: 1,
                    name: "المدير"
                },
                {
                    id: 2,
                    name: 'الشهائد العام'
                },
                {
                    id: 3,
                    name: 'المكاتب'
                },
                {
                    id: 4,
                    name:'البحث العام'
                },
                {
                    id: 5,
                    name: 'إيقاف متوفي'
                }

            ];
            this.GetUsers(this.pageNo);
        } else {
            this.PermissionModale = 3;
            this.UserType = 3;
            this.GetUsers(this.pageNo);
            this.Permissions = [
                {
                    id: 3,
                    name: 'المكاتب'
                }
            ];

        }


        if (this.loginDetails.userType == 1) {
            // change after login
            this.OfficeType = [
                {
                    id: 1,
                    name: "ادارة فروع"
                },
                {
                    id: 2,
                    name: 'مكتب الاصدار'
                },
             

                {
                    id: 4,
                    name: 'مكتب السجلات'
                }
            ];
        }
        else {
            this.OfficeType = [
                {
                    id: 2,
                    name: 'مكتب الاصدار'
                },
             

                {
                    id: 4,
                    name: 'مكتب السجلات'
                }];

        }

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
            UserType: 0,
            Permissions: [],
            AllOffice: [],
            state: 0,
            printData:[],
            Authorizer:'',
            OfficeTypeModel: null,
            Office: [],
            dialogFormVisible: false,
            dialogForm:false,
            OfficePaceholder: '',
            OfficeType: [],
            officeIdV: 0,
            officeTypeV:0,
            PermissionModale: [],
            EditUsersObj: [],
            NID: '',
         
            AllData: [],
            LoginInfo: [],
            Password: null,
          
            ConfimPassword: null,
            form2: {
                NewPassword: null,
                UserId: 0,
            }, 
            form: {
                RecipientName: '',
                LoginInfoId:0,
                Recipient: '',
                SenderName: '',
                Sender: '',
                AuthorizerName: '',
                Authorizer: ''       
            }, 
        };
   
    },
    methods: {
        validPassword: function (NewPassword) {

            var PasswordT = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/;

            return PasswordT.test(NewPassword);
        },

        update() {


         

            if (!this.form2.NewPassword) {
                this.$message({
                    type: 'error',
                    message: '  الرجاء إدخال كلمه المرور الجديده '
                });
                return;
            }
            if (!this.ConfimPassword) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال تأكيد كلمه المرور جديده '
                });
                return;
            }


            if (this.form2.NewPassword != this.ConfimPassword) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء التأكد من تطابق الرقم السري'
                });
                return;
            }
            if (this.form2.NewPassword.length <= 6) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الرقم السري تحتوي علي سته ارقام '
                });
                return;
            }
            if (!this.validPassword(this.form2.NewPassword)) {
                this.$message({
                    type: 'error',
                    message: 'عـفوا : يجب ان يحتوي الرقم السري علي حروف صغيرة وكبيرة وارقام'
                });
                return;
            }
          
            this.$http.ChangePassword(this.form2)
           
                .then(response => {
                    this.NewPassword = '';
                    this.ConfimPassword = '';
                 
                    this.$message({
                        type: 'info',
                        message: 'تم تعيين كلمة المرور بنجاح',
                    });
                    this.dialogForm= false;
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });



        },
        changePassword(userid) {
            this.dialogForm = true;
            this.form2.UserId = userid;
          

        },
        Save() {
            if (!this.form.RecipientName) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال اسم المستلم '
                });
                return;
            }
            if (!this.form.Recipient) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال اسم صفه المستلم '
                });
                return;
            }
            if (!this.form.SenderName) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال اسم المسلم '
                });
                return;
            }
            if (!this.form.Sender) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال صفة المسلم '
                });
                return;
            }
            if (!this.form.Authorizer) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال صفة المعتمد '
                });
                return;
            }
            if (!this.form.AuthorizerName) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال اسم المعتمد '
                });
                return;
            }
            if (!this.Password) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال كلمة المرور '
                });
                return;
            }
            var i = 0;
            if (this.LoginInfo.length != 0) {
                if ((this.LoginInfo["0"].authorizer != this.form.Authorizer)
                    || (this.LoginInfo["0"].authorizerName != this.form.AuthorizerName) ||
                    (this.LoginInfo["0"].sender != this.form.Sender) || (this.LoginInfo["0"].senderName != this.form.SenderName) ||
                    (this.LoginInfo["0"].recipient != this.form.Recipient) || (this.LoginInfo["0"].recipientName != this.form.RecipientName)) {
                    i++;
                }

            }
          
  
            if (i != 0) {
                this.form.LoginInfoId = this.LoginInfo["0"].loginInfoId;
                this.$http.UpdateLoginInfo(this.form)
                    .then(response => {
                        this.$parent.state = 0;
                   
                        window.open("Reports/PasswordReports?Name=" + this.printData.fullName + "&UserName=" + this.printData.loginName + "&password=" + this.Password + "&UserType=" + this.printData.userType + "&AuthorizerName=" + this.form.AuthorizerName + "&Authorizer=" + this.form.Authorizer + "&Sender=" + this.form.Sender + "&SenderName=" + this.form.SenderName + "&RecipientName=" + this.form.RecipientName + "&Recipient=" + this.form.Recipient, "_self");
                        this.dialogFormVisible = false;
                        this.$blockUI.Stop();

                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });
            }
            else if (this.LoginInfo.length == 0) {
                this.$http.AddLoginInfo(this.form)
                    .then(response => {
                        this.$parent.state = 0;
                     
                        window.open("Reports/PasswordReports?Name=" + this.printData.fullName + "&UserName=" + this.printData.loginName + "&password=" + this.Password + "&UserType=" + this.printData.userType + "&AuthorizerName=" + this.form.AuthorizerName + "&Authorizer=" + this.form.Authorizer + "&Sender=" + this.form.Sender + "&SenderName=" + this.form.SenderName + "&RecipientName=" + this.form.RecipientName + "&Recipient=" + this.form.Recipient, "_self");
                        this.dialogFormVisible = false;
                        this.$blockUI.Stop();

                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        this.$message({
                            type: 'error',
                            message: err.response.data
                        });
                    });

            }
            else {
                window.open("Reports/PasswordReports?Name=" + this.printData.fullName + "&UserName=" + this.printData.loginName + "&password=" + this.Password + "&UserType=" + this.printData.userType + "&AuthorizerName=" + this.form.AuthorizerName + "&Authorizer=" + this.form.Authorizer + "&Sender=" + this.form.Sender + "&SenderName=" + this.form.SenderName + "&RecipientName=" + this.form.RecipientName + "&Recipient=" + this.form.Recipient, "_self");
                this.dialogFormVisible = false;



            }

        },
        SelectedPermissionFun() {
            if (this.PermissionModale == 1) {
                this.persmissonLable = 'الـمدير';
            } else if (this.PermissionModale == 2) {
                this.persmissonLable = 'الشهائد العام';
            } else if (this.PermissionModale == 3) {
                this.persmissonLable = 'المـكاتب';
            } else if (this.PermissionModale == 4) {
                this.persmissonLable = 'البحت العام';
            }else {
                this.persmissonLable = 'إيقاف متوفي';
            }
            this.OfficeTypeModel = null;
            this.officeTypeV = 0;
            this.officeIdV = 0;
            this.UserType = this.PermissionModale;
            this.pageNo = 1;
            this.GetUsers(this.pageNo);
            // this.$http.GetUsers(this.pageNo, this.pageSize, this.UserType)
        },
        print(User) {
       this.dialogFormVisible = true;
            this.printData = User;
       this.GetLoginInfo();
     
       this.LoginInfo = this.form;

        },

        GetLoginInfo() {
            this.$blockUI.Start();
            this.$http.GetLoginInfo()

                .then(response => {
                   this.$blockUI.Stop();
                    this.LoginInfo = response.data.loginInfo;
                
                    if ( this.LoginInfo.length != 0)  {
                    this.form.Authorizer = this.LoginInfo["0"].authorizer;
                    this.form.AuthorizerName = this.LoginInfo["0"].authorizerName;
                    this.form.Recipient = this.LoginInfo["0"].recipient;
                    this.form.RecipientName = this.LoginInfo["0"].recipientName;
                    this.form.Sender = this.LoginInfo["0"].sender;
                    this.form.SenderName = this.LoginInfo["0"].senderName;}
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                 
                });
        },
        AddUser() {

            var nationalNoRegex = /^[1-2][1-2][0-9][0-9][0-9]\d\d\d\d\d\d\d$/i;
           
            
            if (!this.NID) {
                this.$message({
                    type: 'error',
                    message: 'الـرجاء إدخال الرقم الوطني'
                });
                return;
            }
            else if (!nationalNoRegex.test(this.NID)) {
                this.$message({
                    type: 'error',
                    message: 'الـرجاء إدخال الرقم الوطني بطريقه صحيحه'
                }); return;
            }
            this.DataPersonal(this.NID);
         
  
        },
          
        DataPersonal(NID) {
            this.$blockUI.Start();
            this.$http.DataPersonal(NID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.AllData = response.data.userData;
                    if (this.AllData === undefined || this.AllData.length == 0) {
                     
                        this.$message({
                            type: 'error',
                            message: 'الرجاء التاكد من بيانات'
                        });
                    }
                   else {
                        this.state = 1;
                    }
                
                    
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: 'خطا'
                    });

                });
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

        SelectedOfficeId() {
            this.officeIdV = this.Office;
            this.GetUsers();
        },
  
        SelectedOfficeTypeFun() {
            if (this.OfficeTypeModel == 1) {
                this.OfficePaceholder = 'ادارة الفروع';
                this.Office = '';
                this.GetAllOfficesByType(1);
                this.officeIdV = 0;
                this.officeTypeV = 1;
                this.GetUsers();

            } else if (this.OfficeTypeModel == 2) {
                this.OfficePaceholder = 'مكاتب الإصدار';

                this.Office = '';
                this.GetAllOfficesByType(2);
                this.officeIdV = 0;
                this.officeTypeV = 2;
                this.GetUsers();
            }  else {
                this.OfficePaceholder = 'مكاتب السجلات';
                this.Office = '';
                this.GetAllOfficesByType(4);
                this.officeIdV = 0;
                this.officeTypeV = 4;
                this.GetUsers();
            }


        },

        GetUsers(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();

            this.$http.GetUsers(this.pageNo, this.pageSize, this.UserType, this.officeTypeV,this.officeIdV)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Users = response.data.user;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        GetAllOfficesByType(OfficeType) {
            //this.$blockUI.Start();
            this.$http.GetAllOfficesByType(OfficeType)

                .then(response => {
                    //this.$blockUI.Stop();
                    this.AllOffice = response.data.office;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    //this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        EditUser(User) {
            this.state = 2;
            this.EditUsersObj = User;

        },



    }
};
