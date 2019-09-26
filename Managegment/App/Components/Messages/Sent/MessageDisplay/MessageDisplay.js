export default {
    name: 'MessageDisplay',
    props: ["ConversationId"],
    created() {
        this.GetContentInboxSender();
    },
    data() {
        return {
            replay: false,
            contentInbox: {},
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
        }
      
    }
}
