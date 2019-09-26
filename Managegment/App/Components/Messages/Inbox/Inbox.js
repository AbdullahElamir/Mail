import MessageDisplay from './MessageDisplay/MessageDisplay.vue';
import { release } from 'os';

export default {
    name: 'Inbox',

    components: {
        'message-Display': MessageDisplay,
    },
    created() {
        this.GetInbox();  
    },
    data() {
        return {
            state: 1,
            inbox: {},
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            isAttachment: "",
            isFavorate: false,
            conversationId: "",
            resultState:false
        
        };
    },
    computed: {
        IsAttachmentComputed: function () {
            this.isAttachment = "fa fa-paperclip";
        },
        isFavorateClassName: function () {
            return (this.isFavorate) ? "inbox-started":" ";
        }
    },
    watch: {
        isFavorate: function (value) {
            
        }
    },
    methods: {
        RedirectToMessageDisplay(conversationId) {
            this.conversationId = conversationId;
            this.state = 2;
        },
        IsFavorateMethod(item, index) {
            let msg="";
            if(!item.isFavorate)
            {
                msg="تم إضافة الي المفضلة";
            }else{
                msg="تم إزالة من المفضلة";
            }
            this.$http.IsFavorate(!item.isFavorate, item.conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState)
                    {
                        this.inbox[index].isFavorate = !item.isFavorate;
                        this.resultState = false;
                        this.showMessage(msg);
                    }
                      
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                    this.resultState = false;
                });
        },
        
        GetInbox() {
            this.pageNo = 1;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetInbox(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.inbox = response.data.inbox;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },
        ArchaveInbox(index, item) {

            let setArchive = !item.isArchive;
            let msg="";
            if(setArchive)
            {
                msg="تم إضافة الي الارشيف";
            }else{
                msg="تم إزالة من الارشيف";
            }
            this.$http.SetArchaveInbox(setArchive, item.conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState)
                    {
                        this.inbox.splice(index, 1);
                        this.showMessage(msg);
                    }
                    this.resultState = false;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                    this.resultState = false;
                });
            },
        DeleteInbox(index, item) {
            let setDelete = true;
            this.$http.DeleteInbox(setDelete, item.conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState)
                        this.inbox.splice(index, 1);
                    this.resultState = false;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                    this.resultState = false;
                });
        },
        showMessage(msg)
        {
            const h = this.$createElement;
            this.$message({
                message: h('p', null, [
                h('span', { style: 'margin-right: 5px' }, msg)
                ])
            });
        },
        ReadInbox(index, item) {
            let readUnRead = !item.isRead;
            this.$http.ReadUnReadInbox(readUnRead, item.conversationID)
                .then(response => {
                    this.$blockUI.Stop();
                    this.resultState = response.data.state;
                    if (this.resultState) {
                        this.inbox[index].isRead = readUnRead;
                        this.inbox[index].messageCountNotRead = 0;
                    }
                   
                    this.resultState = false;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                    this.resultState = false;
                });
        }
    }
}
