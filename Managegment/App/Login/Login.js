export default {
    name: 'login',
    created() {
        this.returnurl = location.pathname;
        this.SetRules();
      

    },
    data() {
        return {
            ForgetPassword:false,
            returnurl: '',
            form: {
                Password: null,
                Email: null
            },
            form2: {
                Email: null
            },
            success: { confirmButtonText: 'OK', type: 'success', dangerouslyUseHTMLString: true, center: true },
            error: { confirmButtonText: 'OK', type: 'error', dangerouslyUseHTMLString: true, center: true },
            warning: { confirmButtonText: 'OK', type: 'warning', dangerouslyUseHTMLString: true, center: true },
            rules: {},
            forgetPassowrd: false,
            


        };
    },
    methods: {
        //Login() {
        //    this.$emit('authenticated');
        //},

                 Send(form) {

            let $blockUI = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
            this.$http.ResetPassword(this.form2.Email.trim())
                .then(response => {
                    $blockUI.close();       
                    this.form2.Email = null;
                    this.forgetPassowrd = false;
                    this.$alert('<h4>' + response.data + '</h4>', '', this.success);
                })
                .catch((error) => {
                    $blockUI.close();       

                    if (error.response.status == 400) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                    } else if (error.response.status == 404) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                    } else {
                        console.log(error.response);
                    }
                });
        

        },

        Save() {
            console.log(this.form)
            let $blockUI = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
            this.$http.loginUserAccount(this.form)
                .then(response => {
                    $blockUI.close();       
                    sessionStorage.setItem('currentUser', JSON.stringify(response.data));
                    //if (response.data.userType == 1) {
                    //    window.location.href = '/Branches';
                    //}    
                    //else if (response.data.userType == 2) {                    
                    //    window.location.href = '/Branches';
                    //}
                  
                    // else {
                    //    window.location.href = '/';                  
                    //}
                   
                   
                })
                .catch((error) => {
                    $blockUI.close()
                    this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                    if (error.response.status == 400) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                    } else if (error.response.status == 404) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                    } else {
                        console.log(error.response);
                    }
                });
        },


        ResetPassword(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    let $blockUI = this.$loading({
                        fullscreen: true,
                        text: 'loading ...'
                    });
                    this.$http.ResetPassword(this.form2.Email.trim())
                    .then(response => {
                        $blockUI.close()                        
                        this.form2.Email = null;
                        this.forgetPassowrd = false;
                        this.$alert('<h4>' + response.data + '</h4>', '', this.success);
                    })
                    .catch((error) => {
                        $blockUI.close()                        

                        if (error.response.status == 400) {
                            this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                        } else if (error.response.status == 404) {
                            this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                        } else {
                            console.log(error.response);
                        }
                    });
                } else {
                    console.log("form not complete");
                    return false;
                }
            });
        },
        SetRules() {

            this.rules = {
                Email: [
                    { required: true, message: 'Please input your Email', trigger: 'blur' },
                    { type: 'email', message: 'Please input correct email address', trigger: ['blur', 'change'] }
                ],
                Password: [
                    { required: true, message: 'Please input your Password', trigger: 'blur' }
                ],
            }
        },
    }
}
