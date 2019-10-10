
import moment from 'moment';
import { release } from 'os';
export default {
    name: 'MessageDisplay',
    props: ["ConversationId"],
    created() {
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
        } else {
            window.location.href = '/Security/Login';
        }
        this.GetContentInboxSender();
    },
    data() {
        return {
            replay: false,
            contentInbox: {},
            replayText: "",
            loginDetails: "",
            newReplayMessage: {},
            resultState: false,
        };
    },
    methods: {
        forceFileDownload(response, item) {
            const url = window.URL.createObjectURL(new Blob([response.data]))
            const link = document.createElement('a')
            link.href = url
            link.setAttribute('download', item.fileName)  
            document.body.appendChild(link)
            link.click()
        },
        downloadFile(item) {
            this.$blockUI.Start();
            this.$http.downloadFile(item.attachmentId).then(response => {
                this.$blockUI.Stop();
                this.forceFileDownload(response, item);
            })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });
        },
        Back() {
            this.$parent.state = 1;
        },
        toggelReplay() {
            
            this.replay = !this.replay;
        },
        GetContentInboxSender() {
            this.$blockUI.Start();
            this.$http.GetContentInbox(this.ConversationId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.contentInbox = response.data.data;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.pages = 0;
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });
        },
        thereIsReplay() {
            return true;
            //return this.contentInbox.messageDTOs.length > 0
        },
        thereIsAttachment() {
            // return this.contentInbox.attachmentDTOs.length > 0
            return true;
        },
        TestDataReplay() {
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
         
            this.$http.ReplayMessages(this.newReplayMessage)
                .then(response => {
                    this.resultState = response.data.status;
                    if (this.resultState) {
                        this.contentInbox.messageDTOs.push({
                            subject: this.replayText,
                            dateTime: "الان",
                            userName: this.loginDetails.fullName,
                            imageUser: this.loginDetails.photo
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
