export default {
    name: 'AddBranches',    
    created() {
       
    },
    data() {
        return {
        
            form: {
                AdTypeName: ''
            
            },
          
         
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        Save() {
            if (!this.form.AdTypeName) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال نوع الرسالة'
                });
                return;
            }


       

            this.$http.AddAdTypes(this.form)
                .then(response => {
                    this.$parent.state = 0;
                    this.$parent.GetAdTypes(this.pageNo);
                    this.$message({
                        type: 'info',
                        message: response.data
                    });
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });
        },
    }    
}
