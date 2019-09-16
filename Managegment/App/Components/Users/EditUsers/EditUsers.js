export default {
    name: 'EditUser',    
    created() {
        console.log(this.$parent.EditUsersObj);
        this.form.FullName = this.$parent.EditUsersObj.fullName;
        this.form.LoginName = this.$parent.EditUsersObj.loginName;
        this.form.Phone = this.$parent.EditUsersObj.phone;
        this.form.Email = this.$parent.EditUsersObj.email;
        this.form.Gender = this.$parent.EditUsersObj.gender;
        this.form.DateOfBirth = this.$parent.EditUsersObj.dateOfBirth;
        this.form.UserId = this.$parent.EditUsersObj.userId;
        this.PermissionModale = this.$parent.EditUsersObj.userType;
        this.OfficeTypeModel = this.$parent.EditUsersObj.officeType;
        this.Hospital = this.$parent.EditUsersObj.hospitalId;
        this.form.SearchDeathsQouta = this.$parent.EditUsersObj.searchDeathsQouta;
        if (this.$parent.EditUsersObj.searchByRegistryNumber == 1) { this.form.SearchByRegistryNumber = true;}
        if (this.$parent.EditUsersObj.searchByNationalId == 1) { this.form.SearchByNationalId = true; }
        if (this.$parent.EditUsersObj.searchByName == 1) { this.form.SearchByName = true; }
        this.Hospital = this.$parent.EditUsersObj.hospitalId;
        if (this.$parent.EditUsersObj.deathInfoPrivilege == 1) { this.form.DeathInfoPrivilege = true; }
        if (this.$parent.EditUsersObj.deathEntryPrivilege == 1) { this.form.DeathEntryPrivilege = true; }
        this.form.UserType = this.$parent.EditUsersObj.userType;
        if (this.$parent.EditUsersObj.userType == 5) {
            this.form.HospitalId = this.$parent.EditUsersObj.hospitalId;
        }
        this.Office = this.$parent.EditUsersObj.officeName;
        this.AllOffice = this.GetAllOfficesByType(this.OfficeTypeModel);
        // change after login
        this.OfficeType = [
            {
                id: 1,
                name: "ادارة الفروع"
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
                name: 'البحث العام'
            },
      {
                id: 5,
                name: 'الوفيات'
            }
        ];


    },
    data() {
       
        return {
           
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            isFromSelect: true,
            Users: [],
            Permissions: [],
            AllOffice: [],
            state: 0,
            OfficeTypeModel: null,
            Office: [],
            OfficePaceholder: '',
            OfficeType: [],
            PermissionModale: [],
            Hospital: this.SelectHospital(),
            form: {
                HospitalId: '',
                LoginName: '',
                Phone: '',
                Password: '',
                FullName: '',
                UserType: '',
                Email: '',
                Gender: '',
                DateOfBirth: '',
                OfficeId: '',
                SearchByRegistryNumber: false,
                SearchByNationalId: false,
                SearchByName: false , 
                  SearchDeathsQouta: 0,
                DeathInfoPrivilege: false,
                DeathEntryPrivilege: false
               
            },
            ConfimPassword: ''
        
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },
        SelectHospital() {
           
            this.$http.SelectHospital()
                .then(response => {
                    this.Hospital = response.data.hospital;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });

        },
        validPhone: function (Phone) {

            var mobileRegex = /^09[1|2|3|4|5][0-9]{7}$/i;

            return mobileRegex.test(Phone);
        },

        validEmail: function (email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        },
        validLoginName: function (LoginName) {
            var login = /^[a-zA-Z]{0,40}$/;
            return login.test(LoginName);
        },
        validFullName: function (FullName) {
            var loginArabic = /^[\u0621-\u064A\u0660-\u0669 ]+$/;
            return loginArabic.test(FullName);
        },

     
        GetAllOfficesByType(OfficeType) {
            this.$blockUI.Start();
            this.$http.GetAllOfficesByType(OfficeType)
                .then(response => {
                    this.$blockUI.Stop();
                    if (this.isFromSelect) {
                        this.form.OfficeId = this.$parent.EditUsersObj.officeId;
                    }
                   
                    this.AllOffice = response.data.office;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        SelectedOfficeTypeFun() {
            this.form.OfficeId = '';
            this.isFromSelect = false;
            if (this.OfficeTypeModel == 1) {
                this.OfficePaceholder = 'ادارة الفروع';
                this.Office = '';
                
                this.GetAllOfficesByType(1);

            } else if (this.OfficeTypeModel == 2) {
                this.OfficePaceholder = 'مكاتب الإصدار';

                this.Office = '';
                this.GetAllOfficesByType(2);
            }  else {
                this.OfficePaceholder = 'مكاتب السجلات';
                this.Office = '';
                this.GetAllOfficesByType(4);
            }
        },

        SelectedPermissionFun() {
            if (this.PermissionModale == 1) {
                this.persmissonLable = 'الـمدير';
            } else if (this.PermissionModale == 2) {
                this.persmissonLable = 'الشهائد العام';
            } else if (this.PermissionModale == 3) {
             
                this.persmissonLable = 'المكاتب';
          
        } else if(this.PermissionModale == 4) {
    this.persmissonLable = 'البحت العام';
}else {
    this.persmissonLable = 'الوفيات';
}


        },


        Edit(){
            //if (!this.form.LoginName) {
            //    this.$message({
            //        type: 'error',
            //        message: 'الـرجاء إدخال اسم الدخول'
            //    });
            //    return;
            //} else if (!this.validLoginName(this.form.LoginName)) {
            //    this.$message({
            //        type: 'error',
            //        message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه '
            //    });
            //    return;
            //}
         

            //if (!this.form.FullName) {
            //    this.$message({
            //        type: 'error',
            //        message: 'الرجاء إدخال الاسم التلاثي '
            //    });
            //    return;
            //} else if (!this.validFullName(this.form.FullName)) {
            //    this.$message({
            //        type: 'error',
            //        message: 'الرجاء إدخال الاسم التلاثي بطريقه صحيحه '
            //    });
            //    return;
            //}

            if (!this.form.Email) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني '
                });
                return;
            } else if (!this.validEmail(this.form.Email)) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه '
                });
                return;
            }

            if (!this.form.Gender) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إختيار الجنس '
                });
                return;
            }

            if (!this.form.DateOfBirth) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إختيار تاريخ الميلاد '
                });
                return;
            }


            if (!this.form.Phone) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء رقم الهاتف '
                });
                return;
            } else if (!this.validPhone(this.form.Phone)) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال رقم الهاتف  بطريقه صحيحه '
                });
                return;
            }
         
            if (this.$parent.EditUsersObj.userType == 5) {
                if (!this.form.SearchDeathsQouta) {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message: 'الرجاء ادخال العدد المتاح للبحت'
                    });
                    return;
                }
            }
           
            
            if (this.$parent.EditUsersObj.userType == 5) {
                if (!this.form.HospitalId) {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message: 'الرجاء اختيار المستشفي'
                    });
                    return;
                }
            }
            else {
                this.form.HospitalId = '';
            }
            console.log(this.form);
            this.$http.EditUser(this.form)
                .then(response => {
                    
                    this.$message({
                        type: 'info',
                        message: response.data
                    });
                    this.$parent.NID = '';
                    this.UserType = '';
                    this.$parent.EditUsersObj = [];
                   // this.$parent.PermissionModale = '';
                    this.$parent.GetUsers(this.pageNo);
                  this.$parent.state = 0;
                 
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });
        }




    }    
}
