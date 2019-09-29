import addAdTypes from './AddAdTypes/AddAdTypes.vue';
import editAdTypes from './EditAdTypes/EditAdTypes.vue';
import moment from 'moment';

export default {
    name: 'AdTypes',    
    created() {
        this.GetAdTypes(this.pageNo);
    },
    components: {
        'add-AdTypes': addAdTypes,
        'edit-AdTypes':editAdTypes
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
           // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {  
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            AdTypes: [],
            AdTypeEditObject:[],
            state: 0,
        };
    },
    methods: {
        RefreshTheTable() {
            this.GetAdTypes(this.pageNo);
        },

        RedirectToAddComponent() {
            this.state = 1;
        },
        GetAdTypes(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetAdTypes(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.AdTypes = response.data.adTypes;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },
        EditAdTypes(AdType) {
            this.state = 2;
            this.AdTypeEditObject = AdType;
        }

 
       
    }    
}
