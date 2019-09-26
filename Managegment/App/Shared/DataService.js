import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');



export default {
    //Login(loginName, password, secretNo) {
    //    return axios.post(baseUrl + '/security/login', { loginName, password, secretNo });
    //},
    //Logout() {
    //    return axios.post(baseUrl + '/security/logout');
    //},    
    //CheckLoginStatus() {
    //    return axios.post('/security/checkloginstatus');
    //},  
  
    //*******************************************  Branches Service *********************************
    GetBranches(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branches/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
      DeleteBranch(BracnhId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
          return axios.post(`/Api/Admin/Branches/${BracnhId}/delete`);
    },
    AddBranches(Branch) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post( `/Api/Admin/Branches/Add`, Branch);
    },

    EditBranches(Branch) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branches/Edit`, Branch);
    },
    GetInbox(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/GetAllInbox?page=${pageNo}&pagesize=${pageSize}`);
    },
    IsFavorate(isFavorate, conversationId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/EnabelDisplayFavorate?isFavorate=${isFavorate}&conversationId=${conversationId}`);
    },
    SetArchaveInbox(setArchive, conversationId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/Archive?isArchive=${setArchive}&conversationId=${conversationId}`);
    },
    DeleteInbox(isDelete, conversationId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/DeleteConversation?isDelete=${isDelete}&conversationId=${conversationId}`);
    },
    ReadUnReadInbox(isReadUnRead, conversationId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/ReadUnReadInbox?isReadUnRead=${isReadUnRead}&conversationId=${conversationId}`);
    },
    GetContentInbox(conversationId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/getContentConversation?conversationId=${conversationId}`);
    },
    GetSent(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Messages/GetAllInboxSender?page=${pageNo}&pagesize=${pageSize}`);
    },



}