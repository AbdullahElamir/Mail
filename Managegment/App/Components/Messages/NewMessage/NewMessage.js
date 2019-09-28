import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'

import { quillEditor } from 'vue-quill-editor'

export default {

    name: 'NewMessage',

    components: {
        quillEditor
    },
    data() {
        return {
            form: {
                from: "",
                users: [],
                selectedusers:[],
                subject: "",
                type: "",
                replay: true,
                priority:"",
                dialogImageUrl: '',
                content: "",
            },
            state: 1,
            list: [],
            editorOption: {
                debug: 'info',
                placeholder: " ",
                readOnly: true,
                theme: 'snow',
                modules: {
                    toolbar: [
                        ['bold', 'italic', 'underline'],
                        ['code-block'],
                        [{ 'color': [] }, { 'background': [] }],
                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                        [{ 'direction': 'rtl' }],
                        [{ 'align': [] }],

                    ],
                },
            },
            types: [],
            loading: false,
            dialogVisible: false,
            Prioritytexts: [' عادي ', ' متوسط ', ' مهم '],
            priorityint: null,
        };
    },
    created() {
        this.remoteMethod(" ");
        this.GetAllAdTypes();
    },

    methods: {
        handleRemove(file, fileList) {
            console.log(file, fileList);
        },
        handlePictureCardPreview(file) {
            this.form.dialogImageUrl = file.url;
            this.form.dialogVisible = true;
        },
        remoteMethod(query) {
            if (query !== '') {
                this.loading = true;
                this.$blockUI.Start();
                this.$http.GetAllUsers()
                    .then(response => {
                        this.loading = false;
                        this.$blockUI.Stop();
                        this.list = response.data.allUsers.map(user => {
                            return { value: user.userId, label: user.userName }
                        });
                        this.form.users = this.list.filter(i => {
                            return i.label.toLowerCase()
                                .indexOf(query.toLowerCase()) > -1;
                        });
                    })
                    .catch((err) => {
                        this.loading = false;
                        this.$blockUI.Stop();
                        console.error(err);
                        this.$message({
                            type: 'error',
                            message: response.data
                        }); 
                    });
            } else {
                this.form.Users = [];
            }
        },
        GetAllAdTypes()
        {
            this.loading = true;
            this.$blockUI.Start();
            this.$http.GetAllAdTypes()
                .then(response => {
                    this.form.loading = false;
                    this.$blockUI.Stop();
                    this.types = response.data.data.map(type => {
                        return { value: type.adTypeId, label: type.adTypeName }
                    });
                     
                })
                .catch((err) => {
                    this.loading = false;
                    this.$blockUI.Stop();
                    console.error(err);
                    this.$message({
                        type: 'error',
                        message: response.data
                    }); 
                });
        },
        SendMessage()
        {
            if (this.form.selectedusers.length < 1)
            {
                this.$message({
                    type: 'error',
                    message: 'الرجاء اختيار علي الاقل مستقبل واحد'
                });
                return;
            }
            if (!this.form.subject) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال العنوان'
                });
                return;
            }
            if (!this.form.type) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء اختيار نوع التعميم'
                });
                return;
            }
            if (!this.form.content) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء ادخال محتوي التعميم'
                });
                return;
            }

            this.form.priority = this.Prioritytexts[this.priorityint - 1];
            console.log(this.form.priority);
            this.$http.NewMessage(this.form)
                .then(response => {
                    this.$message({
                        type: 'info',
                        message: response.data
                    });
                    this.form = [];
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: response.data
                    }); 
                });
        }

    }
}
