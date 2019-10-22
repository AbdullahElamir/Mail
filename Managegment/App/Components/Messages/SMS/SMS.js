export default {
    name: 'SMS',
    created() {

        var loginDetails = sessionStorage.getItem('currentUser');
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
        } else {
            window.location.href = '/Security/Login';
        }
         

        this.getAllMessagesOrFilter();

    },
    data() {

        return {
            form: {
                fromDate: null,
                toDate:null,
                stateTransaction: '4',
                isAllShowMessagess: null,
                searchWithUser: null,
                page: 1,
                PageSize:10
            },
            activeName: '',
            filterUserMessage: [

                {
                    value: "false",
                    label: "عرض الرسائل الخاصة"
                },
                {
                    value: "true",
                    label: "عرض كل الرسائل"
                }
            ],
            loginDetails: {},
            branchId: '',
            ListBranch: [],
            userList: [],
            messageData: [],
            dateFilter: null,
            dialogVisibleDetiales: false,
            modelDateConvartaion: null,
            modelSubject: null,
            modelSubjectBody: null,
            resultState: false,
            pages: 0,

        };
    },

    watch: {
        'form.isAllShowMessagess': function(value) {
            if (value == 'false') {
                this.branchId = '';
                this.form.searchWithUser = null;
              
            } else {
                if (this.ListBranch==0)
                    this.getBranch();
                this.form.searchWithUser = null;
            }
        },
        branchId: function(value) {
            
                this.GetAllUsersByBranch(value);
        }
    },

    methods: {
        getPagintion(page) {
            this.form.page = page;
            this.getAllMessagesOrFilter();
        },

        CleareFilters() {
            this.dateFilter = null;
            this.form.fromDate = null;
            this.form.toDate = null;
            this.form.stateTransaction = '4';
            this.form.isAllShowMessagess =null;
            this.form.searchWithUser = null;
            this.branchId = "";
            this.userList = [];
            this.messageData = [];
           

            // request Backend
            this.getAllMessagesOrFilter();
        },

        getBranch() {
           
            this.$http.GetBranchsV1()
                .then(response => {
                    this.ListBranch = response.data.branches;
                })
                .catch((err) => {

                });
        },

        GetAllUsersByBranch(branchId) {
           
            this.$http.GetAllUsersByBranch(branchId)
                .then(response => {
                    this.userList = response.data.users;
                })
                .catch((err) => {
                    
                });
        },

        getAllMessagesOrFilter() {

            if (this.dateFilter != null) {
                this.form.fromDate = this.dateFilter[0];
                this.form.toDate = this.dateFilter[1];
            } else {
                this.dateFilter = null;
                this.form.fromDate = null;
                this.form.toDate = null;
            }

            

            this.$blockUI.Start();
            this.$http.GetTransactions(this.form)
                .then(response => {
                    this.$blockUI.Stop();
                    this.messageData = response.data.data;
                    this.pages = response.data.countPage;
                     
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message:"error"
                    });
                });
            


        },
        SearchFilter() {
            this.getAllMessagesOrFilter();
        },
        showDetailes(index) {
            this.dialogVisibleDetiales = true;
            this.modelSubject = this.messageData.result[index].subject;
            this.modelSubjectBody = this.messageData.result[index].subjectBody;
            this.modelDateConvartaion= this.messageData.result[index].dateConversation;
          
        },

        replaySendSms(userID, conversationID)
        {

            this.$blockUI.Start();
            this.$http.replaySendSms(userID, conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState) {
                        this.getAllMessagesOrFilter();
                        this.$message({
                            type: 'success',
                            message: "تم ارسال الرسالة الي رقم الهاتف"
                        });
                    } else {
                        this.$message({
                            type: 'error',
                            message: "لم يتم ارسال الرسالة يرجي محاولة لاحقا"
                        });
                    }
                    this.resultState = false;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.resultState = false;
                    this.$message({
                        type: 'error',
                        message: err.message.data
                    });
                });
        },


        replaySendEmail(userID, conversationID) {
            this.$blockUI.Start();
            this.$http.replaySendEmail(userID, conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState) {
                        this.getAllMessagesOrFilter();
                        this.$message({
                            type: 'success',
                            message: "تم ارسال الرسالة الي البريد الالكتروني"
                        });
                    } else {
                        this.$message({
                            type: 'error',
                            message: "لم يتم ارسال الرسالة يرجي محاولة لاحقا"
                        });
                    }
                    this.resultState = false;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.resultState = false;
                    this.$message({
                        type: 'error',
                        message: err.message.data
                    });
                });
        },

    }
}