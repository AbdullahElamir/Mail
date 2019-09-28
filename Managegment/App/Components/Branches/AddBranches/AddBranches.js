export default {
    name: 'AddBranches',    
    created() {
       
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            form: {
                Name: '',
                Description: ''
            },
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        Save() {
            if (!this.form.Name) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال اسم المكتب'
                });
                return;
            }

            if (!this.form.Description) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال تفاصيل'
                });
                return;
            }
       

            this.$http.AddBranches(this.form)
                .then(response => {
                    this.$parent.state = 0;
                    this.$parent.GetBranches(this.pageNo);
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
