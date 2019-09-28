
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

        console.log(this.contentInbox);
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
                    console.log(this.contentInbox);
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
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
           // moment(String(new Date().toISOString().slice(0, 10))).format('ddd, DD MMM YYYY H:mm A'),
          //  console.log(this.newReplayMessage);
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
