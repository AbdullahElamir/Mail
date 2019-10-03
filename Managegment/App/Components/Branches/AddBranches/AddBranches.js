export default {
    name: 'AddBranches',    
    created() {
        this.form.BranchLevel=this.$parent.permissionModale;
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            form: {
                Name: '',
                Description: '',
                BranchLevel:0,
            },
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        Save() {
            this.form.BranchLevel=this.$parent.permissionModale;
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
