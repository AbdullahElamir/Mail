export default {
    name: 'EditBranches',    
    created() {
       
        var branch = this.$parent.branchEditObject;
        this.form.Name = branch.name;
        this.form.BranchId = branch.branchId;
        this.form.Description = branch.description;
       
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            form: {
                Name: '',
                Description: '' , 
                BranchId: ''  
            },     
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        Edit() {
            if (!this.form.Name) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال اداره الفروع'
                });
                return;
            }

            if (!this.form.Description) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال المعلومات عن اداره الفروع '
                });
                return;
            }


            this.$http.EditBranches(this.form)
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
