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
                Description: '',
                ContinentId:''
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
                    message: 'Please Enter Country in Arabic'
                });
                return;
            }

            if (!this.form.NameEng) {
                this.$message({
                    type: 'error',
                    message: 'Please Enter Country in English'
                });
                return;
            }
            this.form.ContinentId = this.$parent.ContinentId;

            this.$http.AddCountries(this.form)
                .then(response => {
                    this.$parent.state = 0;
                    this.$parent.GetCountries(this.pageNo);
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
