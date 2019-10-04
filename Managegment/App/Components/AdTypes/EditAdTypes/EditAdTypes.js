export default {
    name: 'EditAdTypes',    
    created() {
       
        var branch = this.$parent.AdTypeEditObject;
        this.form.AdTypeName = branch.adTypeName;
        this.form.AdTypeId = branch.adTypeId;
       
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            form: {
                AdTypeName: '',
                AdTypeId:''
            },     
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        Edit() {
            if (!this.form.AdTypeName) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال النوع '
                });
                return;
            }


            this.$http.EditAdTypes(this.form)
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
