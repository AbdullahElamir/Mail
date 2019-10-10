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
                selectedOption: 3,
                files: []

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
            checkAll: false,
            options: ['البريد الإلكتروني', 'رسالة نصية'],
            isIndeterminate: false,
            checkOptions: [],
            selectFiles: []

        };
    },
    created() {
        this.remoteMethod(" ");
        this.GetAllAdTypes();
    },

    methods: {
        handleCheckAllChange(val) {
            
            this.checkOptions = val ? this.options : [];
            this.form.selectedOption = val ? 0 : null;
            this.isIndeterminate = false;
            
        },
        handleCheckedOptionChange(value) {
            
            let checkedCount = value.length;
            this.checkAll = checkedCount === this.options.length;
           
            this.isIndeterminate = checkedCount > 0 && checkedCount < this.options.length;
            if (this.checkAll) {
                this.form.selectedOption = 0;
                return;
            } else if (value[0]== this.options[0]) {
                this.form.selectedOption = 1;
            } else if (value[0]== this.options[1]) {
                this.form.selectedOption = 2;
            } else {
                this.form.selectedOption = 3;
            }

        },
        FileChanged(file, fileList) {
          
            var fileSize = (file.size / 1024) | 0;
            if (fileSize > 8000) 
            {
                this.$message({
                    type: 'error',
                    message: "حجم الملف كبير لايمكن تحميله"
                }); 
                return;
            }
            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file.raw);
            reader.onload = function (e) {
                var obj =
                {
                    fileName: file.raw.name,
                    fileBase64: reader.result,
                    type: file.raw.type
                };
                $this.form.files.push(obj);
                

            }
            console.log($this.form.files);
        },
        handleRemove(file) {
            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file.raw);
            reader.onload = function (e) {
                $this.form.files.splice($this.form.files.indexOf(reader.result), 1);
            }
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
                        this.$message({
                            type: 'error',
                            message: err.message.data
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
                    this.$message({
                        type: 'error',
                        message: err.message.data
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
            this.$blockUI.Start();
            this.form.priority = this.Prioritytexts[this.priorityint - 1];
            this.$http.NewMessage(this.form)
                .then(response => {
                    this.$message({
                        type: 'info',
                        message: response.data
                    });
                    this.form = [];
                    this.$router.push('/Inbox');
                    this.$blockUI.Stop();
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: err.message.data
                    }); 
                    this.$blockUI.Stop();
                });
        }

    }
}
