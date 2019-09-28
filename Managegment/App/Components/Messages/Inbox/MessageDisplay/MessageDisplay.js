
import moment from 'moment';
export default {
    name: 'MessageDisplay',
    props:["ConversationId"],
    created() {
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
        } else {
            window.location.href = '/Security/Login';
        }
        this.GetContentInbox();
        console.log(this.loginDetails.fullName);

    },
    computed: {
        test: function (t) {
            console.log(t);
            for (var i = 0; i < this.contentInbox.attachmentDTOs.length; i++) {
                if (this.contentInbox.attachmentDTOs[i].extension == "pdf") {
                    console.log("pdf");
                    return "fa fa-file-pdf-o";

                }
                if (this.contentInbox.attachmentDTOs[i].extension == "jpg" ||
                    this.contentInbox.attachmentDTOs[i].extension == "png" ||
                    this.contentInbox.attachmentDTOs[i].extension == "jpeg") {
                    console.log("png");
                    return "fa fa-camera";
                }
                if (this.contentInbox.attachmentDTOs[i].extension == "xlsx") {
                    console.log("xlsx");
                    return "fa fa-file-excel-o";
                }
                if (this.contentInbox.attachmentDTOs[i].extension == "docx") {
                    console.log("docx");
                    return "fa fa-file-word-o";
                }
                

            }
            
        }

    },
    data() {
        return {
            replay: false,
            contentInbox: {},
            replayText:"",
            loginDetails: "",
            newReplayMessage: {},
            resultState: false,
        };
    },
    methods: {
        Back() {
            this.$parent.state = 1;
        },
        thereIsReplay() {
            return true;
            //return this.contentInbox.messageDTOs.length > 0
        },
        thereIsAttachment() {
            // return this.contentInbox.attachmentDTOs.length > 0
            return true;
        },
        toggelReplay() {
            this.replayText = "";
            this.replay = !this.replay;
        },
        GetContentInbox() {
            this.$blockUI.Start();
            this.$http.GetContentInbox(this.ConversationId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.contentInbox = response.data.data;
                    console.log(this.contentInbox);
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },
        TestDataReplay()
        {
            if (!this.replayText) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال الرد'
                });
                return;
            }

            this.newReplayMessage = {
                ConversationId: this.ConversationId,
                MessageReplay: this.replayText
            }
            console.log(this.newReplayMessage);
            this.$http.ReplayMessages(this.newReplayMessage)
                .then(response => {
                    console.log("ok");
                    this.resultState = response.data.status;
                    if (this.resultState) {
                        this.contentInbox.messageDTOs.push({
                            subject: this.replayText,
                            dateTime: "الان",
                            userName: this.loginDetails.fullName
                        });
                        this.replayText = "";
                        this.resultState = false;
                    }
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
