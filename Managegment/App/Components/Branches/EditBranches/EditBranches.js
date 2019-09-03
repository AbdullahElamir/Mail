export default {
    name: 'EditBranches',    
    created() {
       
        var country = this.$parent.CountryEditObject;
        this.form.Name = country.name;
        this.form.NameEng = country.nameEng;
        this.form.Description = country.description;
       
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            form: {
                Name: '',
                Description: '',
                ContinentId: ''   
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
                    message: 'Please enter country name in arabic'
                });
                return;
            }

            if (!this.form.NameEng) {
                this.$message({
                    type: 'error',
                    message: 'Please enter country name in english'
                });
                return;
            }
            this.form.ContinentId = this.$parent.ContinentId;

            this.$http.EditCountry(this.form)
                .then(response => {
                    this.$parent.state = 0;
                    this.$parent.GetCoursesBySuperPackageId();
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
