export default {
    name: 'ChangePassword',
    created() {
     

    },
    data() {
        return {
                      form: {
                          Password: '',
              
                          NewPassword: '',
            },
            ConfimPassword: '',
           
       


        };
    },
    methods: {
        validPassword: function (NewPassword) {

            var PasswordT = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/;

            return PasswordT.test(NewPassword);
        },
      
        Save() {


            if (!this.form.Password) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال كلمه المرورالحاليه '
                });
                return;
            }

            if (!this.form.NewPassword) {
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


            if (this.form.NewPassword != this.ConfimPassword) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء التأكد من تطابق الرقم السري'
                });
                return;
            }
            if (this.form.NewPassword.length <= 6) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الرقم السري تحتوي علي سته ارقام '
                });
                return;
            }
            if (!this.validPassword(this.form.NewPassword)) {
                this.$message({
                    type: 'error',
                    message: 'عـفوا : يجب ان يحتوي الرقم السري علي حروف صغيرة وكبيرة وارقام'
                });
                return;
            }
            this.$http.ChangePassword(this.form)
                .then(response => {
                    this.form.NewPassword = '';
                    this.ConfimPassword = '';
                    this.form.Password = '';
                    this.$message({
                        type: 'info',
                        message: 'تم تغير بنجاح',
                    });
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
